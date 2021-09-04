using System;
using System.Windows.Forms;
using MQTTnet;

namespace device_manager
{
    public partial class Form1 : Form
    {
        private MqttBroker broker;

        public Form1()
        {
            InitializeComponent();
            broker = new();
        }

        private void AcceptCardNumber_Click(object sender, EventArgs e)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("/topic/rfid_data")
                .WithPayload("Hello World")
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            broker.PublishMessege(message);
        }
    }
}
