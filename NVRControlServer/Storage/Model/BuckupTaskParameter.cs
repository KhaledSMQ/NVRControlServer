#region ************************文件说明************************************
/// 作者(Author)：                         黄顺彬
/// 
/// 日期(Create Date)：                2014.11.4
/// 
/// 功能：                                        存储NVR原始历史数据任务参数
///
/// 修改记录(Revision History)：       无
///
#endregion *****************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NVRControlServer.NVR.Control;

namespace NVRControlServer.Storage.Model
{
    public class BuckupTaskParameter
    {
        #region 1.变量属性

        #region 1.1变量
        private int nvrSelectIndex;//Nvr编号
        private string nvrSelectName;//Nvr名
        private int channelSelectIndex;//通道号
        private string channelSelectName;//通道名
        private NVRControler nvrSelectControler;//NVR控制器
        private int backupVideoTimeLong;//单次备份录像时长(分钟)
        private string backupDataSavePath;//保存备份数据目录 
        private TimeSpan stEndTimeSpan;//开始备份到结束备份的时间段
        private DateTime startTime;//开始备份时间
        private DateTime endTime;//结束备份时间
        private int restartWaitTimeLong;//获取失败后重新开始的时间长
        private string backupTaskSetConfigFile;
        #endregion 1.1变量

        #region 1.2属性
        public int NvrSelectIndex
        {
            get { return this.nvrSelectIndex; }
            set { this.nvrSelectIndex = value; }
        }

        public int ChannelSelectIndex
        {
            get { return this.channelSelectIndex; }
            set { this.channelSelectIndex = value; }
        }

        public NVRControler ThreadPamamterNvrControler
        {
            get { return this.nvrSelectControler; }
            set { this.nvrSelectControler = value; }
        }

        public int BackupTimeLong
        {
            get { return this.backupVideoTimeLong; }
            set { this.backupVideoTimeLong = value; }
        }

        public int RestartWaitTimeLong
        {
            get { return restartWaitTimeLong; }
            set { restartWaitTimeLong = value; }
        }

        public string BackupDataSavePath
        {
            get { return this.backupDataSavePath; }
            set { this.backupDataSavePath = value; }
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

        public string BackupTaskSetConfigFile
        {
            get { return backupTaskSetConfigFile; }
            set { backupTaskSetConfigFile = value; }
        }
        #endregion 1.2属性

        #endregion 1.变量属性

        #region 2. 构造方法
        public BuckupTaskParameter(int nvrId, int channelId, 
            int backupTimeLong, int restartWaitTimeLong, 
            NVRControler nvrControler,string backupDataSavePath, 
            TimeSpan taskSTEndTimeSpan,DateTime startTime,
            DateTime endTime, string backupTaskSetConfigFile)
        {
            this.nvrSelectIndex = nvrId;
            this.channelSelectIndex = channelId;
            this.backupVideoTimeLong = backupTimeLong;
            this.nvrSelectControler = nvrControler;
            this.backupDataSavePath = backupDataSavePath;
            this.stEndTimeSpan = taskSTEndTimeSpan;
            this.restartWaitTimeLong = restartWaitTimeLong;
            this.startTime = startTime;
            this.endTime = endTime;
            this.backupTaskSetConfigFile = backupTaskSetConfigFile;
        }
        #endregion 2. 构造方法

    }
}
