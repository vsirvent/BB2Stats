using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Xml;
using System.Threading;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using BB2Stats.schemas.board_action;
using BB2Stats.schemas.coach_choice;
using BB2Stats.schemas.end_turn;
using BB2Stats.schemas.forced_dices;
using BB2Stats.schemas.waiting_request;
using BB2Stats.schemas.full_state;
using BB2Stats.schemas.header;
using Ionic.Zlib;
using schemas.full_state;

namespace BB2Stats
{
    public interface SnifferListener
    {
        void OnRulesEventBoardAction(RulesEventBoardAction rulesEventBoardAction);
        void OnRulesEventCoachChoice(RulesEventCoachChoice rulesEventCoachChoice);
        void OnRulesEventEndTurn(RulesEventEndTurn rulesEventEndTurn);
        void OnRulesEventFullState(RulesEventFullState rulesEventFullState);
        void OnRulesEventWaitingRequest(RulesEventWaitingRequest rulesEventWaitingRequest);
        void OnRulesEventForcedDices(RulesEventForcedDices rulesEventForcedDices);        
    };

    public class Sniffer
    {

        static Sniffer p = null;
            
        EventWaitHandle data_processed = new EventWaitHandle(true, EventResetMode.ManualReset);
        EventWaitHandle data_ready = new EventWaitHandle(false, EventResetMode.ManualReset);
        Thread t = null;
        Thread t2 = null;
        int good_frames = 0;
        int bad_frames = 0;
        int header_length = 0;
        int body_length = 0;
        string zipped = "no";
        List<MemoryStream> frame_list = new List<MemoryStream>();
        Mutex mutex = new Mutex();
        SnifferListener listener = null;
        List<String> eth_names = new List<String>();
        Dictionary<String, LivePacketDevice> eth_map = new Dictionary<String, LivePacketDevice>();
        int file_count = 0;
        private PacketCommunicator communicator;
        bool is_running = false;
        class DataPacket
        {
            public uint last_sq = 0;
            public MemoryStream buff = new MemoryStream();
        };
        Dictionary<KeyValuePair<int, int>, DataPacket> buffer_map = new Dictionary<KeyValuePair<int, int>, DataPacket>();
        private string eth_name = "";
        private bool stop_worker;

        public Sniffer()
        {
            p = this;
            t2 = new Thread(new ThreadStart(ThreadProc2));

            // Retrieve the device list from the local machine
            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;

            if (allDevices.Count == 0)
            {
                Console.WriteLine("No interfaces found! Make sure WinPcap is installed.");
                return;
            }

            // Print the list
            for (int i = 0; i != allDevices.Count; ++i)
            {
                LivePacketDevice device = allDevices[i];
                Console.Write((i + 1) + ". " + device.Name);
                if (device.Description != null)
                {
                    if (!eth_map.ContainsKey(device.Description))
                    {
                        string key = device.Description;
                        if (device.Addresses.Count > 0)
                        {
                            foreach (var address in device.Addresses) {
                                if (address.Address.Family == SocketAddressFamily.Internet)
                                {
                                    key += "(" + address.ToString() + ")";
                                    break;
                                }
                            }
                           
                        } 
                        eth_map.Add(key, device);
                        eth_names.Add(key);
                    }
                }
                else
                {
                    Console.WriteLine(" (No description available)");
                }
            }
            t2.Start();
        }

        public List<String> GetDevices()
        {
            return eth_names;
        }

        public void SetDevice(String eth_name)
        {
            this.eth_name = eth_name;
        }
        public void SetListener(SnifferListener listener)
        {
              this.listener = listener;
        }

        public void GetStats(out int good_frames, out int total_frames)
        {
            good_frames = this.good_frames;
            total_frames = this.good_frames + this.bad_frames;
        }

        public static void ThreadProc()
        {
            p.NetWorker();
        }

        public static void ThreadProc2()
        {
            p.BgWorker();
        }

        public void NetWorker()
        {
            Packet packet;
            while (!stop_worker)
            {
                if (communicator.ReceivePacket(out packet) == PacketCommunicatorReceiveResult.Ok)
                {
                    OnDataReceived(packet);
                }
            }
        }

