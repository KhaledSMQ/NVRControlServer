#region ************************文件说明************************************
/// 作者(Author)：                         黄顺彬
/// 
/// 日期(Create Date)：                2014.11.4
/// 
/// 功能：                                        存储NVR原始历史数据任务
///
/// 修改记录(Revision History)：       无
///
#endregion *****************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;

using NVRControlServer.Storage.Module;
using NVRControlServer.NVR.Control;
using NVRControlServer.Storage.Utils;
using NVRControlServer.DataBase.Dao;
using NVRControlServer.DataBase.SqlServerDAL;
using NVRControlServer.DataBase.Modle;



namespace NVRControlServer.Storage.Model
{
    public class BackupTask
    {
        #region 1. 变量属性

        #region 1.1 变量
        private string taskId;//任务标识号        
        private string taskName;//任务名称
        private string taskDescription;//任务描述
        private System.Timers.Timer checkBackupTaskTimer;
        private TimerCallback taskExecJob;//定时后执行任务函数
        private Thread taskWorkThread;//任务工作线程
        private object taskWorkThreadParam;//任务工作线程参数
        private AutoResetEvent checkTaskStatusAutoEvent;

        private object lockObject;
        private int checkTaskStatusFlag = 0;
        private string fileSaveFormat;
        private DateTime realStartBackUpTime;
        private DateTime realFinishBackUpTime;

        #endregion 1.1 变量

        #region 1.2 属性
        public string TaskID
        {
            get{return this.taskId;}
        }

        public string TaskName
        {
            get{return taskName;}
            set{taskName = value;}
        }

        public string TaskDescription
        {
            get{return taskDescription;}
            set{taskDescription = value;}
        }

        public object TaskWorkThreadParam
        {
            set{taskWorkThreadParam = value;}
        }

        public TimerCallback TaskExecJob
        {
            get{return taskExecJob;}
            set{taskExecJob = value;}
        }


        public object LockObject
        {
            get
            {
                if (lockObject == null)
                    lockObject = new object();
                return lockObject;
            }
        }
        #endregion 1.2 属性

        #endregion 1. 变量属性

        #region 2. 构造函数

         public BackupTask(object param)
         {
             this.taskWorkThreadParam = param;
             checkTaskStatusAutoEvent = new AutoResetEvent(false);
             checkBackupTaskTimer = new System.Timers.Timer();
             checkBackupTaskTimer.Interval = 10000;
             checkBackupTaskTimer.Elapsed += new ElapsedEventHandler(CheckTaskStatus);
         }

         ~BackupTask()
         {
             //taskTimer.Dispose();
             //staskWorkThread.Abort();
         }

        #endregion 2. 构造方法


        #region 3. 私有方法

         private void CheckTaskStatus(object sender, System.Timers.ElapsedEventArgs e)
         {

             lock (LockObject)
             {
                 if (checkTaskStatusFlag == 0)
                     checkTaskStatusFlag = 1;
                 else
                     return;
             }

             BuckupTaskParameter parameter = (BuckupTaskParameter)taskWorkThreadParam;
             NVRControler nvrControler = parameter.ThreadPamamterNvrControler;
             int nvrSelectIndex = parameter.NvrSelectIndex;
             int channelSelectIndex = parameter.ChannelSelectIndex;
             DateTime planeStartTime = parameter.StartTime;
             DateTime planEndTime = parameter.EndTime;
             string backupSetConfigFile = parameter.BackupTaskSetConfigFile;
             string backupDataSavePath = parameter.BackupDataSavePath;

             int downLoadPos = nvrControler.NvrGetDownLoadPos();
             //Console.WriteLine("通道" + channelSelectIndex  + "下载进度" + downLoadPos);

             //先查看下载的文件大小为0
             string backupSaveFileName = FileHelper.GetVideoFileName(nvrSelectIndex, channelSelectIndex, planeStartTime);
             string backupSaveFullPathFileName = backupDataSavePath + "/" + backupSaveFileName;


             //下载完成后写入配置文件下一个备份时间
             if (downLoadPos == 100)
             {
                Console.WriteLine("已经完成下载了");


                if (nvrControler.NvrStopDownLoad())
                {
                    if (FileHelper.GetFileLength(backupSaveFullPathFileName) == 0)
                    {
                        Console.WriteLine("删除的文件为" + backupSaveFullPathFileName);
                        FileHelper.DeleteFile(backupSaveFullPathFileName);
                    }
                    else
                    {

                        VideoInfoDAL videoInfoDal = new VideoInfoDAL();
                        VideoInfoData videoInfoData = new VideoInfoData();
                        videoInfoData.nvrId = nvrSelectIndex;
                        videoInfoData.channelId = channelSelectIndex;
                        videoInfoData.startTime =  planeStartTime;
                        videoInfoData.endTime = planEndTime;
                        videoInfoData.filePath = backupDataSavePath;
                        videoInfoData.fileName = backupSaveFileName;
                        videoInfoDal.Insert(videoInfoData);

                        Console.WriteLine("停止下载,写入下一个需备份的时间点");
                        DataOperate.WritePrivateProfileString(nvrControler.NvrName, channelSelectIndex.ToString() + "backupTime",
                               TimeHelper.DateTimeToString(planEndTime), backupSetConfigFile);
                    }
                }
                
                this.checkBackupTaskTimer.Enabled = false;
                this.checkBackupTaskTimer.Stop();
                 checkTaskStatusAutoEvent.Set();
             }

            //下载过程中出现网络问题则在断掉的地方重新下载
             else if (downLoadPos == 200)
             {
                 Console.WriteLine("出现网络故障");


                 realFinishBackUpTime = DateTime.Now;
                 TimeSpan timeSpan = realFinishBackUpTime - realStartBackUpTime;
                 DateTime nextBackupTime = planeStartTime + timeSpan;


                 if (nvrControler.NvrStopDownLoad())
                 {
                     DataOperate.WritePrivateProfileString(nvrControler.NvrName, channelSelectIndex.ToString() + "backupTime",
                           TimeHelper.DateTimeToString(nextBackupTime), backupSetConfigFile);

                     VideoInfoDAL videoInfoDal = new VideoInfoDAL();
                     VideoInfoData videoInfoData = new VideoInfoData();
                     videoInfoData.nvrId = nvrSelectIndex;
                     videoInfoData.channelId = channelSelectIndex;
                     videoInfoData.startTime = planeStartTime;
                     videoInfoData.endTime = realFinishBackUpTime;
                     videoInfoData.filePath = backupDataSavePath;
                     videoInfoData.fileName = backupSaveFileName;
                     videoInfoDal.Insert(videoInfoData);

                     this.checkBackupTaskTimer.Enabled = false;
                     this.checkBackupTaskTimer.Stop();
                     checkTaskStatusAutoEvent.Set();
                 }
             }

              //下载出现错误
             else if (downLoadPos == -1)
             {
                 Console.WriteLine("下载失败，可能没有这个文件");
                 if (nvrControler.NvrStopDownLoad())
                     DataOperate.WritePrivateProfileString(nvrControler.NvrName, channelSelectIndex.ToString() + "backupTime",
                            TimeHelper.DateTimeToString(planEndTime), backupSetConfigFile);

                 this.checkBackupTaskTimer.Enabled = false;
                 this.checkBackupTaskTimer.Stop();
                 checkTaskStatusAutoEvent.Set();
             }

             lock (LockObject)
                 checkTaskStatusFlag = 0;
         }
        #endregion 3. 私有方法


