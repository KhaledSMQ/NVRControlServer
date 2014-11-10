#region ************************文件说明************************************
/// 作者(Author)：                     ShunBin Huang
/// 
/// 日期(Create Date)：            2014.6.20
/// 
/// 功能：                                    存储设置界面
///
/// 修改记录(Revision History)：无
///
#endregion *****************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using NVRControlServer.NVR.Control;
using NVRControlServer.Storage.Control;
using NVRControlServer.Storage.Module;

using NVRControlServer.DataBase.Dao;

namespace NVRControlServer.UI
{
    public partial class RecordConfigForm : Form
    {
        string autoVideoConfigFile = Application.StartupPath + "\\VideoSet.ini";//定义要读取的INI文件
        MainForm m_mainForm;
        StorageDataCenter storageDataCenter;
        DataOperate dataoperate = new DataOperate();
        
        public RecordConfigForm()
        {
            InitializeComponent();
        }

        public RecordConfigForm(MainForm mMainForm)
        {
            this.m_mainForm = mMainForm;
            storageDataCenter = m_mainForm.StorageControlCenter;
            InitializeComponent();
        }

        private void RecordConfigForm_Load(object sender, EventArgs e)
        {
            ShowConfigFileVideoSet_Timer.Start();

            for (int i = 1; i < 31; i++)
            {
                cboxDate.Items.Add(i);//给日期下拉列表赋值
            }

            for (int i = 1; i < 13; i++)
            {
                autoVideoLength_ComboBox.Items.Add(i * 5);
            }

            autoVideoFrequency_Combox.Text = dataoperate.ReadString("VideoSet", "Frequency", "", autoVideoConfigFile);

            autoVideoStartHour1.Value = Convert.ToDecimal(dataoperate.ReadString("VideoSet", "StartHour1", "", autoVideoConfigFile));
            autoVideoStartMin1.Value = Convert.ToDecimal(dataoperate.ReadString("VideoSet", "StartMin1", "", autoVideoConfigFile));
            autoVideoEndHour1.Value = Convert.ToDecimal(dataoperate.ReadString("VideoSet", "EndHour1", "", autoVideoConfigFile));
            autoVideoEndMin1.Value = Convert.ToDecimal(dataoperate.ReadString("VideoSet", "EndMin1", "", autoVideoConfigFile));

            autoVideoStartHour2.Value = Convert.ToDecimal(dataoperate.ReadString("VideoSet", "StartHour2", "", autoVideoConfigFile));
            autoVideoStartMin2.Value = Convert.ToDecimal(dataoperate.ReadString("VideoSet", "StartMin2", "", autoVideoConfigFile));
            autoVideoEndHour2.Value = Convert.ToDecimal(dataoperate.ReadString("VideoSet", "EndHour2", "", autoVideoConfigFile));
            autoVideoEndMin2.Value = Convert.ToDecimal(dataoperate.ReadString("VideoSet", "EndMin2", "", autoVideoConfigFile));

            autoVideoStartHour3.Value = Convert.ToDecimal(dataoperate.ReadString("VideoSet", "StartHour3", "", autoVideoConfigFile));
            autoVideoStartMin3.Value = Convert.ToDecimal(dataoperate.ReadString("VideoSet", "StartMin3", "", autoVideoConfigFile));
            autoVideoEndHour3.Value = Convert.ToDecimal(dataoperate.ReadString("VideoSet", "EndHour3", "", autoVideoConfigFile));
            autoVideoEndMin3.Value = Convert.ToDecimal(dataoperate.ReadString("VideoSet", "EndMin3", "", autoVideoConfigFile));

            autoVideoStartHour4.Value = Convert.ToDecimal(dataoperate.ReadString("VideoSet", "StartHour4", "", autoVideoConfigFile));
            autoVideoStartMin4.Value = Convert.ToDecimal(dataoperate.ReadString("VideoSet", "StartMin4", "", autoVideoConfigFile));
            autoVideoEndHour4.Value = Convert.ToDecimal(dataoperate.ReadString("VideoSet", "EndHour4", "", autoVideoConfigFile));
            autoVideoEndMin4.Value = Convert.ToDecimal(dataoperate.ReadString("VideoSet", "EndMin4", "", autoVideoConfigFile));

            autoVideoLength_ComboBox.Text = dataoperate.ReadString("VideoSet", "VideoLength", "", autoVideoConfigFile);
            cboxWeek.Text = dataoperate.ReadString("VideoSet", "Week", "", autoVideoConfigFile);
            cboxDate.Text = dataoperate.ReadString("VideoSet", "Date", "", autoVideoConfigFile);

        }