        public void BgWorker()
        {
            while (true)
            {
                data_ready.WaitOne();
                data_ready.Reset();
                while (true)
                {
                    mutex.WaitOne();
                    if (frame_list.Count == 0)
                    {
                        mutex.ReleaseMutex();
                        break;
                    }
                    MemoryStream frame = frame_list[0];
                    mutex.ReleaseMutex();
                    if (frame != null)
                    {
                        frame.Seek(0, SeekOrigin.Begin);
                        header_length = (frame.ReadByte()) | (frame.ReadByte() << 8) | (frame.ReadByte() << 16) | (frame.ReadByte() << 24);
                        if (ParseHeader(frame) > 0)
                        {
                            if (body_length <= (frame.Length - frame.Position))
                            {
                                ProcessData(frame);
                                good_frames++;
                            }
                            else
                            {
                                System.Console.WriteLine("BAD MESSAGE: to_read = " + body_length + ", available = " + (frame.Length - frame.Position));
                                bad_frames++;
                            }
                        }
                        System.Console.WriteLine("Frames " + good_frames + "/" + (bad_frames + good_frames));
                        header_length = 0;
                        body_length = 0;
                        zipped = "";
                    }
                    mutex.WaitOne();
                    frame_list.Remove(frame);
                    mutex.ReleaseMutex();
                }
                data_processed.Set();
            }
        }

        public void Run()
        {
            if (eth_name != "")
            {
                // Take the selected adapter
                PacketDevice selectedDevice = eth_map[eth_name];
                // Open the device
                communicator =
                    selectedDevice.Open(2000,                                  // portion of the packet to capture
                                                                               // 65536 guarantees that the whole packet will be captured on all the link layers
                                        PacketDeviceOpenAttributes.Promiscuous, // promiscuous mode
                                        1000);                                  // read timeout
                {
                    // Check the link layer. We support only Ethernet for simplicity.
                    if (communicator.DataLink.Kind != DataLinkKind.Ethernet)
                    {
                        Console.WriteLine("This Sniffer works only on Ethernet networks.");
                        return;
                    }
                    communicator.SetKernelBufferSize(1024 * 1024 * 128);
                    //communicator.SetKernelMinimumBytesToCopy(10*1024);
                    // Compile the filter
                    communicator.SetFilter(communicator.CreateFilter("tcp and tcp src portrange 21000-22000"));
                    Console.WriteLine("Listening on " + selectedDevice.Description + "...");
                    stop_worker = false;
                    // start the capture
                    t = new Thread(new ThreadStart(ThreadProc));
                    t.Start();
                    is_running = true;
                }
            }
        }

        internal void Stop()
        {
            stop_worker = true;
            is_running = false;
        }

        public bool IsRunning()
        {
            return is_running;
        }

        private int ParseHeader(MemoryStream data)
        {
            int read = 0;
            System.Text.StringBuilder header = new System.Text.StringBuilder();
            while (data.Position < data.Length && read < header_length && header.ToString().IndexOf("</Header>") < 0)
            {
                header.Append((char)data.ReadByte());
                read++;
            }
            string header_str = header.ToString();
            //System.Console.WriteLine(header_str);   
            XmlReader reader = XmlTextReader.Create(new System.IO.StringReader(header_str));
            XmlSerializer serializer = new XmlSerializer(typeof(Header));
            Header header_data = (Header)serializer.Deserialize(reader);
            if (header_data.Items.Length > 0)
            {
                zipped = header_data.Items[0].zipped;
                body_length = int.Parse(header_data.Items[0].size);
            }
            else
            {
                System.Console.WriteLine("Header parse error");
            }
            return read;
        }
                