        #region 4. 公有方法
         public void Start()
         {
             WorkFunc();
         }

         private void WorkFunc()
         {
             BuckupTaskParameter parameter = (BuckupTaskParameter)taskWorkThreadParam;
             if (null != parameter)
             {
                 NVRControler nvrControler = parameter.ThreadPamamterNvrControler;
                 int nvrSelectIndex = parameter.NvrSelectIndex;
                 int channelSelectIndex = parameter.ChannelSelectIndex;
                 string nvrSelectName = nvrControler.NvrName;
                 string channelSelectName = nvrControler.
                     NvrChannels[channelSelectIndex].ChannelName;

                 int backupVideoTimeLong = FileHelper.Min2Subtle(parameter.BackupTimeLong);
                 int backupRestartWaitTimeLong = FileHelper.Min2Subtle(parameter.RestartWaitTimeLong);
                 string backupDataSavePath = parameter.BackupDataSavePath;

                 TimeSpan startendTimeSpan = parameter.StEndTimeSpan;
                 DateTime startTime = parameter.StartTime;
                 DateTime endTime = parameter.EndTime;

                 string backupSaveFileName = FileHelper.GetVideoFileName(nvrSelectIndex, channelSelectIndex, startTime);
                 //string backupSaveFileName = TimeHelper.DateTimeToString(startTime) + ".avi";
                 string backupSaveFullPathFileName = backupDataSavePath + "/" + backupSaveFileName;

                 int startYear = startTime.Year;
                 int startMonth = startTime.Month;
                 int startDay = startTime.Day;
                 int startHour = startTime.Hour;
                 int startMin = startTime.Minute;
                 int startSecond = startTime.Second;

                 int endYear = endTime.Year;
                 int endMonth = endTime.Month;
                 int endDay = endTime.Day;
                 int endHour = endTime.Hour;
                 int endMin = endTime.Minute;
                 int endSecond = endTime.Second;

                 if (nvrControler.NvrDownLoadFileByTime(channelSelectIndex, startYear, startMonth, startDay, startHour, startMin, startSecond,
                     endYear, endMonth, endDay, endHour, endMin, endSecond, backupSaveFullPathFileName))
                 {
                     Console.WriteLine("开始下载成功");
                     realStartBackUpTime = DateTime.Now;
                     checkBackupTaskTimer.Enabled = true;
                     checkBackupTaskTimer.Start();
                     checkTaskStatusAutoEvent.WaitOne();
                 }
                 Console.WriteLine("结束下载");
                 checkBackupTaskTimer.Enabled = false;
                 checkBackupTaskTimer.Stop();
                 checkBackupTaskTimer.Dispose();
             }
         }

         public void Stop()
         {


         }





        #endregion 4. 公有方法


    }
}
