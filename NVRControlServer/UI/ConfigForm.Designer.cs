namespace NVRControlServer.UI
{
    partial class ConfigForm
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
            this.Port_groupBox = new System.Windows.Forms.GroupBox();
            this.ConfigForm_NetConfigPort_textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ConfigForm_NetConfigIp_textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ConfigForm_SaveNetConfig_button = new System.Windows.Forms.Button();
            this.Port_groupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Port_groupBox
            // 
            this.Port_groupBox.Controls.Add(this.ConfigForm_NetConfigPort_textBox);
            this.Port_groupBox.Controls.Add(this.label1);
            this.Port_groupBox.Location = new System.Drawing.Point(12, 104);
            this.Port_groupBox.Name = "Port_groupBox";
            this.Port_groupBox.Size = new System.Drawing.Size(447, 63);
            this.Port_groupBox.TabIndex = 0;
            this.Port_groupBox.TabStop = false;
            this.Port_groupBox.Text = "端口";
            // 
            // ConfigForm_NetConfigPort_textBox
            // 
            this.ConfigForm_NetConfigPort_textBox.Location = new System.Drawing.Point(236, 24);
            this.ConfigForm_NetConfigPort_textBox.Name = "ConfigForm_NetConfigPort_textBox";
            this.ConfigForm_NetConfigPort_textBox.Size = new System.Drawing.Size(100, 21);
            this.ConfigForm_NetConfigPort_textBox.TabIndex = 1;
            this.ConfigForm_NetConfigPort_textBox.Text = "8889";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(73, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "侦听端口";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ConfigForm_NetConfigIp_textBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 23);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(447, 63);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "IP地址";
            // 
            // ConfigForm_NetConfigIp_textBox
            // 
            this.ConfigForm_NetConfigIp_textBox.Location = new System.Drawing.Point(236, 24);
            this.ConfigForm_NetConfigIp_textBox.Name = "ConfigForm_NetConfigIp_textBox";
            this.ConfigForm_NetConfigIp_textBox.Size = new System.Drawing.Size(100, 21);
            this.ConfigForm_NetConfigIp_textBox.TabIndex = 1;
            this.ConfigForm_NetConfigIp_textBox.Text = "127.0.0.1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(73, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "监听IP地址";
            // 
            // ConfigForm_SaveNetConfig_button
            // 
            this.ConfigForm_SaveNetConfig_button.Location = new System.Drawing.Point(384, 191);
            this.ConfigForm_SaveNetConfig_button.Name = "ConfigForm_SaveNetConfig_button";
            this.ConfigForm_SaveNetConfig_button.Size = new System.Drawing.Size(75, 23);
            this.ConfigForm_SaveNetConfig_button.TabIndex = 3;
            this.ConfigForm_SaveNetConfig_button.Text = "保存";
            this.ConfigForm_SaveNetConfig_button.UseVisualStyleBackColor = true;
            this.ConfigForm_SaveNetConfig_button.Click += new System.EventHandler(this.ConfigForm_SaveNetConfig_button_Click);
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 269);
            this.Controls.Add(this.ConfigForm_SaveNetConfig_button);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Port_groupBox);
            this.Name = "ConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "配置";
            this.Port_groupBox.ResumeLayout(false);
            this.Port_groupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox Port_groupBox;
        private System.Windows.Forms.TextBox ConfigForm_NetConfigPort_textBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox ConfigForm_NetConfigIp_textBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ConfigForm_SaveNetConfig_button;
    }
}