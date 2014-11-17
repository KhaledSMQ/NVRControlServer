using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVRControlServer.DataBase.Modle
{
    [Serializable]
    public class VideoInfoData
    {
        public int id { get; set; }
        public int nvrId { get; set; }
        public string nvrName { get; set; }
        public int channelId { get; set; }
        public string channelName { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public string fileName { get; set; }
        public string filePath { get; set; }
        public string fileTag { get; set; }
        public long fileSize { get; set; }
    }
}
