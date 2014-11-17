#region ************************文件说明************************************
/// 作者(Author)：                     ShunBin Huang
/// 
/// 日期(Create Date)：                2014.6.21
/// 
/// 功能：                             存储数据任务
///
/// 修改记录(Revision History)：       无
///
#endregion *****************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using NVRControlServer.NVR.Control;
using NVRControlServer.Storage.Utils;
using NVRControlServer.Utils.Log;



namespace NVRControlServer.Storage.Module
{

    #region 计划接口
    public interface ISchedule
    {
        DateTime ExecutionTime
        {
            get;
            set;
        }

        long DueTime
        {
            get;
        }

        long Period
        {
            get;
        }
    }
    #endregion 计划接口

    #region 立即执行任务计划类
    public class ImmediateExcution : ISchedule
    {
        public DateTime ExecutionTime
        {
            get
            {
                return DateTime.Now;
            }
            set
            {
                ;
            }
        }

        public long DueTime
        {
            get
            {
                return 0;
            }
        }

        public long Period
        {
            get
            {
                return Timeout.Infinite;
            }
        }
    }
    #endregion 立即执行任务计划类

    #region 计划在未来时间执行操作
    public class ScheduleExecutionOnce : ISchedule
    {
        private DateTime m_schedule;

        public ScheduleExecutionOnce(DateTime schedule)
        {
            this.m_schedule = schedule;
        }

        public DateTime ExecutionTime
        {
            get
            {
                return m_schedule;
            }
            set
            {
                m_schedule = value;
            }
        }

        public long DueTime
        {
            get
            {
                long ms = (m_schedule.Ticks - DateTime.Now.Ticks) / 10000;
                if (ms < 0) ms = 0;
                return ms;
            }
        }

        public long Period
        {
            get
            {
                return Timeout.Infinite;
            }
        }
    }
    #endregion 计划在未来时间执行操作

    #region 周期性的执行计划
    public class CycExecution : ISchedule
    {
        private DateTime m_shedule;
        private TimeSpan m_period;

        public DateTime ExecutionTime
        {
            get
            {
                return m_shedule;
            }
            set
            {
                m_shedule = value;
            }
        }

        public long Period
        {
            get
            {
                return m_period.Ticks / 10000;
            }
        }

        public CycExecution(DateTime shedule, TimeSpan period)
        {
            m_shedule = shedule;
            m_period = period;
        }

        public CycExecution(TimeSpan period)
        {
            m_shedule = DateTime.Now;
            m_period = period;
        }

        public long DueTime
        {
            get
            {
                long ms = (m_shedule.Ticks - DateTime.Now.Ticks) / 10000;
                if (ms < 0) ms = 0;
                return ms;
            }
        }
    }
    #endregion 周期性的执行计划

    public class StorageTask
    {
        #region 1.变量属性

        #region 1.1 变量
        private string taskId;                                                                //任务标识号        
        private string taskName;                                                              //任务名称
        private string taskDescription;                                                       //任务描述
        private Timer taskTimer;                                                              //任务定时器
        private ISchedule taskSchedule;                                                           //任务计划
        private DateTime taskNextExecuteTime;                                                     //计划中下一开始执行时间
        private DateTime taskNextExecuteEndTiem;                                                  //计划中下一结束执行时间
        private DateTime taskLastExecuteTime;                                                     //计划最后一次的开始执行时间
        private DateTime taskLastExecuteEndTime;                                                  //计划最后一次的结束执行时间
        private TimerCallback taskExecTask;                                                       //定时后执行任务函数
        private Thread taskWorkThread;                                                            //任务工作线程
        private object taskWorkThreadParam;                                                       //任务工作线程参数
        private bool taskWorkThreadRunFlag;                                                       //任务工作线程工作指示
        #endregion 1.1 变量

        #region 1.2 属性
        public string TaskID
        {
            get
            {
                return this.taskId;
            }
        }

        public object JobParam
        {
            set
            {
                taskWorkThreadParam = value;
            }
        }

        public string Name
        {
            get
            {
                return taskName;
            }
            set
            {
                taskName = value;
            }
        }

        public string Description
        {
            get
            {
                return taskDescription;
            }
            set
            {
                taskDescription = value;
            }
        }


        public DateTime NextExecuteTime
        {
            get
            {
                return taskNextExecuteTime;
            }
        }

        public ISchedule Shedule
        {
            get
            {
                return taskSchedule;
            }
        }

        public TimerCallback Job
        {
            get
            {
                return taskExecTask;
            }
            set
            {
                taskExecTask = value;
            }
        }

        public DateTime LastExecuteTime
        {
            get
            {
                return taskLastExecuteTime;
            }
        }
        #endregion 1.2 属性

        #endregion 1.变量属性

        #region 2.构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="shedule">为每个任务制定一个执行计划</param>
        public StorageTask(ISchedule shedule)
        {
            if (shedule == null)
            {
                throw (new ArgumentNullException("shedule"));
            }
            taskSchedule = shedule;

        }

        public StorageTask(ISchedule _shedule, string _taskId, string _taskName, string _description, object _param)
        {
            if (_shedule == null)
            {
                throw (new ArgumentNullException("shedule"));
            }
            taskSchedule = _shedule;
            this.taskId = _taskId;
            this.taskName = _taskName;
            this.taskDescription = _description;
            this.taskWorkThreadParam = _param;
            this.Job += new TimerCallback(this.Execute);

        }