        private int ProcessData(MemoryStream data)
        {
            int read = 0;
            MemoryStream raw = new MemoryStream();
            if (zipped == "yes")
            {
                try
                {
                    var decompressor = new ZlibStream(data, Ionic.Zlib.CompressionMode.Decompress);
                    decompressor.CopyTo(raw);
                    read += (int)raw.Length;
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Zlib failed");
                }
            }
            else if (zipped == "no")
            {
                while (data.Position < data.Length && read < body_length)
                {
                    raw.WriteByte((byte)data.ReadByte());
                    read++;
                }
            }
            else
            {
                System.Console.WriteLine("Bad header parse");
            }
            System.Text.StringBuilder body = new System.Text.StringBuilder();
            raw.Seek(0, SeekOrigin.Begin);
            var utf8 = Encoding.UTF8;
            byte[] utfBytes = raw.ToArray();

            string body_str = utf8.GetString(utfBytes, 0, utfBytes.Length);
            //string path = ".\\full_" + file_count++ + ".xml";
            //File.WriteAllText(path, body_str);
            //System.Console.WriteLine(" " + body_length + "/" + body_str.Length + " => " + body_str.Substring(body_str.Length - 10));
            XmlReader reader = XmlTextReader.Create(new System.IO.StringReader(body_str));
            if (body_str.StartsWith("<RulesEventFullState>"))
            {
                XmlSerializer rulesEventFullStateSerializer = new XmlSerializer(typeof(RulesEventFullState));
                RulesEventFullState rulesEventFullState = (RulesEventFullState)rulesEventFullStateSerializer.Deserialize(reader);
                if (listener != null)
                {
                    listener.OnRulesEventFullState(rulesEventFullState);
                }
                //System.Console.WriteLine("RulesEventFullState received");
                RulesEventFullStateMessages msgs = (RulesEventFullStateMessages)rulesEventFullState.Items[1];
                if (msgs.StringMessage != null)
                {
                    for (int i = 0; i < msgs.StringMessage.Length; ++i)
                    {
                        var msg = msgs.StringMessage[i];
                        //System.Console.WriteLine("Message name: " + msg.Name);
                        //{
                        //    string path = ".\\" + msg.Name + "_" + file_count++ + ".xml";
                        //    System.Console.WriteLine("Event received, saving to " + path + "...");
                        //    File.WriteAllText(path, msg.MessageData);
                        //}
                        if (msg.Name == "RulesEventBoardAction")
                        {
                            StringReader string_reader = new StringReader(msg.MessageData);
                            XmlReader action_reader = XmlReader.Create(string_reader);
                            XmlSerializer actionBoardSerializer = new XmlSerializer(typeof(RulesEventBoardAction));
                            RulesEventBoardAction actionBoard = (RulesEventBoardAction)actionBoardSerializer.Deserialize(action_reader);
                            //System.Console.WriteLine("Action received from player " + actionBoard.PlayerId + " action type " + actionBoard.ActionType);
                            if (listener != null)
                            {
                                listener.OnRulesEventBoardAction(actionBoard);
                            }
                        }
                        else if (msg.Name == "RulesEventCoachChoice")
                        {
                            StringReader string_reader = new StringReader(msg.MessageData);
                            XmlReader choice_reader = XmlReader.Create(string_reader);
                            XmlSerializer choiceSerializer = new XmlSerializer(typeof(RulesEventCoachChoice));
                            RulesEventCoachChoice rulesEventCoachChoice = (RulesEventCoachChoice)choiceSerializer.Deserialize(choice_reader);
                            //System.Console.WriteLine("RulesEventCoachChoice received");
                            if (listener != null)
                            {
                                listener.OnRulesEventCoachChoice(rulesEventCoachChoice);
                            }
                        }
                        else if (msg.Name == "RulesEventEndTurn")
                        {
                            StringReader string_reader = new StringReader(msg.MessageData);
                            XmlReader end_turn_reader = XmlReader.Create(string_reader);
                            XmlSerializer endSerializer = new XmlSerializer(typeof(RulesEventEndTurn));
                            RulesEventEndTurn rulesEventEndTurn = (RulesEventEndTurn)endSerializer.Deserialize(end_turn_reader);
                            //System.Console.WriteLine("RulesEventEndTurn received");
                            if (listener != null)
                            {
                                listener.OnRulesEventEndTurn(rulesEventEndTurn);
                            }
                        }
                        else if (msg.Name == "RulesEventForcedDices")
                        {
                            StringReader string_reader = new StringReader(msg.MessageData);
                            XmlReader forced_dices_reader = XmlReader.Create(string_reader);
                            XmlSerializer forcedSerializer = new XmlSerializer(typeof(RulesEventForcedDices));
                            RulesEventForcedDices rulesEventForcedDices = (RulesEventForcedDices)forcedSerializer.Deserialize(forced_dices_reader);
                            //System.Console.WriteLine("RulesEventForcedDices received");
                            if (listener != null)
                            {
                                listener.OnRulesEventForcedDices(rulesEventForcedDices);
                            }
                        }
                        else if (msg.Name == "RulesEventWaitingRequest")
                        {
                            StringReader string_reader = new StringReader(msg.MessageData);
                            XmlReader waiting_reader = XmlReader.Create(string_reader);
                            XmlSerializer waitingSerializer = new XmlSerializer(typeof(RulesEventWaitingRequest));
                            RulesEventWaitingRequest rulesEventWaitingRequest = (RulesEventWaitingRequest)waitingSerializer.Deserialize(waiting_reader);
                            //System.Console.WriteLine("RulesEventWaitingRequest received");
                            if (listener != null)
                            {
                                listener.OnRulesEventWaitingRequest(rulesEventWaitingRequest);
                            }
                        }
                        else
                        {                            
                            string path = ".\\" + msg.Name + "_" + file_count++ + ".xml";
                            System.Console.WriteLine("Unknown event received, saving to " + path + "...");
                            //File.WriteAllText(path, msg.MessageData);
                        }
                    }
                }
            }
            return read;
        }

