
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
            this.TokenListBox = new System.Windows.Forms.ListBox();
            this.LogLabel = new System.Windows.Forms.Label();
            this.CardNumberInputBox = new System.Windows.Forms.TextBox();
            this.AcceptCardNumber = new System.Windows.Forms.Button();
            this.RemoveTokenButton = new System.Windows.Forms.Button();
            this.UpdateListButton = new System.Windows.Forms.Button();
            this.MqttLog = new System.Windows.Forms.ListBox();
            this.ExportToCsvButton = new System.Windows.Forms.Button();
            this.SortButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TokenListBox
            // 
            this.TokenListBox.FormattingEnabled = true;
            this.TokenListBox.ItemHeight = 20;
            this.TokenListBox.Location = new System.Drawing.Point(12, 12);
            this.TokenListBox.Name = "TokenListBox";
            this.TokenListBox.Size = new System.Drawing.Size(200, 164);
            this.TokenListBox.TabIndex = 1;
            // 
            // LogLabel
            // 
            this.LogLabel.AutoSize = true;
            this.LogLabel.Location = new System.Drawing.Point(12, 179);
            this.LogLabel.Name = "LogLabel";
            this.LogLabel.Size = new System.Drawing.Size(126, 20);
            this.LogLabel.TabIndex = 2;
            this.LogLabel.Text = "Log from devices:";
            // 
            // CardNumberInputBox
            // 
            this.CardNumberInputBox.Location = new System.Drawing.Point(218, 114);
            this.CardNumberInputBox.Name = "CardNumberInputBox";
            this.CardNumberInputBox.Size = new System.Drawing.Size(235, 27);
            this.CardNumberInputBox.TabIndex = 10;
            // 
            // AcceptCardNumber
            // 
            this.AcceptCardNumber.Location = new System.Drawing.Point(218, 147);
            this.AcceptCardNumber.Name = "AcceptCardNumber";
            this.AcceptCardNumber.Size = new System.Drawing.Size(94, 29);
            this.AcceptCardNumber.TabIndex = 4;
            this.AcceptCardNumber.Text = "Add Token";
            this.AcceptCardNumber.UseVisualStyleBackColor = true;
            this.AcceptCardNumber.Click += new System.EventHandler(this.AcceptCardNumber_Click);
            // 
            // RemoveTokenButton
            // 
            this.RemoveTokenButton.Location = new System.Drawing.Point(218, 12);
            this.RemoveTokenButton.Name = "RemoveTokenButton";
            this.RemoveTokenButton.Size = new System.Drawing.Size(235, 29);
            this.RemoveTokenButton.TabIndex = 5;
            this.RemoveTokenButton.Text = "Remove Selected Token";
            this.RemoveTokenButton.UseVisualStyleBackColor = true;
            this.RemoveTokenButton.Click += new System.EventHandler(this.RemoveTokenButton_Click);
            // 
            // UpdateListButton
            // 
            this.UpdateListButton.Location = new System.Drawing.Point(218, 47);
            this.UpdateListButton.Name = "UpdateListButton";
            this.UpdateListButton.Size = new System.Drawing.Size(235, 29);
            this.UpdateListButton.TabIndex = 6;
            this.UpdateListButton.Text = "Update List";
            this.UpdateListButton.UseVisualStyleBackColor = true;
            this.UpdateListButton.Click += new System.EventHandler(this.UpdateListButton_Click);
            // 
            // MqttLog
            // 
            this.MqttLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MqttLog.FormattingEnabled = true;
            this.MqttLog.ItemHeight = 20;
            this.MqttLog.Location = new System.Drawing.Point(12, 202);
            this.MqttLog.Name = "MqttLog";
            this.MqttLog.Size = new System.Drawing.Size(455, 184);
            this.MqttLog.TabIndex = 7;
            // 
            // ExportToCsvButton
            // 
            this.ExportToCsvButton.Location = new System.Drawing.Point(318, 147);
            this.ExportToCsvButton.Name = "ExportToCsvButton";
            this.ExportToCsvButton.Size = new System.Drawing.Size(135, 29);
            this.ExportToCsvButton.TabIndex = 8;
            this.ExportToCsvButton.Text = "Export to CSV";
            this.ExportToCsvButton.UseVisualStyleBackColor = true;
            this.ExportToCsvButton.Click += new System.EventHandler(this.ExportToCsvButton_Click);
            // 
            // SortButton
            // 
            this.SortButton.Location = new System.Drawing.Point(218, 79);
            this.SortButton.Name = "SortButton";
            this.SortButton.Size = new System.Drawing.Size(94, 29);
            this.SortButton.TabIndex = 9;
            this.SortButton.Text = "Sort";
            this.SortButton.UseVisualStyleBackColor = true;
            this.SortButton.Click += new System.EventHandler(this.SortButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 403);
            this.Controls.Add(this.SortButton);
            this.Controls.Add(this.ExportToCsvButton);
            this.Controls.Add(this.MqttLog);
            this.Controls.Add(this.UpdateListButton);
            this.Controls.Add(this.RemoveTokenButton);
            this.Controls.Add(this.AcceptCardNumber);
            this.Controls.Add(this.CardNumberInputBox);
            this.Controls.Add(this.LogLabel);
            this.Controls.Add(this.TokenListBox);
            this.MinimumSize = new System.Drawing.Size(500, 450);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox TokenListBox;
        private System.Windows.Forms.Label LogLabel;
        private System.Windows.Forms.TextBox CardNumberInputBox;
        private System.Windows.Forms.Button AcceptCardNumber;
        private System.Windows.Forms.Button RemoveTokenButton;
        private System.Windows.Forms.Button UpdateListButton;
        private System.Windows.Forms.ListBox MqttLog;
        private System.Windows.Forms.Button ExportToCsvButton;
        private System.Windows.Forms.Button SortButton;
    }
}