        ~StorageTask()
        {
            taskTimer.Dispose();
            taskWorkThread.Abort();
            StorageTaskParameter parameter = (StorageTaskParameter)taskWorkThreadParam;
            NVRControler mNvrControler = parameter.ThreadPamamterNvrControler;
            int mSelectChannelIndex = parameter.SelectChannelIndex;
            mNvrControler.RecordStop(mSelectChannelIndex);
            mNvrControler.StopChannelRealPlay(mSelectChannelIndex);
        }
        #endregion 2.构造函数

        #region 3.公有方法
        // 启动任务
        public void Start()
        {
            taskTimer = new Timer(taskExecTask, taskWorkThreadParam, taskSchedule.DueTime, taskSchedule.Period);
            taskWorkThreadRunFlag = true;
        }

        // 停止任务
        public void Stop()
        {
            Console.WriteLine("停止线程工作");
            taskTimer.Dispose();
            taskWorkThreadRunFlag = false;
            if (taskWorkThread.IsAlive)
            {
                taskWorkThread.Abort();
            }
        }

        /// <summary>
        /// 执行任务内容
        /// </summary>
        /// <param name="param">任务函数参数</param>
        public virtual void Execute(object param)
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

            taskWorkThread = new Thread(new ThreadStart(WorkThreadFunc));
            taskWorkThread.Start();
        }

        private void WorkThreadFunc()
        {
            StorageTaskParameter parameter = (StorageTaskParameter)taskWorkThreadParam;
            if (null != parameter)
            {
                NVRControler mNvrControler = parameter.ThreadPamamterNvrControler;
                int mSelectNvrIndex = parameter.SelectNvrIndex;
                int mSelectChannelIndex = parameter.SelectChannelIndex;
                string mSelectNvrName = mNvrControler.NvrName;
                string mSelectChannelName = mNvrControler.
                    NvrChannels[mSelectChannelIndex].ChannelName;

                int mVideoRecordTimeLong;
                int mVideoRestartWaitTimeLong;
                string mSavePath;
                string mSaveFileName;
                string mSavePathFile;
                bool mIsAllDay;
                TimeSpan mStEndTimeSpan;
                DateTime mStartTime;
                DateTime eveStartTime;
               
                taskWorkThreadRunFlag = true;
                mVideoRecordTimeLong = FileHelper.Min2Subtle(parameter.RecordTimeLong);
                mVideoRestartWaitTimeLong = FileHelper.Min2Subtle(parameter.RestartWaitTimeLong);
                mSavePath = parameter.VideoSavePath;
                mSaveFileName = FileHelper.GetVideoFileName(mSelectNvrIndex,
                                                        mSelectChannelIndex, DateTime.Now);
                mSavePathFile = mSavePath + "/" + mSaveFileName;
                mIsAllDay = parameter.IsAllDay;
                mStEndTimeSpan = parameter.StEndTimeSpan;
                mStartTime = DateTime.Now;

                while (taskWorkThreadRunFlag)
                {
                    eveStartTime = DateTime.Now;
                    try
                    {
                        if (mIsAllDay || (eveStartTime < (mStartTime + mStEndTimeSpan)))
                        {
                            if (!mNvrControler.ChannelRealPlay(mSelectChannelIndex))
                            {
                                Console.WriteLine("预览失败");
                            }
                            if (mNvrControler.Record(mSelectChannelIndex, mSavePathFile))
                            {
                                Console.WriteLine("录像成功");
                                Thread.Sleep(mVideoRecordTimeLong);
                            }
                            else
                            {
                                Console.WriteLine("录像失败,休眠后重新开始录像");
                                Thread.Sleep(mVideoRestartWaitTimeLong);
                                continue;
                            }
                            mSavePath = parameter.VideoSavePath;
                            mSaveFileName = FileHelper.GetVideoFileName(mSelectNvrIndex,
                                   mSelectChannelIndex, DateTime.Now);
                            mSavePathFile = mSavePath + "/" + mSaveFileName;
                            Log.WriteLog(LogType.Trace, "通道" + mSelectChannelIndex + "保存" + mSaveFileName + "完成");
                        }
                        else
                        {
                            taskWorkThreadRunFlag = false;
                        }
                    }
                    //异常情况:1.手动停止录像线程 2.SDK接口问题（包括底层网络断掉的原因）          
                    catch (Exception ex)
                    {
                        Console.WriteLine("捕获异常");
                        Log.WriteLog(LogType.Error, "通道" + mSelectChannelIndex + "录像线程异常停止");
                        Log.WriteLog(LogType.Error, "异常情况如下" + ex.Message);
                        break;
                    }
                    finally
                    {
                        if (!mNvrControler.RecordStop(mSelectChannelIndex))
                            Log.WriteLog(LogType.Error, "通道" + mSelectChannelIndex + "停止录像失败");
                    }
                }
            }
        }

        public override int GetHashCode()
        {
            return 1;
        }

        public override bool Equals(Object obj)
        {
            if (this == obj)
                return true;
            if (obj == null)
                return false;

            StorageTask other = obj as StorageTask;
            if (!this.taskWorkThreadParam.Equals(other.taskWorkThreadParam)
                && !this.taskSchedule.Equals(other.taskSchedule))
                return false;

            return true;
        }

        #endregion 3.公有方法

    }
}
