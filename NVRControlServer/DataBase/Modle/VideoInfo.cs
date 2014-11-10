using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVRControlServer.DataBase.Module
{
    public class VideoInfo
    {
        #region 1. 变量属性

        #region 1.1 变量
        private int nvrId;
        private string nvrName;
        private int channelId;
        private string channelName;
        private DateTime startTime;
        private DateTime endTime;
        private string videoFileName;
        private string videoFilePath;
        #endregion 1.1 变量

        #region 1.2 属性

        public int NvrId
        {
            get { return nvrId; }
            set { nvrId = value; }
        }
        
        public string NvrName
        {
            get { return nvrName; }
            set { nvrName = value; }
        }
       
        public int ChannelId
        {
            get { return channelId; }
            set { channelId = value; }
        }

        public string ChannelName
        {
            get { return channelName; }
            set { channelName = value; }
        }

        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        public DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        public string VideoFileName
        {
            get { return videoFileName; }
            set { videoFileName = value; }
        }

        public string VideoFilePath
        {
            get { return videoFilePath; }
            set { videoFilePath = value; }
        }
        #endregion 1.2属性

        #endregion 1. 变量属性

        #region 2.构造方法

        #region 2.1 有参构造
        public VideoInfo(int nvrId, string nvrName, int channelId, string channelName,
            DateTime startTime, DateTime endTime, string fileName, string filePath)
        {
            this.nvrId = nvrId;
            this.nvrName = nvrName;
            this.channelId = channelId;
            this.channelName = channelName;
            this.startTime = startTime;
            this.endTime = endTime;
            this.videoFileName = fileName;
            this.videoFilePath = filePath;
        }
        #endregion 2.1 有参构造

        #region 2.2 无参构造
        #endregion 2.2 无参构造

        #endregion 2. 构造方法

    }
}
