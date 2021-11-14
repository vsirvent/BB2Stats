using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Formatter;
using MQTTnet.Protocol;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BB2Stats
{
    public partial class BBStatsMain : Form
    {
        BB2StatsForm team1 = null;
        BB2StatsForm team2 = null;
        ConfigForm configForm = null;
        IManagedMqttClient managedMqttClientPublisher;
        IManagedMqttClient managedMqttClientSubscriber;
        string mqttId = "";
        bool publishMqtt = false;
        bool subscribeMqtt = false;
        string mqttIdSubs = "";
        
        public BBStatsMain()
        {
            InitializeComponent();
            configForm = new ConfigForm();
            team1 = new BB2StatsForm();
            team2 = new BB2StatsForm();
            team1.TopLevel = false;
            team2.TopLevel = false;
            team1.MouseDown += Form1_MouseDown;
            team1.MouseUp += Form1_MouseUp;
            team1.MouseMove += Form1_MouseMove;
            team2.MouseDown += Form1_MouseDown;
            team2.MouseUp += Form1_MouseUp;
            team2.MouseMove += Form1_MouseMove;
            pictureBox1.BringToFront();
            checkBox1.Checked = true;
            this.panel1.Controls.Add(team1);
            this.panel2.Controls.Add(team2);
            team1.Show();
            team2.Show();            
        }

        private bool IsFormBeingDragged = false;
        private Point OrigPosition;
        private Point MouseDownPosition;
        private bool stopWorker;

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
            stopWorker = false; 
            backgroundWorker1.RunWorkerAsync();
        }

        private async void MqttDisconnectPublisher()
        {
            stopWorker = true;
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
            }else if (x.ApplicationMessage.Topic == topic2)
            {
                team1.Invoke((MethodInvoker)delegate { team2.fromJson(x.ApplicationMessage.ConvertPayloadToString()); }); 
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
            if (checkBox1.Checked)
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
                OnPublish();
                System.Threading.Thread.Sleep(5000);
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
    }
}
