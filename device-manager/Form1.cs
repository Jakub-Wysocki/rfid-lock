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
    
        public Form1()
        {
            InitializeComponent();
            BrokerInit();

        }


        private void AcceptCardNumber_Click(object sender, EventArgs e)
        {
            string cardNumber = CardNumberInputBox.Text.ToString();

            


            if (!(cardNumber.All(char.IsDigit) &&  !String.IsNullOrEmpty(cardNumber)))
            {
                MessageBox.Show("Only numbers are allowed");
                return;
            }
            else if(TokenListBox.Items.Contains(cardNumber))
            {
                MessageBox.Show("Number already on the list");
                return;
            }

            TokenListBox.Items.Add(cardNumber);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("/rfid/add")
                .WithPayload(cardNumber)
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            PublishMessege(message);

            CardNumberInputBox.Clear();
        }

        private void RemoveTokenButton_Click(object sender, EventArgs e)
        {
            if(TokenListBox.SelectedItem != null)
            {
                var message = new MqttApplicationMessageBuilder()
                .WithTopic("/rfid/remove")
                .WithPayload(TokenListBox.SelectedItem.ToString())
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

                PublishMessege(message);

                TokenListBox.Items.Remove(TokenListBox.SelectedItem);

            }
                  
        }

        private void UpdateListButton_Click(object sender, EventArgs e)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("/rfid/list")
                .WithPayload("update")
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            ClearTokenBox();

            PublishMessege(message);
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

        void AppendTokenBox(string messege)
        {

            if (TokenListBox.InvokeRequired)
                TokenListBox.Invoke(new Action<string>(AppendTokenBox), messege);
            else
                TokenListBox.Items.Add(messege);

        }

        void ClearTokenBox()
        {

            if (TokenListBox.InvokeRequired)
                TokenListBox.Invoke(new Action(ClearTokenBox));
            else
                TokenListBox.Items.Clear();

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

                case "/topic/remove":

                    if (message == "REMOVED")
                    {
                        AppendLog(log);
                    }
                    else if (message == "NO DEVICES")
                    {
                        AppendLog(log);
                        ClearTokenBox();
                    }

                   break;
                case "/topic/add":

                    if (message.StartsWith("ADDED DEVICE"))
                    {
                        AppendLog(log);
                    }
                    else
                        AppendLog(log);

                    break;
                case "/topic/list":

                    if (message == "NO DEVICES")
                    {
                        AppendLog(log);
                        ClearTokenBox();
                    }
                    else
                    {
                        string parsedMessage = System.Text.RegularExpressions.Regex.Replace(message, "[^0-9]", "").Trim();

                        AppendTokenBox(parsedMessage);
                        AppendLog("Sync device from ESP: " + parsedMessage);
                        


                    }
                    break;

            }

            //var payload = context.ApplicationMessage?.Payload == null ? null : Encoding.UTF8.GetString(context.ApplicationMessage?.Payload);


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
                if(!saveFileDialog1.CheckFileExists)
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

        private void SortButton_Click(object sender, EventArgs e)
        {
            
            TokenListBox.Sorted = !TokenListBox.Sorted;
        }


    }
}
 