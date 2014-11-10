#region ************************文件说明************************************
/// 作者(Author)：                    黄顺彬
/// 
/// 日期(Create Date)：            2014.6.21
/// 
/// 功能：                                  存储数据控制中心
///
/// 修改记录(Revision History)：2014.10.17  增加自动录像功能
///
#endregion *****************************************************************

using System;
using System.Collections.Generic;


using NVRControlServer.NVR.Control;
using NVRControlServer.Storage.Utils;
using NVRControlServer.Storage.Module;
using System.Windows.Forms;
using NVRControlServer.DataBase.Dao;
using System.Threading;


namespace NVRControlServer.Storage.Control
{
    public class StorageDataCenter
    {

        #region 1.变量属性

        #region 1.1 变量
        private string storageDataPath = "e:/";                                                              //存储硬盘目录
        private StorageTaskCenter taskCenter;                                                             //存储任务控制中心
        private List<NVRControler> nvrList;                                                                  //保存所有Nvr设备 
        private HashSet<StorageTask> taskSet;                                                           //存储任务暂存哈希表
        private System.Windows.Forms.Timer startAutoVideoTimer;
        private System.Windows.Forms.Timer endAutoVideoTimer;
        private DataOperate dataoperate;
        private string autoVideofigFile = Application.StartupPath + "\\VideoSet.ini";//定义要读取的INI文件
        private int timerInterval = 1000;
        #endregion 1.1 变量

        #region 1.2 属性

        public List<NVRControler> NvrList
        {
            get { return nvrList; }
            set { nvrList = value; }
        }

        public string StorageDataPath
        {
            get { return storageDataPath; }
            set { storageDataPath = value; }
        }

        public DataOperate Dataoperate
        {
            get 
            {
                if (dataoperate == null)
                {
                    dataoperate = new DataOperate();
                    return dataoperate;
                }
                else
                    return dataoperate;
            }
            set { dataoperate = value; }
        }

        public int TimerInterval
        {
            get { return timerInterval; }
            set { timerInterval = value; }
        }

        public System.Windows.Forms.Timer StartAutoVideoTimer
        {
            get
            {
                if (startAutoVideoTimer == null)
                {
                    startAutoVideoTimer = new System.Windows.Forms.Timer();
                    startAutoVideoTimer.Tick += new EventHandler(StartAutoVideo_Timer_Tick);
                    return startAutoVideoTimer;
                }
                else
                    return startAutoVideoTimer;
            }
        }

        public System.Windows.Forms.Timer EndAutoVideoTimer
        {
            get
            {
                if (endAutoVideoTimer == null)
                {
                    endAutoVideoTimer = new System.Windows.Forms.Timer();
                    endAutoVideoTimer.Tick += new EventHandler(EndAutoVideo_Timer_Tick);
                    return endAutoVideoTimer;
                }
                else
                    return endAutoVideoTimer;
            }
        }

        #endregion 1.2 属性

        #endregion 1.变量属性

        #region 2.构造方法
        public StorageDataCenter(string storageDataPath, 
            List<NVRControler> mNvrList,
            string autoVideoConfigFile)
        {
            this.storageDataPath = storageDataPath;
            this.nvrList = mNvrList;
            this.autoVideofigFile = autoVideoConfigFile;
            dataoperate = new DataOperate();
        }
        #endregion 2.构造方法

        #region 3.私有方法

        private void StartAutoVideo_Timer_Tick(object sender, EventArgs e)
        {
            string autoVideoFrequency = dataoperate.ReadString("VideoSet", "Frequency", "", autoVideofigFile);

            int autoVideoStartHour1 = Convert.ToInt32(dataoperate.ReadString("VideoSet", "StartHour1", "", autoVideofigFile));
            int autoVideoStartMin1 = Convert.ToInt32(dataoperate.ReadString("VideoSet",  "StartMin1", "", autoVideofigFile));

            int autoVideoStartHour2 = Convert.ToInt32(dataoperate.ReadString("VideoSet", "StartHour2", "", autoVideofigFile));
            int autoVideoStartMin2 = Convert.ToInt32(dataoperate.ReadString("VideoSet", "StartMin2", "", autoVideofigFile));

            int autoVideoStartHour3 = Convert.ToInt32(dataoperate.ReadString("VideoSet", "StartHour3", "", autoVideofigFile));
            int autoVideoStartMin3 = Convert.ToInt32(dataoperate.ReadString("VideoSet", "StartMin3", "", autoVideofigFile));

            int autoVideoStartHour4 = Convert.ToInt32(dataoperate.ReadString("VideoSet", "StartHour4", "", autoVideofigFile));
            int autoVideoStartMin4 = Convert.ToInt32(dataoperate.ReadString("VideoSet", "StartMin4", "", autoVideofigFile));

            string autoVideoWeek = dataoperate.ReadString("VideoSet", "Week", "", autoVideofigFile);
            string autoVideoDate = dataoperate.ReadString("VideoSet", "Date", "", autoVideofigFile);

            Boolean startFlag = false;

            switch (autoVideoFrequency)
            {
                case "每天":
                    startFlag = true;
                    break;
                case "每周":
                    if (TimeHelper.GetCNDateOfWeek() == autoVideoWeek)
                        startFlag = true;
                    break;
                case "每月":
                    if (DateTime.Now.Month == Convert.ToInt32(autoVideoDate))
                        startFlag = true;
                    break;
            }

            int nowHour = DateTime.Now.Hour;
            int nowMinute = DateTime.Now.Minute;
            
            if (startFlag && 
                    ((nowHour == autoVideoStartHour1 && nowMinute == autoVideoStartMin1)|| 
                        (nowHour == autoVideoStartHour2 && nowMinute == autoVideoStartMin2)||
                            (nowHour == autoVideoStartHour3 && nowMinute == autoVideoStartMin3)||
                                (nowHour == autoVideoStartHour4 && nowMinute == autoVideoStartMin4)))
            {
                StartAutoVideoTimer.Enabled = false;
                EndAutoVideoTimer.Enabled = true;
                StopAutoVideo();
                AutoVideo();
            }
        }

