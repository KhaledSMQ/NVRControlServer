#region ************************文件说明************************************
/// 作者(Author)：                     ShunBin Huang
/// 
/// 日期(Create Date)：                2014.6.15
/// 
/// 功能：                             存储任务属性参数 
///
/// 修改记录(Revision History)：无
///
#endregion *****************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using NVRControlServer.NVR.Control;

namespace NVRControlServer.Storage.Module
{
    public class StorageTaskParameter
    {
        #region 1.变量属性

        #region 1.1变量
        private int selectNvrId;                                                                    //Nvr编号
        private string selectNvrName;                                                       //Nvr名
        private int selectChannelIndex;                                                     //通道号
        private string selectChannelName;                                               //通道名
        private NVRControler threadPamamterNvrControler;              //NVR控制器
        private int recordVideoTimeLong;                                                //单次录像时长(分钟)
        private string videoSavePath;                                                         //保存目录 
        private TimeSpan stEndTimeSpan;                                                //开始录像到结束录像的时间段
        private DateTime startTime;                                                           //开始录像时间
        private DateTime endTime;                                                           //结束录像时间
        private int restartWaitTimeLong;                                                  //录像失败后重新开始的时间长
        private bool isAllDay = false;                                                        //全天候录像
        #endregion 1.1变量

        #region 1.2属性
        public int SelectNvrIndex
        {
            get { return this.selectNvrId; }
            set { this.selectNvrId = value; }
        }

        public int SelectChannelIndex
        {
            get { return this.selectChannelIndex; }
            set { this.selectChannelIndex = value; }
        }

        public int RecordTimeLong
        {
            get { return this.recordVideoTimeLong; }
            set { this.recordVideoTimeLong = value; }
        }

        public int RestartWaitTimeLong
        {
            get { return restartWaitTimeLong; }
            set { restartWaitTimeLong = value; }
        }

        public NVRControler ThreadPamamterNvrControler
        {
            get { return this.threadPamamterNvrControler; }
            set { this.threadPamamterNvrControler = value; }
        }

        public string VideoSavePath
        {
            get { return this.videoSavePath; }
            set { this.videoSavePath = value; }
        }

        public TimeSpan StEndTimeSpan
        {
            get { return this.stEndTimeSpan; }
            set { this.stEndTimeSpan = value; }
        }

        public DateTime StartTime
        {
            get { return this.startTime; }
            set { this.startTime = value; }
        }

        public DateTime EndTime
        {
            get { return this.endTime; }
            set { this.endTime = value; }
        }

        public bool IsAllDay
        {
            get { return this.isAllDay; }
            set { this.isAllDay = value; }
        }
        #endregion 1.2属性

        #endregion 1.变量属性

        #region 2.构造函数
        public StorageTaskParameter(int nvrId, int channelId, int recordTimeLong, int restartWaitTimeLong, NVRControler nvrControler,
            string channelVideoSavePath, TimeSpan taskSTEndTimeSpan,  bool taskIsAllDay)
        {
            this.selectNvrId = nvrId;
            this.selectChannelIndex = channelId;
            this.recordVideoTimeLong = recordTimeLong;
            this.threadPamamterNvrControler = nvrControler;
            this.videoSavePath = channelVideoSavePath;
            this.stEndTimeSpan = taskSTEndTimeSpan;
            this.restartWaitTimeLong = restartWaitTimeLong;
            this.isAllDay = taskIsAllDay;
        }

        #endregion 2.构造函数
    }
}
