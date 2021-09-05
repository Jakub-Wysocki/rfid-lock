using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MQTTnet;
using Serilog;
using Serilog.Sinks.WinForms;

namespace device_manager
{
    public partial class Form1 : Form
    {
        private readonly MqttBroker broker;

        public Form1()
        {
            InitializeComponent();
            broker = new();
        }

   /*     private void HandleReceivedApplicationMessage(MqttApplicationMessageReceivedEventArgs x)
        {
          
                MessageBox.Show(x.ApplicationMessage.Topic.ToString());
            
        }*/


        private void AcceptCardNumber_Click(object sender, EventArgs e)
        {
            TokenListBox.Items.Add(CardNumberInputBox.Text.ToString());

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("/rfid/add")
                .WithPayload(CardNumberInputBox.Text.ToString())
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            broker.PublishMessege(message);
         
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

                broker.PublishMessege(message);

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

            broker.PublishMessege(message);
        }
    }
}
