using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MQTTnet;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet.Server;
using System.Text;
using System.Linq;
using System.IO;
using CsvHelper;
using System.Globalization;

namespace device_manager
{
    public partial class Form1 : Form
    {
        private IMqttServer mqttServer;
        private string selectedTopic;

        public Form1()
        {
            InitializeComponent();
            BrokerInit();

        }

        private void AcceptCardNumber_Click(object sender, EventArgs e)
        {
            string user_message = CardNumberInputBox.Text.ToString();

            if(string.IsNullOrEmpty(selectedTopic))
            {
                MessageBox.Show("Wrong topic ");
                return;
            } else if(string.IsNullOrEmpty(user_message))
            {
                MessageBox.Show("Wrong message");
                return;
            } else if(selectedTopic != "/eink/message" && !int.TryParse(user_message, out _))
            {

                MessageBox.Show("Should be number");
                return ;
            }
            //add font sizes verification


            var message = new MqttApplicationMessageBuilder()
                .WithTopic(selectedTopic)
                .WithPayload(user_message)
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            PublishMessege(message);

            CardNumberInputBox.Clear();
        }

        void AppendLog(string messege)
        {
            var time = DateTime.Now.ToString("T");

            if (MqttLog.InvokeRequired)
            {
                MqttLog.Invoke(new Action<string>(AppendLog), time + " " + messege);
            }

            else
            {
                MqttLog.Items.Add(time + " " + messege);
            }

        }

        private async void BrokerInit()
        {
            // Configure MQTT server.
            var optionsBuilder = new MqttServerOptionsBuilder()
                .WithConnectionBacklog(100)
                .WithDefaultEndpointPort(1883)
                .WithConnectionValidator(OnNewConnection)
                .WithApplicationMessageInterceptor(OnNewMessage);

            mqttServer = new MqttFactory().CreateMqttServer();
            await mqttServer.StartAsync(optionsBuilder.Build());

            //mqttServer.UseApplicationMessageReceivedHandler(this.HandleReceivedApplicationMessage);
        }

        private void OnNewConnection(MqttConnectionValidatorContext context)
        {
            AppendLog($"Client connected: {context.ClientId} ");

            //AppendText($"Timestamp: {DateTime.Now:O} | Topic: {x.ApplicationMessage.Topic} | Payload: {x.ApplicationMessage.ConvertPayloadToString()} | QoS: {x.ApplicationMessage.QualityOfServiceLevel}");

            //AppendText( );


        }
        private void OnNewMessage(MqttApplicationMessageInterceptorContext context)
        {
            string message = context.ApplicationMessage.ConvertPayloadToString();
            string topic = context.ApplicationMessage.Topic;
            string log = "Topic:  " + topic + "Message: " + message;

            switch (topic)
            {
                case "/topic/log":
                    AppendLog(log);
                    break;

            }


        }

        private async void PublishMessege(MqttApplicationMessage message)
        {

            await mqttServer.PublishAsync(message, CancellationToken.None);

        }

        private void ExportToCsvButton_Click(object sender, EventArgs e)
        {

            SaveFileDialog saveFileDialog1 = new();

            saveFileDialog1.Filter = "CSV file (*.csv)|*.csv";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (!saveFileDialog1.CheckFileExists)
                {
                    StreamWriter myOutputStream = new(Path.GetFullPath(saveFileDialog1.FileName));

                    foreach (var item in MqttLog.Items)
                    {
                        myOutputStream.WriteLine(item.ToString());
                    }

                    myOutputStream.Close();
                }



            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           string topic;

           switch(comboBox1.SelectedItem.ToString())
            {
                case "Message":
                    topic = "message";
                    break;
                case "Time":
                    topic = "time";
                    break;
                case "Font Size":
                    topic = "font-size";
                    break;
                default:
                    MessageBox.Show("Wrong topic!");
                    return;

            }
 
            selectedTopic = "/eink/" + topic;

        }
    }
}
