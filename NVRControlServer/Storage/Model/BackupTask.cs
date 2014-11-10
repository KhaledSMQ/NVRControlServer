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

namespace NVRControlServer.Storage.Model
{
    public class BackupTask
    {
        #region 1. 变量属性

        #region 1.1 变量
        private string taskId;//任务标识号        
        private string taskName;//任务名称
        private string taskDescription;//任务描述
        private System.Threading.Timer taskTimer;//任务定时器
        private System.Timers.Timer checkBackupTaskTimer;
        private ISchedule taskSchedule;//任务计划
        private DateTime taskNextExecuteTime;//计划中下一开始执行时间
        private DateTime taskNextExecuteEndTiem;//计划中下一结束执行时间
        private DateTime taskLastExecuteTime;//计划最后一次的开始执行时间
        private DateTime taskLastExecuteEndTime;//计划最后一次的结束执行时间
        private TimerCallback taskExecJob;//定时后执行任务函数
        private Thread taskWorkThread;//任务工作线程
        private object taskWorkThreadParam;//任务工作线程参数
        private bool taskWorkThreadRunFlag;//任务工作线程工作指示
        private AutoResetEvent autoEvent;
        private object lockObject;
        private int checkTaskStatusFlag = 0;
        private string fileSaveFormat;
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

        public ISchedule TaskShedule
        {
            get{return taskSchedule;}
        }
      
        public DateTime NextExecuteTime
        {
            get{return taskNextExecuteTime;}
        }

        public TimerCallback TaskExecJob
        {
            get{return taskExecJob;}
            set{taskExecJob = value;}
        }