        private void ShowErroProvider(object sender, string erroMessage)
        {
            this.AutoVideoConfig_ErrorProvider.Clear();
            this.AutoVideoConfig_ErrorProvider.SetError((Control)sender, erroMessage);
        }

        private bool CheckParamForAutoVideoSet()
        {

            if (Convert.ToDecimal(autoVideoStartHour1.Value.ToString())
                > Convert.ToDecimal(autoVideoEndHour1.Value.ToString()))
            {
                ShowErroProvider(this.autoVideoEndHour1, "结束时间应大于开始时间");
                return false;
            }

            if (Convert.ToDecimal(autoVideoStartHour2.Value.ToString())
               > Convert.ToDecimal(autoVideoEndHour2.Value.ToString()))
            {
                ShowErroProvider(this.autoVideoEndHour2, "结束时间应大于开始时间");
                return false;
            }

            if (Convert.ToDecimal(autoVideoStartHour3.Value.ToString())
               > Convert.ToDecimal(autoVideoEndHour3.Value.ToString()))
            {
                ShowErroProvider(this.autoVideoEndHour3, "结束时间应大于开始时间");
                return false;
            }

            if (Convert.ToDecimal(autoVideoStartHour4.Value.ToString())
               > Convert.ToDecimal(autoVideoEndHour4.Value.ToString()))
            {
                ShowErroProvider(this.autoVideoEndHour4, "结束时间应大于开始时间");
                return false;
            }


            if(Convert.ToDecimal(autoVideoStartHour1.Value.ToString())
                > Convert.ToDecimal(autoVideoStartHour2.Value.ToString()))
            {
                ShowErroProvider(this.autoVideoStartHour2, "第一段开始时间应小于第二段开始时间");
                return false;
            }

            if (Convert.ToDecimal(autoVideoEndHour1.Value.ToString())
                > Convert.ToDecimal(autoVideoStartHour2.Value.ToString()))
            {
                ShowErroProvider(this.autoVideoStartHour2, "第二段开始时间应大于第一段结束时间");
                return false;
            }


            if (Convert.ToDecimal(autoVideoStartHour2.Value.ToString())
                > Convert.ToDecimal(autoVideoStartHour3.Value.ToString()))
            {
                ShowErroProvider(this.autoVideoStartHour3, "第二段开始时间应小于第三段开始时间");
                return false;
            }

            if (Convert.ToDecimal(autoVideoEndHour2.Value.ToString())
               > Convert.ToDecimal(autoVideoStartHour3.Value.ToString()))
            {
                ShowErroProvider(this.autoVideoStartHour3, "第三段开始时间应大于第二段结束时间");
                return false;
            }


            if (Convert.ToDecimal(autoVideoStartHour3.Value.ToString())
                > Convert.ToDecimal(autoVideoStartHour4.Value.ToString()))
            {
                ShowErroProvider(this.autoVideoStartHour4, "第三段开始时间应小于第四段开始时间");
                return false;
            }


            if (Convert.ToDecimal(autoVideoEndHour3.Value.ToString())
               > Convert.ToDecimal(autoVideoStartHour4.Value.ToString()))
            {
                ShowErroProvider(this.autoVideoStartHour4, "第四段开始时间应大于第三段结束时间");
                return false;
            }

           
            return true;
        }


