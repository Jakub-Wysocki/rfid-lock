
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
            this.MqttLog = new System.Windows.Forms.TextBox();
            this.TokenListBox = new System.Windows.Forms.ListBox();
            this.LogLabel = new System.Windows.Forms.Label();
            this.CardNumberInputBox = new System.Windows.Forms.TextBox();
            this.AcceptCardNumber = new System.Windows.Forms.Button();
            this.RemoveTokenButton = new System.Windows.Forms.Button();
            this.UpdateListButton = new System.Windows.Forms.Button();
            this.SaveLog = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // MqttLog
            // 
            this.MqttLog.Location = new System.Drawing.Point(12, 214);
            this.MqttLog.Multiline = true;
            this.MqttLog.Name = "MqttLog";
            this.MqttLog.Size = new System.Drawing.Size(776, 224);
            this.MqttLog.TabIndex = 0;
            // 
            // TokenListBox
            // 
            this.TokenListBox.FormattingEnabled = true;
            this.TokenListBox.ItemHeight = 20;
            this.TokenListBox.Location = new System.Drawing.Point(12, 12);
            this.TokenListBox.Name = "TokenListBox";
            this.TokenListBox.Size = new System.Drawing.Size(169, 164);
            this.TokenListBox.TabIndex = 1;
            // 
            // LogLabel
            // 
            this.LogLabel.AutoSize = true;
            this.LogLabel.Location = new System.Drawing.Point(12, 191);
            this.LogLabel.Name = "LogLabel";
            this.LogLabel.Size = new System.Drawing.Size(126, 20);
            this.LogLabel.TabIndex = 2;
            this.LogLabel.Text = "Log from devices:";
            // 
            // CardNumberInputBox
            // 
            this.CardNumberInputBox.Location = new System.Drawing.Point(187, 119);
            this.CardNumberInputBox.Name = "CardNumberInputBox";
            this.CardNumberInputBox.Size = new System.Drawing.Size(165, 27);
            this.CardNumberInputBox.TabIndex = 3;
            // 
            // AcceptCardNumber
            // 
            this.AcceptCardNumber.Location = new System.Drawing.Point(187, 152);
            this.AcceptCardNumber.Name = "AcceptCardNumber";
            this.AcceptCardNumber.Size = new System.Drawing.Size(94, 29);
            this.AcceptCardNumber.TabIndex = 4;
            this.AcceptCardNumber.Text = "Add Token";
            this.AcceptCardNumber.UseVisualStyleBackColor = true;
            this.AcceptCardNumber.Click += new System.EventHandler(this.AcceptCardNumber_Click);
            // 
            // RemoveTokenButton
            // 
            this.RemoveTokenButton.Location = new System.Drawing.Point(187, 12);
            this.RemoveTokenButton.Name = "RemoveTokenButton";
            this.RemoveTokenButton.Size = new System.Drawing.Size(194, 29);
            this.RemoveTokenButton.TabIndex = 5;
            this.RemoveTokenButton.Text = "Remove Selected Token";
            this.RemoveTokenButton.UseVisualStyleBackColor = true;
            this.RemoveTokenButton.Click += new System.EventHandler(this.RemoveTokenButton_Click);
            // 
            // UpdateListButton
            // 
            this.UpdateListButton.Location = new System.Drawing.Point(187, 67);
            this.UpdateListButton.Name = "UpdateListButton";
            this.UpdateListButton.Size = new System.Drawing.Size(194, 29);
            this.UpdateListButton.TabIndex = 6;
            this.UpdateListButton.Text = "Update List";
            this.UpdateListButton.UseVisualStyleBackColor = true;
            this.UpdateListButton.Click += new System.EventHandler(this.UpdateListButton_Click);
            // 
            // SaveLog
            // 
            this.SaveLog.Location = new System.Drawing.Point(694, 179);
            this.SaveLog.Name = "SaveLog";
            this.SaveLog.Size = new System.Drawing.Size(94, 29);
            this.SaveLog.TabIndex = 7;
            this.SaveLog.Text = "button1";
            this.SaveLog.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.SaveLog);
            this.Controls.Add(this.UpdateListButton);
            this.Controls.Add(this.RemoveTokenButton);
            this.Controls.Add(this.AcceptCardNumber);
            this.Controls.Add(this.CardNumberInputBox);
            this.Controls.Add(this.LogLabel);
            this.Controls.Add(this.TokenListBox);
            this.Controls.Add(this.MqttLog);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox MqttLog;
        private System.Windows.Forms.ListBox TokenListBox;
        private System.Windows.Forms.Label LogLabel;
        private System.Windows.Forms.TextBox CardNumberInputBox;
        private System.Windows.Forms.Button AcceptCardNumber;
        private System.Windows.Forms.Button RemoveTokenButton;
        private System.Windows.Forms.Button UpdateListButton;
        private System.Windows.Forms.Button SaveLog;
    }
}