        private void EndAutoVideo_Timer_Tick(object sender, EventArgs e)
        {
            int autoVideoEndHour1 = Convert.ToInt32(dataoperate.ReadString("VideoSet", "EndHour1", "", autoVideofigFile));
            int autoVideoEndMin1 = Convert.ToInt32(dataoperate.ReadString("VideoSet", "EndMin1", "", autoVideofigFile));
            int autoVideoEndHour2 = Convert.ToInt32(dataoperate.ReadString("VideoSet", "EndHour2", "", autoVideofigFile));
            int autoVideoEndMin2 = Convert.ToInt32(dataoperate.ReadString("VideoSet", "EndMin2", "", autoVideofigFile));
            int autoVideoEndHour3 = Convert.ToInt32(dataoperate.ReadString("VideoSet", "EndHour3", "", autoVideofigFile));
            int autoVideoEndMin3 = Convert.ToInt32(dataoperate.ReadString("VideoSet", "EndMin3", "", autoVideofigFile));
            int autoVideoEndHour4 = Convert.ToInt32(dataoperate.ReadString("VideoSet", "EndHour4", "", autoVideofigFile));
            int autoVideoEndMin4 = Convert.ToInt32(dataoperate.ReadString("VideoSet", "EndMin4", "", autoVideofigFile));

            int nowHour = DateTime.Now.Hour;
            int nowMinute = DateTime.Now.Minute;
            if((nowHour == autoVideoEndHour1 && nowMinute == autoVideoEndMin1) ||
                (nowHour == autoVideoEndHour2 && nowMinute == autoVideoEndMin2) ||
                 (nowHour == autoVideoEndHour3 && nowMinute == autoVideoEndMin3) ||
                   (nowHour == autoVideoEndHour4 && nowMinute == autoVideoEndMin4))
            {
                StopAutoVideo();
                EndAutoVideoTimer.Enabled = false;
                StartAutoVideoTimer.Enabled = true;
            }
        }

        private void AutoVideo()
        {
            int autoVideoLength = Convert.ToInt32(dataoperate.ReadString("VideoSet", "VideoLength", "", autoVideofigFile));
            autoVideoLength = ((autoVideoLength == 0) ? 15 : autoVideoLength);
            for (int j = 0; j < NvrList.Count; j++)
            {
                NVRControler nvrControler = NvrList[j];
                NVRChannel[] nvrChannels = nvrControler.NvrChannels;
                for (int i = 0; i < nvrChannels.Length; i++)
                {
                    string channelStorageDataPath = StorageDataPath +
                        nvrControler.NvrName + "/" + i + "/";
                    StorageTaskParameter storageThreadParameter = new StorageTaskParameter
                    (j, i, autoVideoLength, 2, nvrControler, channelStorageDataPath, new TimeSpan(0, 0, 1), true);
                    DateTime sheduleTime = DateTime.Now;
                    ScheduleExecutionOnce future = new ScheduleExecutionOnce(sheduleTime);
                    StorageTask sheduleTask = new StorageTask(future);
                    sheduleTask.Job += new TimerCallback(sheduleTask.Execute);
                    sheduleTask.JobParam = storageThreadParameter;
                    AddTask(sheduleTask);
                }
            }
            StartAllTask();
        }


        #endregion 3. 私有方法

        #region 4.公有方法
        public bool Initalization()
        {
            taskCenter = new StorageTaskCenter();

            //在硬盘中创建主目录
            if (!FileHelper.CreateNvrFoleder(storageDataPath, nvrList))
                return false;

            taskSet = new HashSet<StorageTask>();

            return true;
        }

        public void StartAutoVideo()
        {
            StartAutoVideoTimer.Enabled = true;
            //StartAutoVideoTimer.Start();
        }

        public void StopAutoVideo()
        {
            StopAllTask();
        }

        public void AddTask(StorageTask mTask)
        {
            taskSet.Add(mTask);
        }

        //开始任务管理中心中的所有任务
         public void StartAllTask()
        {
            foreach (StorageTask t in taskSet)
                taskCenter.AddTask(t);

            taskSet.Clear();
            taskCenter.StartAllTask();
        }

        //停止任务管理中心中的所有任务
        public void StopAllTask()
         {
            taskCenter.TerminateAllTask();
        }

        //打印缓冲哈希表
        public void PrintSet()
        {
            foreach (StorageTask t in taskSet)
            {
                Console.Write(t.TaskID);
            }
        }
        #endregion 4.公有方法

    }
}