        //确定修改INI设置文件内容
        private void autoVideoConfigFileSure_Button_Click(object sender, EventArgs e)
        {
            if (!CheckParamForAutoVideoSet())
                return;

            try
            {
                DataOperate.WritePrivateProfileString("VideoSet", "Frequency", autoVideoFrequency_Combox.Text, autoVideoConfigFile);
                DataOperate.WritePrivateProfileString("VideoSet", "StartHour1", autoVideoStartHour1.Value.ToString(), autoVideoConfigFile);
                DataOperate.WritePrivateProfileString("VideoSet", "StartMin1", autoVideoStartMin1.Value.ToString(), autoVideoConfigFile);
                DataOperate.WritePrivateProfileString("VideoSet", "EndHour1", autoVideoEndHour1.Value.ToString(), autoVideoConfigFile);
                DataOperate.WritePrivateProfileString("VideoSet", "EndMin1", autoVideoEndMin1.Value.ToString(), autoVideoConfigFile);
                DataOperate.WritePrivateProfileString("VideoSet", "StartHour2", autoVideoStartHour2.Value.ToString(), autoVideoConfigFile);
                DataOperate.WritePrivateProfileString("VideoSet", "StartMin2", autoVideoStartMin2.Value.ToString(), autoVideoConfigFile);
                DataOperate.WritePrivateProfileString("VideoSet", "EndHour2", autoVideoEndHour2.Value.ToString(), autoVideoConfigFile);
                DataOperate.WritePrivateProfileString("VideoSet", "EndMin2", autoVideoEndMin2.Value.ToString(), autoVideoConfigFile);
                DataOperate.WritePrivateProfileString("VideoSet", "StartHour3", autoVideoStartHour3.Value.ToString(), autoVideoConfigFile);
                DataOperate.WritePrivateProfileString("VideoSet", "StartMin3", autoVideoStartMin3.Value.ToString(), autoVideoConfigFile);
                DataOperate.WritePrivateProfileString("VideoSet", "EndHour3", autoVideoEndHour3.Value.ToString(), autoVideoConfigFile);
                DataOperate.WritePrivateProfileString("VideoSet", "EndMin3", autoVideoEndMin3.Value.ToString(), autoVideoConfigFile);
                DataOperate.WritePrivateProfileString("VideoSet", "StartHour4", autoVideoStartHour4.Value.ToString(), autoVideoConfigFile);
                DataOperate.WritePrivateProfileString("VideoSet", "StartMin4", autoVideoStartMin4.Value.ToString(), autoVideoConfigFile);
                DataOperate.WritePrivateProfileString("VideoSet", "EndHour4", autoVideoEndHour4.Value.ToString(), autoVideoConfigFile);
                DataOperate.WritePrivateProfileString("VideoSet", "EndMin4", autoVideoEndMin4.Value.ToString(), autoVideoConfigFile);
                DataOperate.WritePrivateProfileString("VideoSet", "VideoLength", autoVideoLength_ComboBox.Text, autoVideoConfigFile);
                DataOperate.WritePrivateProfileString("VideoSet", "Week", cboxWeek.Text, autoVideoConfigFile);
                DataOperate.WritePrivateProfileString("VideoSet", "Date", cboxDate.Text, autoVideoConfigFile);
                MessageBox.Show("定时录像设置成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show("定时录像设置失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // 根据录像频率下拉列表框中的选择项控制其他控件的可用状态
        private void ControlState()
        {
            int index = autoVideoFrequency_Combox.SelectedIndex;
            switch (index)
            {
                case 0:
                    autoVideoStartHour1.Enabled = autoVideoStartMin1.Enabled = true;
                    cboxWeek.Enabled = cboxDate.Enabled = false;
                    break;
                case 1:
                    autoVideoStartHour1.Enabled = autoVideoStartMin1.Enabled = cboxWeek.Enabled = true;
                    cboxDate.Enabled = false;
                    break;
                case 2:
                    autoVideoStartHour1.Enabled = autoVideoStartMin1.Enabled = cboxDate.Enabled = true;
                    cboxWeek.Enabled = false;
                    break;
            }
        }


        private void ShowConfigFileVideoSetTimer_Tick(object sender, EventArgs e)
        {
            ControlState();
        }


      

        private void Applicate_button_Click(object sender, EventArgs e)
        {
            storageDataCenter.StartAllTask();
        }

        private void OK_button_Click(object sender, EventArgs e)
        {
            #region 采用计划录像
            /*
            if (CheckParamForRecord())
            {
                NVRControler mNvrControler = m_mainForm.NvrList[0];
                if (mNvrControler != null)
                {
                    if (mNvrControler.RealPlay(int.Parse(this.ChannelSelect_comboBox.Text)))
                    {
                        MessageBox.Show("通道预览成功");
                        //选择的录像通道
                        int mSelectChannel = int.Parse(this.ChannelSelect_comboBox.Text);
                        //录像时间长度
                        int mRecordTimeLong = int.Parse(this.RecordTimeLong_comboBox.Text);
                        //当前时间与任务开始时间天数差
                        int mDifferenceDay = TimeHelper.GetDifferenceDay(DateTime.Now.DayOfWeek.ToString(),
                            this.DayOfWeek_comboBox.Text);
                        //任务开始时间 = 星期几 + 时间控件(小时,分钟,秒)
                        DateTime mFirstStartTime = this.StartTime_dateTimePicker1.Value.AddDays(mDifferenceDay);
                        //任务结束时间 = 星期几 + 时间控件(小时,分钟,秒)
                        DateTime mFirstEndTime = this.EndTime_dateTimePicker1.Value.AddDays(mDifferenceDay);
                        //获得任务开始时间与结束时间的时间差
                        TimeSpan mStEnTimeSpain = mFirstEndTime - mFirstStartTime;
                        //存储路径 = 主路径 + NVR名 + 通道名
                        string mStorageDataPath = m_mainForm.StorageDataPath + NVRSelect_comboBox.Text + "/"
                            + mSelectChannel + "/";

                        StorageTaskParameter mStorageThreadParameter = new StorageTaskParameter
                        (mSelectChannel, mRecordTimeLong, mNvrControler, mStorageDataPath, mStEnTimeSpain, false);
                        //开始时间 = 星期几 + 时间控件(小时,分钟,秒) 
                        //开始时间 + 循环周期(小时,分钟,秒)
                        CycExecution cycShedule = new CycExecution(mFirstStartTime, new TimeSpan(0, 5, 0));
                        StorageTask cycSheduleTask = new StorageTask(cycShedule, TimeHelper.GenerateRandomNumber(8),
                          "hellorecord", " record", mStorageThreadParameter);

                        m_StorageDataCenter.AddTask(cycSheduleTask);
                        MessageBox.Show("计划设置成功");
                    }
                    else
                    {
                        MessageBox.Show("通道预览失败");
                        return;
                    }
                }
            }
              */
            #endregion
        }

        private void Cancel_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //启用计划录像
        //private void IsRecordPlan_checkBox_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (IsRecordPlan_checkBox.Checked)
        //        this.RecordTime_groupBox.Enabled = true;
        //    else
        //        this.RecordTime_groupBox.Enabled = false;
        //}

        //// 启用全天录像
        //private void AllDay_checkBox_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (AllDayRecord_checkBox.Checked)
        //        this.StartEndTime_groupBox.Enabled = true;
        //    else
        //        this.StartEndTime_groupBox.Enabled = false;
        //}


       

        private void button2_Click(object sender, EventArgs e)
        {
            for (int j = 0; j < m_mainForm.NvrList.Count; j++)
            {
                    NVRControler nvrControler = m_mainForm.NvrList[j];
                    NVRChannel[] nvrChannels = nvrControler.NvrChannels;
                    for (int i = 0; i < nvrChannels.Length; i++)
                    {
                            string channelStorageDataPath = m_mainForm.StorageDataPath +
                                nvrControler.NvrName + "/" + i + "/";
                            StorageTaskParameter storageThreadParameter = new StorageTaskParameter
                            (j, i, 15, 2, nvrControler, channelStorageDataPath, new TimeSpan(0, 0, 1), true);
                            DateTime sheduleTime = DateTime.Now;
                            ScheduleExecutionOnce future = new ScheduleExecutionOnce(sheduleTime);
                            StorageTask sheduleTask = new StorageTask(future);
                            sheduleTask.Job += new TimerCallback(sheduleTask.Execute);
                            sheduleTask.JobParam = storageThreadParameter;
                            storageDataCenter.AddTask(sheduleTask);
                    }
            }
            storageDataCenter.StartAllTask();
            this.button2.Enabled = false;
            this.button3.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ////未来执行一次
            //DateTime sheduleTime = DateTime.Now.AddSeconds(10);
            //ScheduleExecutionOnce future = new ScheduleExecutionOnce(sheduleTime);
            //Task sheduleTask = new Task(future);
            //sheduleTask.Job += new TimerCallback(sheduleTask.Execute);
            //sheduleTask.JobParam = "Test excute once task";

            //执行多次
            //CycExecution cyc = new CycExecution(new TimeSpan(0, 0, 2));
            //Task cysTask = new Task(cyc);
            //cysTask.Job += new TimerCallback(cysTask.Execute);
            //cysTask.JobParam = "Test cyc task";

            //在未来的时间内执行多次
            //CycExecution cycShedule = new CycExecution(DateTime.Now.AddSeconds(8), new TimeSpan(0, 0, 2));
            //Task cycSheduleTask = new Task(cycShedule);
            //cycSheduleTask.Job += new TimerCallback(cysTask.Execute);
            //cycSheduleTask.JobParam = "Test cyc Shedule task";

        }


        private void button3_Click(object sender, EventArgs e)
        {
            storageDataCenter.StopAllTask();
            this.button2.Enabled = true;
            this.button3.Enabled = false;
        }



        private void btnClose_Click(object sender, EventArgs e)
        {

        }






    }
}
