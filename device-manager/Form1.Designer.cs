
namespace device_manager
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LogLabel = new System.Windows.Forms.Label();
            this.CardNumberInputBox = new System.Windows.Forms.TextBox();
            this.AcceptCardNumber = new System.Windows.Forms.Button();
            this.MqttLog = new System.Windows.Forms.ListBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LogLabel
            // 
            this.LogLabel.AutoSize = true;
            this.LogLabel.Location = new System.Drawing.Point(10, 134);
            this.LogLabel.Name = "LogLabel";
            this.LogLabel.Size = new System.Drawing.Size(101, 15);
            this.LogLabel.TabIndex = 2;
            this.LogLabel.Text = "Log from devices:";
            // 
            // CardNumberInputBox
            // 
            this.CardNumberInputBox.Location = new System.Drawing.Point(64, 69);
            this.CardNumberInputBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CardNumberInputBox.Name = "CardNumberInputBox";
            this.CardNumberInputBox.Size = new System.Drawing.Size(206, 23);
            this.CardNumberInputBox.TabIndex = 10;
            // 
            // AcceptCardNumber
            // 
            this.AcceptCardNumber.Location = new System.Drawing.Point(64, 96);
            this.AcceptCardNumber.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.AcceptCardNumber.Name = "AcceptCardNumber";
            this.AcceptCardNumber.Size = new System.Drawing.Size(116, 22);
            this.AcceptCardNumber.TabIndex = 4;
            this.AcceptCardNumber.Text = "Send Message";
            this.AcceptCardNumber.UseVisualStyleBackColor = true;
            this.AcceptCardNumber.Click += new System.EventHandler(this.AcceptCardNumber_Click);
            // 
            // MqttLog
            // 
            this.MqttLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MqttLog.FormattingEnabled = true;
            this.MqttLog.ItemHeight = 15;
            this.MqttLog.Location = new System.Drawing.Point(10, 152);
            this.MqttLog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MqttLog.Name = "MqttLog";
            this.MqttLog.Size = new System.Drawing.Size(314, 139);
            this.MqttLog.TabIndex = 7;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Message",
            "Time",
            "Font Size"});
            this.comboBox1.Location = new System.Drawing.Point(64, 31);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(206, 23);
            this.comboBox1.TabIndex = 11;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.ComboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 15);
            this.label1.TabIndex = 12;
            this.label1.Text = "Topic";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 15);
            this.label2.TabIndex = 13;
            this.label2.Text = "Message";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 308);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.MqttLog);
            this.Controls.Add(this.AcceptCardNumber);
            this.Controls.Add(this.CardNumberInputBox);
            this.Controls.Add(this.LogLabel);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MinimumSize = new System.Drawing.Size(440, 347);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label LogLabel;
        private System.Windows.Forms.TextBox CardNumberInputBox;
        private System.Windows.Forms.Button AcceptCardNumber;
        private System.Windows.Forms.ListBox MqttLog;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

