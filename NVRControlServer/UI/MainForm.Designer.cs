namespace NVRControlServer.UI
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.listBoxStatus = new System.Windows.Forms.ListBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.Connect_groupBox = new System.Windows.Forms.GroupBox();
            this.NVRonlin_Label = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Time_groupBox = new System.Windows.Forms.GroupBox();
            this.TimeShow_panel = new System.Windows.Forms.Panel();
            this.labelX_Time = new DevComponents.DotNetBar.LabelX();
            this.labelX_Date = new DevComponents.DotNetBar.LabelX();
            this.Contorl_groupBox = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.UdpSendFile_button = new System.Windows.Forms.Button();
            this.RecordConfig_button = new System.Windows.Forms.Button();
            this.Quit_button = new System.Windows.Forms.Button();
            this.About_button = new System.Windows.Forms.Button();
            this.Config_button = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ServerStatus_listView = new System.Windows.Forms.ListView();
            this.control = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ClientIp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ClientPort = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Time = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.NVRStatus_listView = new System.Windows.Forms.ListView();
            this.NVRName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NVRIp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NVRPort = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.OnLineTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Channels = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Status_groupBox = new System.Windows.Forms.GroupBox();
            this.StopAutoVideo_Timer = new System.Windows.Forms.Timer(this.components);
            this.CheckNvrStatus_Timer = new System.Windows.Forms.Timer(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.Connect_groupBox.SuspendLayout();
            this.Time_groupBox.SuspendLayout();
            this.TimeShow_panel.SuspendLayout();
            this.Contorl_groupBox.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.Status_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBoxStatus
            // 
            this.listBoxStatus.FormattingEnabled = true;
            this.listBoxStatus.ItemHeight = 12;
            this.listBoxStatus.Location = new System.Drawing.Point(0, 0);
            this.listBoxStatus.Name = "listBoxStatus";
            this.listBoxStatus.Size = new System.Drawing.Size(538, 292);
            this.listBoxStatus.TabIndex = 0;
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(9, 20);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(86, 38);
            this.buttonStart.TabIndex = 1;
            this.buttonStart.Text = "开始监听";
            this.buttonStart.UseVisualStyleBackColor = true;
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(8, 64);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(86, 34);
            this.buttonStop.TabIndex = 2;
            this.buttonStop.Text = "停止监听";
            this.buttonStop.UseVisualStyleBackColor = true;
            // 
            // Connect_groupBox
            // 
            this.Connect_groupBox.Controls.Add(this.NVRonlin_Label);
            this.Connect_groupBox.Controls.Add(this.label2);
            this.Connect_groupBox.Controls.Add(this.label1);
            this.Connect_groupBox.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Connect_groupBox.Location = new System.Drawing.Point(12, 7);
            this.Connect_groupBox.Name = "Connect_groupBox";
            this.Connect_groupBox.Size = new System.Drawing.Size(529, 106);
            this.Connect_groupBox.TabIndex = 4;
            this.Connect_groupBox.TabStop = false;
            this.Connect_groupBox.Text = "连接信息";
            // 
            // NVRonlin_Label
            // 
            this.NVRonlin_Label.AutoSize = true;
            this.NVRonlin_Label.Location = new System.Drawing.Point(342, 31);
            this.NVRonlin_Label.Name = "NVRonlin_Label";
            this.NVRonlin_Label.Size = new System.Drawing.Size(76, 21);
            this.NVRonlin_Label.TabIndex = 2;
            this.NVRonlin_Label.Text = "NVR在线";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(59, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 21);
            this.label2.TabIndex = 1;
            this.label2.Text = "连接客户数";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "连接通道数";
            // 
            // Time_groupBox
            // 
            this.Time_groupBox.Controls.Add(this.TimeShow_panel);
            this.Time_groupBox.Location = new System.Drawing.Point(547, 9);
            this.Time_groupBox.Name = "Time_groupBox";
            this.Time_groupBox.Size = new System.Drawing.Size(215, 104);
            this.Time_groupBox.TabIndex = 5;
            this.Time_groupBox.TabStop = false;
            // 
            // TimeShow_panel
            // 
            this.TimeShow_panel.Controls.Add(this.labelX_Time);
            this.TimeShow_panel.Controls.Add(this.labelX_Date);
            this.TimeShow_panel.Location = new System.Drawing.Point(12, 15);
            this.TimeShow_panel.Name = "TimeShow_panel";
            this.TimeShow_panel.Size = new System.Drawing.Size(155, 83);
            this.TimeShow_panel.TabIndex = 0;
            // 
            // labelX_Time
            // 
            // 
            // 
            // 
            this.labelX_Time.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX_Time.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX_Time.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelX_Time.Location = new System.Drawing.Point(16, 45);
            this.labelX_Time.Name = "labelX_Time";
            this.labelX_Time.Size = new System.Drawing.Size(122, 23);
            this.labelX_Time.TabIndex = 0;
            this.labelX_Time.Text = "<b>00:00:00 PM</b>";
            this.labelX_Time.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // labelX_Date
            // 
            // 
            // 
            // 
            this.labelX_Date.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX_Date.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX_Date.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelX_Date.Location = new System.Drawing.Point(23, 14);
            this.labelX_Date.Name = "labelX_Date";
            this.labelX_Date.Size = new System.Drawing.Size(114, 23);
            this.labelX_Date.TabIndex = 0;
            this.labelX_Date.Text = "<b>2014-4-25</b>";
            this.labelX_Date.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // Contorl_groupBox
            // 
            this.Contorl_groupBox.Controls.Add(this.button2);
            this.Contorl_groupBox.Controls.Add(this.button1);
            this.Contorl_groupBox.Controls.Add(this.UdpSendFile_button);
            this.Contorl_groupBox.Controls.Add(this.RecordConfig_button);
            this.Contorl_groupBox.Controls.Add(this.Quit_button);
            this.Contorl_groupBox.Controls.Add(this.About_button);
            this.Contorl_groupBox.Controls.Add(this.Config_button);
            this.Contorl_groupBox.Controls.Add(this.buttonStart);
            this.Contorl_groupBox.Controls.Add(this.buttonStop);
            this.Contorl_groupBox.Location = new System.Drawing.Point(547, 116);
            this.Contorl_groupBox.Name = "Contorl_groupBox";
            this.Contorl_groupBox.Size = new System.Drawing.Size(215, 365);
            this.Contorl_groupBox.TabIndex = 6;
            this.Contorl_groupBox.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(8, 269);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 37);
            this.button1.TabIndex = 8;
            this.button1.Text = "进程情况";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // UdpSendFile_button
            // 
            this.UdpSendFile_button.Location = new System.Drawing.Point(8, 227);
            this.UdpSendFile_button.Name = "UdpSendFile_button";
            this.UdpSendFile_button.Size = new System.Drawing.Size(86, 37);
            this.UdpSendFile_button.TabIndex = 7;
            this.UdpSendFile_button.Text = "发送视频文件";
            this.UdpSendFile_button.UseVisualStyleBackColor = true;
            // 
            // RecordConfig_button
            // 
            this.RecordConfig_button.Location = new System.Drawing.Point(9, 189);
            this.RecordConfig_button.Name = "RecordConfig_button";
            this.RecordConfig_button.Size = new System.Drawing.Size(86, 32);
            this.RecordConfig_button.TabIndex = 6;
            this.RecordConfig_button.Text = "录像配置";
            this.RecordConfig_button.UseVisualStyleBackColor = true;
            this.RecordConfig_button.Click += new System.EventHandler(this.RecordConfig_button_Click);
            // 
            // Quit_button
            // 
            this.Quit_button.Location = new System.Drawing.Point(9, 312);
            this.Quit_button.Name = "Quit_button";
            this.Quit_button.Size = new System.Drawing.Size(86, 36);
            this.Quit_button.TabIndex = 5;
            this.Quit_button.Text = "退出";
            this.Quit_button.UseVisualStyleBackColor = true;
            this.Quit_button.Click += new System.EventHandler(this.Quit_button_Click);
            // 
            // About_button
            // 
            this.About_button.Location = new System.Drawing.Point(9, 146);
            this.About_button.Name = "About_button";
            this.About_button.Size = new System.Drawing.Size(86, 37);
            this.About_button.TabIndex = 4;
            this.About_button.Text = "关于";
            this.About_button.UseVisualStyleBackColor = true;
            this.About_button.Click += new System.EventHandler(this.About_button_Click);
            // 
            // Config_button
            // 
            this.Config_button.Location = new System.Drawing.Point(8, 104);
            this.Config_button.Name = "Config_button";
            this.Config_button.Size = new System.Drawing.Size(86, 35);
            this.Config_button.TabIndex = 3;
            this.Config_button.Text = "配置";
            this.Config_button.UseVisualStyleBackColor = true;
            this.Config_button.Click += new System.EventHandler(this.Config_button_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(6, 20);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(554, 328);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ServerStatus_listView);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(546, 298);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "服务器状态";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // ServerStatus_listView
            // 
            this.ServerStatus_listView.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.ServerStatus_listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.control,
            this.ClientIp,
            this.ClientPort,
            this.Time});
            this.ServerStatus_listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ServerStatus_listView.Location = new System.Drawing.Point(3, 3);
            this.ServerStatus_listView.Name = "ServerStatus_listView";
            this.ServerStatus_listView.Size = new System.Drawing.Size(540, 292);
            this.ServerStatus_listView.TabIndex = 0;
            this.ServerStatus_listView.UseCompatibleStateImageBehavior = false;
            this.ServerStatus_listView.View = System.Windows.Forms.View.Details;
            // 
            // control
            // 
            this.control.Text = "操作";
            this.control.Width = 80;
            // 
            // ClientIp
            // 
            this.ClientIp.Text = "客户端地址";
            this.ClientIp.Width = 170;
            // 
            // ClientPort
            // 
            this.ClientPort.Text = "端口";
            this.ClientPort.Width = 100;
            // 
            // Time
            // 
            this.Time.Text = "时间";
            this.Time.Width = 180;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.NVRStatus_listView);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(546, 298);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "NVR状态";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // NVRStatus_listView
            // 
            this.NVRStatus_listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NVRName,
            this.NVRIp,
            this.NVRPort,
            this.OnLineTime,
            this.Status,
            this.Channels});
            this.NVRStatus_listView.Location = new System.Drawing.Point(6, 6);
            this.NVRStatus_listView.Name = "NVRStatus_listView";
            this.NVRStatus_listView.Size = new System.Drawing.Size(534, 286);
            this.NVRStatus_listView.TabIndex = 4;
            this.NVRStatus_listView.UseCompatibleStateImageBehavior = false;
            this.NVRStatus_listView.View = System.Windows.Forms.View.Details;
            // 
            // NVRName
            // 
            this.NVRName.Text = "NVR名称";
            this.NVRName.Width = 90;
            // 
            // NVRIp
            // 
            this.NVRIp.Text = "IP地址";
            this.NVRIp.Width = 121;
            // 
            // NVRPort
            // 
            this.NVRPort.Text = "端口";
            this.NVRPort.Width = 56;
            // 
            // OnLineTime
            // 
            this.OnLineTime.Text = "上线时间";
            this.OnLineTime.Width = 131;
            // 
            // Status
            // 
            this.Status.Text = "状态";
            this.Status.Width = 47;
            // 
            // Channels
            // 
            this.Channels.Text = "通道数";
            this.Channels.Width = 86;
            // 
            // Status_groupBox
            // 
            this.Status_groupBox.Controls.Add(this.tabControl1);
            this.Status_groupBox.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Status_groupBox.Location = new System.Drawing.Point(12, 116);
            this.Status_groupBox.Name = "Status_groupBox";
            this.Status_groupBox.Size = new System.Drawing.Size(529, 306);
            this.Status_groupBox.TabIndex = 8;
            this.Status_groupBox.TabStop = false;
            this.Status_groupBox.Text = "状态栏";
            // 
            // CheckNvrStatus_Timer
            // 
            this.CheckNvrStatus_Timer.Interval = 1000;
            this.CheckNvrStatus_Timer.Tick += new System.EventHandler(this.CheckNvrStatus_Timer_Tick);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(101, 20);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(86, 38);
            this.button2.TabIndex = 9;
            this.button2.Text = "备份配置";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(769, 483);
            this.Controls.Add(this.Status_groupBox);
            this.Controls.Add(this.Contorl_groupBox);
            this.Controls.Add(this.Time_groupBox);
            this.Controls.Add(this.Connect_groupBox);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "  云台控制服务器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Connect_groupBox.ResumeLayout(false);
            this.Connect_groupBox.PerformLayout();
            this.Time_groupBox.ResumeLayout(false);
            this.TimeShow_panel.ResumeLayout(false);
            this.Contorl_groupBox.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.Status_groupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxStatus;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.GroupBox Connect_groupBox;
        private System.Windows.Forms.GroupBox Time_groupBox;
        private System.Windows.Forms.Panel TimeShow_panel;
        private DevComponents.DotNetBar.LabelX labelX_Date;
        private DevComponents.DotNetBar.LabelX labelX_Time;
        private System.Windows.Forms.GroupBox Contorl_groupBox;
        private System.Windows.Forms.Label NVRonlin_Label;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Config_button;
        private System.Windows.Forms.Button About_button;
        private System.Windows.Forms.Button Quit_button;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox Status_groupBox;
        private System.Windows.Forms.ListView ServerStatus_listView;
        private System.Windows.Forms.ColumnHeader control;
        private System.Windows.Forms.ColumnHeader ClientIp;
        private System.Windows.Forms.ColumnHeader ClientPort;
        private System.Windows.Forms.ColumnHeader Time;
        private System.Windows.Forms.Button RecordConfig_button;
        private System.Windows.Forms.ListView NVRStatus_listView;
        private System.Windows.Forms.ColumnHeader NVRName;
        private System.Windows.Forms.ColumnHeader NVRIp;
        private System.Windows.Forms.ColumnHeader NVRPort;
        private System.Windows.Forms.ColumnHeader OnLineTime;
        private System.Windows.Forms.ColumnHeader Status;
        private System.Windows.Forms.ColumnHeader Channels;
        private System.Windows.Forms.Button UdpSendFile_button;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer StopAutoVideo_Timer;
        private System.Windows.Forms.Timer CheckNvrStatus_Timer;
        private System.Windows.Forms.Button button2;
    }
}

