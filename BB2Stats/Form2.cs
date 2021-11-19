using BB2Stats.schemas.board_action;
using BB2Stats.schemas.coach_choice;
using BB2Stats.schemas.end_turn;
using BB2Stats.schemas.forced_dices;
using BB2Stats.schemas.full_state;
using BB2Stats.schemas.waiting_request;
using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Formatter;
using MQTTnet.Protocol;
using schemas.full_state;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;

namespace BB2Stats
{
    public partial class BBStatsMain : Form, SnifferListener
    {
        BB2StatsForm team1 = null;
        BB2StatsForm team2 = null;
        OverlayTeam ovTeam1 = null;
        OverlayTeam ovTeam2 = null;
        ConfigForm configForm = null;
        OverlayBG ovBg = null;
        IManagedMqttClient managedMqttClientPublisher;
        IManagedMqttClient managedMqttClientSubscriber;
        string mqttId = "";
        bool publishMqtt = false;
        bool subscribeMqtt = false;
        string mqttIdSubs = "";
        Sniffer sniffer = null;
        
        public BBStatsMain()
        {
            mainClass = this;
            InitializeComponent();
            origBounds = this.Bounds;
            b1Pos = minimize.Location;
            b2Pos = show.Location;
            b3Pos = settings.Location;

            sniffer = new Sniffer();
            sniffer.SetListener(this);

            configForm = new ConfigForm(sniffer);
            team1 = new BB2StatsForm();
            team2 = new BB2StatsForm();
            ovTeam1 = new OverlayTeam(1);
            ovTeam2 = new OverlayTeam(2);
            ovBg = new OverlayBG();
            
            team1.TopLevel = false;
            team2.TopLevel = false;
            ovTeam1.TopLevel = false;   
            ovTeam2.TopLevel = false;
            team1.MouseDown += Form1_MouseDown;
            team1.MouseUp += Form1_MouseUp;
            team1.MouseMove += Form1_MouseMove;
            team2.MouseDown += Form1_MouseDown;
            team2.MouseUp += Form1_MouseUp;
            team2.MouseMove += Form1_MouseMove;
            settings.BringToFront();
            turnCheck.Checked = true;
            this.panel1.Controls.Add(team1);
            this.panel2.Controls.Add(team2);
            Rectangle screenBounds = Screen.PrimaryScreen.Bounds;
            this.Location = new Point((screenBounds.Width - this.Width)/ 2, 0);
            ovBg.Location = new Point(0, 0);
            ovBg.Size = new Size(screenBounds.Width, screenBounds.Height);

            _hookID = SetHook(_proc);
            timer1.Start();
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLongW")]
        private static extern IntPtr SetWindowLongPtr32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        private bool IsFormBeingDragged = false;
        private Point OrigPosition;
        private Point MouseDownPosition;
        private bool stopWorker = false;

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;
        private static BBStatsMain mainClass = null;
        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(
            int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                Console.WriteLine(vkCode + ":" + wParam + ":" + lParam);
                if (vkCode == 0x09)
                {
                    mainClass.pictureBox2_Click(null, new EventArgs());
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private async void MqttConnectPublisher(string id)
        {
            mqttId = id;
            var mqttFactory = new MqttFactory();

            var tlsOptions = new MqttClientTlsOptions
            {
                UseTls = false,
                IgnoreCertificateChainErrors = true,
                IgnoreCertificateRevocationErrors = true,
                AllowUntrustedCertificates = true
            };

            var options = new MqttClientOptions
            {
                ClientId = "ClientPublisher",
                ProtocolVersion = MqttProtocolVersion.V311,
                ChannelOptions = new MqttClientTcpOptions
                {
                    Server = "broker.hivemq.com",
                    Port = 1883,
                    TlsOptions = tlsOptions
                }
            };

            if (options.ChannelOptions == null)
            {
                throw new InvalidOperationException();
            }

            options.CleanSession = true;
            options.KeepAlivePeriod = TimeSpan.FromSeconds(5);
            this.managedMqttClientPublisher = mqttFactory.CreateManagedMqttClient();
            this.managedMqttClientPublisher.UseApplicationMessageReceivedHandler(this.HandleReceivedApplicationMessage);
            this.managedMqttClientPublisher.ConnectedHandler = new MqttClientConnectedHandlerDelegate(OnPublisherConnected);
            this.managedMqttClientPublisher.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(OnPublisherDisconnected);

            await this.managedMqttClientPublisher.StartAsync(
                new ManagedMqttClientOptions
                {
                    ClientOptions = options
                });
            publishMqtt = true;
        }

        private async void MqttDisconnectPublisher()
        {
            publishMqtt = false;
            if (this.managedMqttClientPublisher == null)
            {
                return;
            }

            await this.managedMqttClientPublisher.StopAsync();
            this.managedMqttClientPublisher = null;
        }

        private async void MqttStartSubscribe(string id)
        {
            mqttId = id;
            mqttIdSubs = id;
            var mqttFactory = new MqttFactory();
            var tlsOptions = new MqttClientTlsOptions
            {
                UseTls = false,
                IgnoreCertificateChainErrors = true,
                IgnoreCertificateRevocationErrors = true,
                AllowUntrustedCertificates = true
            };

            var options = new MqttClientOptions
            {
                ClientId = "ClientSubscriber",
                ProtocolVersion = MqttProtocolVersion.V311,
                ChannelOptions = new MqttClientTcpOptions
                {
                    Server = "broker.hivemq.com",
                    Port = 1883,
                    TlsOptions = tlsOptions
                }
            };

            if (options.ChannelOptions == null)
            {
                throw new InvalidOperationException();
            }

            options.CleanSession = true;
            options.KeepAlivePeriod = TimeSpan.FromSeconds(5);

            this.managedMqttClientSubscriber = mqttFactory.CreateManagedMqttClient();
            this.managedMqttClientSubscriber.ConnectedHandler = new MqttClientConnectedHandlerDelegate(OnSubscriberConnected);
            this.managedMqttClientSubscriber.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(OnSubscriberDisconnected);
            this.managedMqttClientSubscriber.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(this.OnSubscriberMessageReceived);

            await this.managedMqttClientSubscriber.StartAsync(
                new ManagedMqttClientOptions
                {
                    ClientOptions = options
                });
            string topic = "bb2stats/" + id + "/#";
            MqttTopicFilter topicFilter = new MqttTopicFilter { Topic = topic };
            await this.managedMqttClientSubscriber.SubscribeAsync(topicFilter);
            System.Console.WriteLine("Topic " + topic + " is subscribed");
            subscribeMqtt = true;
        }
        private void MqttStopSubscribe()
        {
            string topic = "bb2stats/" + mqttIdSubs + "/#";
            managedMqttClientSubscriber.UnsubscribeAsync(topic);
            subscribeMqtt = false;
        }
        private void OnSubscriberMessageReceived(MqttApplicationMessageReceivedEventArgs x)
        {
            string topic1 = "bb2stats/" + mqttIdSubs + "/team1";
            string topic2 = "bb2stats/" + mqttIdSubs + "/team2";
            var item = $"Timestamp: {DateTime.Now:O} | Topic: {x.ApplicationMessage.Topic} | Payload: {x.ApplicationMessage.ConvertPayloadToString()} | QoS: {x.ApplicationMessage.QualityOfServiceLevel}";
            System.Console.WriteLine(item);
            if (x.ApplicationMessage.Topic == topic1)
            {
                team1.Invoke((MethodInvoker)delegate { team1.fromJson(x.ApplicationMessage.ConvertPayloadToString()); });
                ovTeam1.Invoke((MethodInvoker)delegate { ovTeam1.fromJson(x.ApplicationMessage.ConvertPayloadToString()); });
            }
            else if (x.ApplicationMessage.Topic == topic2)
            {
                team2.Invoke((MethodInvoker)delegate { team2.fromJson(x.ApplicationMessage.ConvertPayloadToString()); });
                ovTeam2.Invoke((MethodInvoker)delegate { ovTeam2.fromJson(x.ApplicationMessage.ConvertPayloadToString()); });
            }
        }

        private async void OnPublish()
        {
            try
            {
                if (this.managedMqttClientPublisher != null)
                {
                    var payload = Encoding.UTF8.GetBytes(team1.toJson());
                    var topic = "bb2stats/" + mqttId + "/team1";
                    var message = new MqttApplicationMessageBuilder().WithTopic(topic).WithPayload(payload).WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce).WithRetainFlag().Build();
                    await this.managedMqttClientPublisher.PublishAsync(message);
                    payload = Encoding.UTF8.GetBytes(team2.toJson());
                    topic = "bb2stats/" + mqttId + "/team2";
                    message = new MqttApplicationMessageBuilder().WithTopic(topic).WithPayload(payload).WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce).WithRetainFlag().Build();
                    await this.managedMqttClientPublisher.PublishAsync(message);
                    System.Console.WriteLine("Mqtt published");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Occurs", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HandleReceivedApplicationMessage(MqttApplicationMessageReceivedEventArgs x)
        {
            var item = $"Timestamp: {DateTime.Now:O} | Topic: {x.ApplicationMessage.Topic} | Payload: {x.ApplicationMessage.ConvertPayloadToString()} | QoS: {x.ApplicationMessage.QualityOfServiceLevel}";
            System.Console.WriteLine(item);
        }

        private static void OnPublisherConnected(MqttClientConnectedEventArgs x)
        {
            System.Console.WriteLine("Publisher Connected");
        }

        private static void OnPublisherDisconnected(MqttClientDisconnectedEventArgs x)
        {
            System.Console.WriteLine("Publisher Disconnected");
        }

        private static void OnSubscriberConnected(MqttClientConnectedEventArgs x)
        {
            System.Console.WriteLine("Subscriber Connected");
        }

        private static void OnSubscriberDisconnected(MqttClientDisconnectedEventArgs x)
        {
            System.Console.WriteLine("Subscriber Disconnected");
        }

        private void Form1_MouseDown(Object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                IsFormBeingDragged = true;
                MouseDownPosition = MousePosition;
                OrigPosition = this.Location;
                System.Console.WriteLine("Mouse Down");
            }
        }

        private void Form1_MouseUp(Object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                IsFormBeingDragged = false;
                System.Console.WriteLine("Mouse Up");
            }
        }

        private void Form1_MouseMove(Object sender, MouseEventArgs e)
        {
            if (IsFormBeingDragged) {
                Point temp = new Point();
                temp.X = OrigPosition.X + (MousePosition.X - MouseDownPosition.X);
                temp.Y = OrigPosition.Y + (MousePosition.Y - MouseDownPosition.Y);
                this.Location = temp;
                System.Console.WriteLine("Mouse Move");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (turnCheck.Checked)
            {
                panel1.Enabled = true;
                panel2.Enabled = false;
            }
            else
            {
                panel2.Enabled = true;
                panel1.Enabled = false;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!stopWorker)
            {
                if (publishMqtt)
                {
                    OnPublish();
                }else if (!subscribeMqtt)
                {
                    string payload = team1.toJson();
                    ovTeam1.Invoke((MethodInvoker)delegate { ovTeam1.fromJson(payload); });
                    payload = team2.toJson();
                    ovTeam2.Invoke((MethodInvoker)delegate { ovTeam2.fromJson(payload); });
                }
                System.Threading.Thread.Sleep(1000);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            configForm.ShowDialog();
            if (publishMqtt != configForm.IsPublishActive())
            {
                if (configForm.IsPublishActive())
                {
                    MqttConnectPublisher(configForm.getSessionId());
                }
                else
                {
                    MqttDisconnectPublisher();
                }                
            }
            if (subscribeMqtt != configForm.IsSubscribeActive())
            {
                if (configForm.IsSubscribeActive())
                {
                    MqttStartSubscribe(configForm.getSessionId());
                }
                else
                {
                    MqttStopSubscribe();
                }
            }

        }

        bool ovActive = false;
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (!ovActive)
            {
                ovTeam1.FadeIn();
                ovTeam2.FadeIn();
                ovActive = true;
            }
            else
            {
                ovTeam1.FadeOut();
                ovTeam2.FadeOut();
                ovActive = false;
            }
        }

        private void BBStatsMain_Shown(object sender, EventArgs e)
        {
            ovTeam1.Parent = ovBg;
            ovTeam2.Parent = ovBg;
            configForm.Owner = this;
            team1.Show();
            team2.Show();
            ovTeam1.Show();
            ovTeam2.Show();
            ovBg.Show();
            ovTeam1.BringToFront();
            ovTeam2.BringToFront();
            ovBg.SendToBack();
            backgroundWorker1.RunWorkerAsync();
        }

        bool minimized = false;
        Point b1Pos;
        Point b2Pos;
        Point b3Pos;
        Rectangle origBounds;
        private void minimize_Click(object sender, EventArgs e)
        {
            if (minimized)
            {
                minimize.Location = b1Pos;
                show.Location = b2Pos;
                settings.Location = b3Pos;
                this.Width = origBounds.Width;
                this.Height = origBounds.Height;
                groupBox1.Visible = true;
                minimized = false;
            }
            else
            {
                minimize.Location = new Point(10, 10);
                show.Location = new Point(minimize.Location.X, minimize.Location.Y + minimize.Height);
                settings.Location = new Point(show.Location.X, show.Location.Y + show.Height);
                this.Width = minimize.Width + 20;
                this.Height = minimize.Height * 3 + 20;
                groupBox1.Visible = false;
                minimized = true;
            }
        }

        void ProcessAction(int team, RulesEventFullStateBoardStateTeamStatePlayerState player, RulesEventBoardAction action)
        {
            Types.ActionTypes action_type = (Types.ActionTypes)Int32.Parse(action.ActionType);
            switch (action_type)
            {
                case Types.ActionTypes.Block:
                {

                }break;
                case Types.ActionTypes.Catch:
                {

                }break;
                case Types.ActionTypes.Pass:
                {

                }
                break;
                default:
                {
                    break;
                }
            }
        }

        void SnifferListener.OnRulesEventBoardAction(RulesEventBoardAction rulesEventBoardAction)
        {
            Console.WriteLine("OnRulesEventBoardAction");
            if (players.Count > 0)
            {
                int player_id = Int32.Parse(rulesEventBoardAction.PlayerId);
                if (players[0].ContainsKey(player_id))
                {
                    Console.WriteLine("Team 1::Player " + player_id + " action received");
                    ProcessAction(0, players[0][player_id], rulesEventBoardAction);
                    this.Invoke((MethodInvoker)delegate { turnCheck.Checked = true; });
                }
                else if (players[1].ContainsKey(player_id))
                {
                    Console.WriteLine("Team 2::Player " + player_id + " action received");
                    ProcessAction(1, players[1][player_id], rulesEventBoardAction);
                    this.Invoke((MethodInvoker)delegate { turnCheck.Checked = false; });
                }
                else
                {
                    Console.WriteLine("ERROR::Unknown player id " + player_id);
                }
            }
        }

        void SnifferListener.OnRulesEventCoachChoice(RulesEventCoachChoice rulesEventCoachChoice)
        {
            Console.WriteLine("OnRulesEventCoachChoice");
        }

        void SnifferListener.OnRulesEventEndTurn(RulesEventEndTurn rulesEventEndTurn)
        {
            Console.WriteLine("OnRulesEventEndTurn");
            this.Invoke((MethodInvoker)delegate { turnCheck.Checked = (rulesEventEndTurn.PlayingTeam == "0"); });
        }

        void SnifferListener.OnRulesEventForcedDices(RulesEventForcedDices rulesEventForcedDices)
        {
            Console.WriteLine("OnRulesEventForcedDices");
        }

        Dictionary<int, Dictionary<int, RulesEventFullStateBoardStateTeamStatePlayerState>> players = new Dictionary<int, Dictionary<int, RulesEventFullStateBoardStateTeamStatePlayerState>>();
        void SnifferListener.OnRulesEventFullState(RulesEventFullState rulesEventFullState)
        {
            Console.WriteLine("OnRulesEventFullState");
            RulesEventFullStateBoardState state = (RulesEventFullStateBoardState)rulesEventFullState.Items[2];
            for (int i = 0; i <= 1; ++i)
            {
                if (!players.ContainsKey(i))
                {
                    players[i] = new Dictionary<int, RulesEventFullStateBoardStateTeamStatePlayerState>();
                }
                foreach (var player in state.ListTeams[i].ListPitchPlayers)
                {
                    int id = Int32.Parse(player.Id);
                    if (!players[i].ContainsKey(id))
                    {
                        players[i].Add(id, player);
                    }
                    else
                    {
                        players[i][id] = player;
                    }
                }
            }
        }

        void SnifferListener.OnRulesEventWaitingRequest(RulesEventWaitingRequest rulesEventWaitingRequest)
        {
            Console.WriteLine("OnRulesEventWaitingRequest");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int ok_frames;
            int total_frames;
            sniffer.GetStats(out ok_frames, out total_frames);
            nFrames.Text = "Frames: " + ok_frames;
            snifferStatus.Text = "Sniffer: " + (sniffer.IsRunning()? "YES" : "NO");
        }
    }
}
