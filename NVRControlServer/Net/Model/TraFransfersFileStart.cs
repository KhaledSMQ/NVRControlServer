using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace NVRControlServer.Net.Model
{
    //开始传输文件的第一个，文件信息
    [Serializable]
    public class TraFransfersFileStart
    {
        private string md5;
        private string fileName;
        private Image fileImage;
        private long fileLength;
        private long partCount;
        private int partSize;


        public string Md5
        {
            get { return md5; }
            set { md5 = value; }
        }

        public long FileLength
        {
            get { return fileLength; }
            set { fileLength = value; }
        }

        public Image FileImage
        {
            get { return fileImage; }
            set { fileImage = value; }
        }

        public long PartCount
        {
            get { return partCount; }
            set { partCount = value; }
        }

        public int PartSize
        {
            get { return partSize; }
            set { partSize = value; }
        }

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        public TraFransfersFileStart(string md5,string fileName,Image fileImage,long fileLength, long partCount, int partSize)
        {
            this.md5 = md5;
            this.fileName = fileName;
            this.fileImage = fileImage;
            this.fileLength = fileLength;
            this.partCount = partCount;
            this.partSize = partSize;
        }



    }
}
