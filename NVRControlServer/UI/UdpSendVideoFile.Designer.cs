namespace NVRControlServer.UI
{
    partial class UdpSendVideoFile
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SendFile_button = new System.Windows.Forms.Button();
            this.tbRemotePort = new System.Windows.Forms.TextBox();
            this.tbLocalPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.StartUdpPortListen_button = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tbRemoteIP = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(8, 8);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(237, 196);
            this.listBox1.TabIndex = 4;

            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 213);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 15;
            this.label1.Text = "本机监听端口";
            // 
            // SendFile_button
            // 
            this.SendFile_button.Location = new System.Drawing.Point(98, 273);
            this.SendFile_button.Name = "SendFile_button";
            this.SendFile_button.Size = new System.Drawing.Size(75, 23);
            this.SendFile_button.TabIndex = 14;
            this.SendFile_button.Text = "发送文件";
            this.SendFile_button.UseVisualStyleBackColor = true;
            // 
            // tbRemotePort
            // 
            this.tbRemotePort.Location = new System.Drawing.Point(300, 241);
            this.tbRemotePort.Name = "tbRemotePort";
            this.tbRemotePort.Size = new System.Drawing.Size(100, 21);
            this.tbRemotePort.TabIndex = 21;
            this.tbRemotePort.Text = "10003";
            // 
            // tbLocalPort
            // 
            this.tbLocalPort.Location = new System.Drawing.Point(98, 210);
            this.tbLocalPort.Name = "tbLocalPort";
            this.tbLocalPort.Size = new System.Drawing.Size(100, 21);
            this.tbLocalPort.TabIndex = 16;
            this.tbLocalPort.Text = "10002";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(205, 247);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 20;
            this.label3.Text = "接收方监听端口";
            // 
            // StartUdpPortListen_button
            // 
            this.StartUdpPortListen_button.Location = new System.Drawing.Point(15, 273);
            this.StartUdpPortListen_button.Name = "StartUdpPortListen_button";
            this.StartUdpPortListen_button.Size = new System.Drawing.Size(75, 23);
            this.StartUdpPortListen_button.TabIndex = 17;
            this.StartUdpPortListen_button.Text = "开始监听";
            this.StartUdpPortListen_button.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 247);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 19;
            this.label2.Text = "接收方IP";
            // 
            // tbRemoteIP
            // 
            this.tbRemoteIP.Location = new System.Drawing.Point(99, 241);
            this.tbRemoteIP.Name = "tbRemoteIP";
            this.tbRemoteIP.Size = new System.Drawing.Size(100, 21);
            this.tbRemoteIP.TabIndex = 18;
            this.tbRemoteIP.Text = "127.0.0.1";
            // 
            // UdpSendVideoFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(529, 318);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SendFile_button);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.tbRemotePort);
            this.Controls.Add(this.tbLocalPort);
            this.Controls.Add(this.tbRemoteIP);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.StartUdpPortListen_button);
            this.Name = "UdpSendVideoFile";
            this.Text = "UdpSendVideoFile";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button SendFile_button;
        private System.Windows.Forms.TextBox tbRemotePort;
        private System.Windows.Forms.TextBox tbLocalPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button StartUdpPortListen_button;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbRemoteIP;
    }
}