        private List<int> getHeaderIndex(MemoryStream data)
        {
            List<int> headerIndex = new List<int>();
            data.Seek(0, SeekOrigin.Begin);
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            while (data.Length > data.Position)
            {
                str.Append((char)data.ReadByte());
            }
            int index = 0;
            while (true)
            {
                index = str.ToString().IndexOf("<Header>", index);
                if (index > 0)
                {
                    //System.Console.WriteLine("Index found " + index + " => " + str.ToString().Substring(index, 40));
                    headerIndex.Add(index - 4);
                }
                else
                {
                    break;
                }
                index += 8;
            }
            data.Seek(0, SeekOrigin.Begin);
            return headerIndex;
        }

        private void OnDataReceived(Packet packet)
        {
            // print timestamp and length of the packet
            IpV4Datagram ip = packet.Ethernet.IpV4;
            TcpDatagram tcp = ip.Tcp;
            if (tcp.PayloadLength == 0)
            {
                return;
            }
            //System.Console.WriteLine("sq:" + tcp.SequenceNumber + ", " + ip.Source + ":" + tcp.SourcePort + ":" + tcp.Payload.Length + " -> " + ip.Destination + ":" + tcp.DestinationPort);
            var key = new KeyValuePair<int, int>(tcp.SourcePort, tcp.DestinationPort);
            if (!buffer_map.ContainsKey(key))
            {
                buffer_map.Add(key, new DataPacket());
            }
            if (null != tcp.Payload && tcp.PayloadLength > 0 && tcp.SequenceNumber > buffer_map[key].last_sq)
            {
                buffer_map[key].last_sq = tcp.SequenceNumber;
                //Put payload to buffer end
                MemoryStream ms = tcp.Payload.ToMemoryStream();
                MemoryStream buffer = buffer_map[key].buff;
                buffer.Seek(0, SeekOrigin.End);
                ms.WriteTo(buffer);
                List<int> index = null;
                //Get current buffer indexes
                index = getHeaderIndex(buffer);

                //If we find at least 2 index we have a full frame to process
                if (index.Count > 1)
                {
                    //System.Console.WriteLine("index[0]: " + index[0] + ", index[1]: " + index[1]);
                    //Copy from first index to next index -1
                    //in a new net_data memory
                    MemoryStream net_data = new MemoryStream();
                    buffer.Seek(index[0], SeekOrigin.Begin);
                    while (buffer.Position < index[1])
                    {
                        net_data.WriteByte((byte)buffer.ReadByte());
                    }
                    //Now we have a full frame in net_data
                    //and we can process it
                    //Index always starts at 0
                    //Wait for background worker to 
                    //process last frame
                    mutex.WaitOne();
                    frame_list.Add(net_data);
                    mutex.ReleaseMutex();
                    data_ready.Set();
                    //Now update buffer to remove processed data
                    MemoryStream temp = new MemoryStream();
                    while (buffer.Position < buffer.Length)
                    {
                        temp.WriteByte((byte)buffer.ReadByte());
                    }
                    buffer = temp;
                    buffer_map[key].buff = buffer;
                }
            }
        }
    }
}

