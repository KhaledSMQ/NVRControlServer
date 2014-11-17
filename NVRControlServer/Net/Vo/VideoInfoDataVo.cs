using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVRControlServer.Net.Vo
{
    [Serializable]
    public class VideoInfoDataVo
    {
        public string FileName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string NvrId { get; set; }
        public string NvrName { get; set; }
        public string ChannelId { get; set; }
        public string ChannelName { get; set; }
        public string FileTag { get; set; }
        public string FileSize { get; set; }

        public VideoInfoDataVo(string fileName, string startTime, string endTime, 
            string nvrId, string nvrName, string channelId, string channelName,
            string fileTag, string fileSize)
        {
            FileName = fileName;
            StartTime = startTime;
            EndTime = endTime;
            NvrId = nvrId;
            NvrName = nvrName;
            ChannelId = channelId;
            ChannelName = channelName;
            FileTag = fileTag;
            FileSize = fileSize;
        }
    }
}