        public DateTime LastExecuteTime
        {
            get{return taskLastExecuteTime;}
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

         public BackupTask(ISchedule shedule)
        {
            if (shedule == null)
            {
                throw (new ArgumentNullException("shedule"));
            }
            taskSchedule = shedule;
        }

         public BackupTask(ISchedule shedule, string taskId, string taskName, string description, object param)
        {
            if (shedule == null)
            {
                throw (new ArgumentNullException("shedule"));
            }
            this.taskSchedule = shedule;
            this.taskId = taskId;
            this.taskName = taskName;
            this.taskDescription = description;
            this.taskWorkThreadParam = param;
            this.taskExecJob += new TimerCallback(this.Execute);
        }

         public BackupTask(object param)
         {
             this.taskWorkThreadParam = param;

             autoEvent = new AutoResetEvent(false);

             checkBackupTaskTimer = new System.Timers.Timer();
             checkBackupTaskTimer.Interval = 2000;
             checkBackupTaskTimer.Elapsed += new ElapsedEventHandler(CheckTaskStatus);
             //checkBackupTaskTimer.Enabled = true;
             //checkBackupTaskTimer.Start();

         }


         ~BackupTask()
         {
             //taskTimer.Dispose();
             //staskWorkThread.Abort();

         }

        #endregion 2. 构造方法


        #region 3. 私有方法

         // 启动任务
         public void Start() 
         {
             //taskTimer = new Timer(taskExecJob, taskWorkThreadParam, taskSchedule.DueTime, taskSchedule.Period);
             //taskWorkThreadRunFlag = true;

             BuckupTaskParameter parameter = (BuckupTaskParameter)taskWorkThreadParam;
             if (null != parameter)
             {
                 NVRControler nvrControler = parameter.ThreadPamamterNvrControler;
                 int nvrSelectIndex = parameter.NvrSelectIndex;
                 int channelSelectIndex = parameter.ChannelSelectIndex;
                 string nvrSelectName = nvrControler.NvrName;
                 string channelSelectName = nvrControler.
                     NvrChannels[channelSelectIndex].ChannelName;

                 taskWorkThreadRunFlag = true;
                 int backupVideoTimeLong = FileHelper.Min2Subtle(parameter.BackupTimeLong);
                 int backupRestartWaitTimeLong = FileHelper.Min2Subtle(parameter.RestartWaitTimeLong);
                 string backupDataSavePath = parameter.BackupDataSavePath;

                 //string backupSaveFileName = FileHelper.GetVideoFileName(nvrSelectIndex,channelSelectIndex, DateTime.Now);

                 TimeSpan startendTimeSpan = parameter.StEndTimeSpan;
                 DateTime startTime = parameter.StartTime;
                 DateTime endTime = parameter.EndTime;

                 string backupSaveFileName = TimeHelper.DateTimeToString(startTime) + ".avi";
                 string backupSaveFullPathFileName = backupDataSavePath + "/" + backupSaveFileName;

                 //while (taskWorkThreadRunFlag)
                 //{
                 //    try
                 //    {
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
                             checkBackupTaskTimer.Enabled = true;
                             checkBackupTaskTimer.Start();
                             autoEvent.WaitOne();
                         }

                          Console.WriteLine("结束下载");
                          checkBackupTaskTimer.Enabled = false;
                          checkBackupTaskTimer.Stop();
                          checkBackupTaskTimer.Dispose();
   
                          //if (mIsAllDay || (eveStartTime < (mStartTime + mStEndTimeSpan)))
                         //{
                         //    if (!mNvrControler.ChannelRealPlay(mSelectChannelIndex))
                         //    {
                         //        Console.WriteLine("预览失败");
                         //    }
                         //    if (mNvrControler.Record(mSelectChannelIndex, mSavePathFile))
                         //    {
                         //        Console.WriteLine("录像成功");
                         //        Thread.Sleep(mVideoRecordTimeLong);
                         //    }
                         //    else
                         //    {
                         //        Console.WriteLine("录像失败,休眠后重新开始录像");
                         //        Thread.Sleep(mVideoRestartWaitTimeLong);
                         //        continue;
                         //    }
                         //    mSavePath = parameter.VideoSavePath;
                         //    mSaveFileName = FileHelper.GetVideoFileName(mSelectNvrIndex,
                         //           mSelectChannelIndex, DateTime.Now);
                         //    mSavePathFile = mSavePath + "/" + mSaveFileName;
                         //    Log.WriteLog(LogType.Trace, "通道" + mSelectChannelIndex + "保存" + mSaveFileName + "完成");
                         //}
                         //else
                         //{
                         //    taskWorkThreadRunFlag = false;
                         //}
                 //    }
                 //    //异常情况:1.手动停止录像线程 2.SDK接口问题（包括底层网络断掉的原因）          
                 //    catch (Exception ex)
                 //    {
                 //        //Console.WriteLine("捕获异常");
                 //        //Log.WriteLog(LogType.Error, "通道" + mSelectChannelIndex + "录像线程异常停止");
                 //        //Log.WriteLog(LogType.Error, "异常情况如下" + ex.Message);
                 //        break;
                 //    }
                 //    finally
                 //    {
                 //        //if (!mNvrControler.RecordStop(mSelectChannelIndex))
                 //        //    Log.WriteLog(LogType.Error, "通道" + mSelectChannelIndex + "停止录像失败");
                 //    }
                 //}
             }
         }

         public void CheckTaskStatus(object sender, System.Timers.ElapsedEventArgs e)
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
             NVRChannel[] nvrChannels  =  nvrControler.NvrChannels;
             int nvrSelectIndex = parameter.NvrSelectIndex;
             int channelSelectIndex = parameter.ChannelSelectIndex;
             DateTime startTime = parameter.StartTime;
             DateTime endTime = parameter.EndTime;
             DateTime backupTime = parameter.EndTime;
             string backupSetConfigFile = parameter.BackupTaskSetConfigFile;
             string backupDataSavePath = parameter.BackupDataSavePath;

             int downLoadPos = nvrControler.NvrGetDownLoadPos();
             //Console.WriteLine("通道" + channelSelectIndex  + "下载进度" + downLoadPos);

             //下载完成后写入配置文件下一个备份时间
             if (downLoadPos == 100)
             {
                Console.WriteLine("已经完成下载了");
                if (nvrControler.NvrStopDownLoad())
                {
                    Console.WriteLine("停止下载,写入下一个需备份的时间点");
                    DataOperate.WritePrivateProfileString(nvrControler.NvrName, channelSelectIndex.ToString() + "backupTime",
                           TimeHelper.DateTimeToString(endTime), backupSetConfigFile);
                }

                string backupSaveFileName = TimeHelper.DateTimeToString(startTime) + ".avi";
                string backupSaveFullPathFileName = backupDataSavePath + "/" + backupSaveFileName;
            
                if (FileHelper.GetFileLength(backupSaveFullPathFileName) == 0)
                {
                    Console.WriteLine("删除的文件为" + backupSaveFullPathFileName);
                    FileHelper.DeleteFile(backupSaveFullPathFileName);
                }

                this.checkBackupTaskTimer.Enabled = false;
                this.checkBackupTaskTimer.Stop();
                 autoEvent.Set();
             }

            //下载过程中出现网络问题则在下次重新备份
             else if (downLoadPos == 200)
             {
                 Console.WriteLine("出现网络故障");
                 //if (nvrControler.NvrStopDownLoad())
                 this.checkBackupTaskTimer.Enabled = false;
                 this.checkBackupTaskTimer.Stop();
                 autoEvent.Set();
             }

              //下载出现错误
             else if (downLoadPos == -1)
             {
                 Console.WriteLine("下载失败，可能没有这个文件");
                 if (nvrControler.NvrStopDownLoad())
                     DataOperate.WritePrivateProfileString(nvrControler.NvrName, channelSelectIndex.ToString() + "backupTime",
                            TimeHelper.DateTimeToString(startTime+new TimeSpan(0,1,0)), backupSetConfigFile);

                 this.checkBackupTaskTimer.Enabled = false;
                 this.checkBackupTaskTimer.Stop();
                 autoEvent.Set();
             }

             lock (LockObject)
                 checkTaskStatusFlag = 0;

         }


         public void Execute(object param)
         {
             taskLastExecuteTime = DateTime.Now;

             if (taskSchedule.Period == Timeout.Infinite)
             {
                 taskNextExecuteTime = DateTime.MaxValue;
             }
             else
             {
                 TimeSpan period = new TimeSpan(taskSchedule.Period * 1000);
                 taskNextExecuteTime = taskLastExecuteTime + period;
             }

             WorkThreadFunc();

             //taskWorkThread = new Thread(new ThreadStart(WorkThreadFunc));
             //taskWorkThread.Start();
         }


         private void WorkThreadFunc()
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

                 DateTime eveStartTime;

                 taskWorkThreadRunFlag = true;
                 int backupVideoTimeLong = FileHelper.Min2Subtle(parameter.BackupTimeLong);
                 int backupRestartWaitTimeLong = FileHelper.Min2Subtle(parameter.RestartWaitTimeLong);
                 string backupDataSavePath = parameter.BackupDataSavePath;

                 //string backupSaveFileName = FileHelper.GetVideoFileName(nvrSelectIndex,channelSelectIndex, DateTime.Now);

                 TimeSpan startendTimeSpan = parameter.StEndTimeSpan;
                 DateTime startTime = parameter.StartTime;
                 DateTime endTime = parameter.EndTime;

                 string backupSaveFileName =  TimeHelper.DateTimeToString(startTime) + ".mp4";
                 string backupSaveFullPathFileName = backupDataSavePath + "/" + backupSaveFileName;

                 while (taskWorkThreadRunFlag)
                 {
                     eveStartTime = DateTime.Now;
                     try
                     {
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

                         nvrControler.NvrDownLoadFileByTime(channelSelectIndex, startYear, startMonth, startDay, startHour, startMin, startSecond,
                             endYear, endMonth, endDay, endHour, endMin, endSecond, backupSaveFullPathFileName);

                     }
                     //异常情况:1.手动停止录像线程 2.SDK接口问题（包括底层网络断掉的原因）          
                     catch (Exception ex)
                     {
                         //Console.WriteLine("捕获异常");
                         //Log.WriteLog(LogType.Error, "通道" + mSelectChannelIndex + "录像线程异常停止");
                         //Log.WriteLog(LogType.Error, "异常情况如下" + ex.Message);
                         break;
                     }
                     finally
                     {
                         //if (!mNvrControler.RecordStop(mSelectChannelIndex))
                         //    Log.WriteLog(LogType.Error, "通道" + mSelectChannelIndex + "停止录像失败");
                     }
                 }
             }
         }

        



        #endregion 3. 私有方法


        #region 4. 公有方法


        #endregion 4. 公有方法


    }
}
