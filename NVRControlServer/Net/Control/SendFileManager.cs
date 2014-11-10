using System;
using System.IO;

using NVRControlServer.Net.Model.Event;
using NVRControlServer.Net.Utils;
using System.Drawing;
using System.Threading;
using NVRControlServer.Net.Model.NetException;

namespace NVRControlServer.Net.Control
{
    //发送文件
    public class SendFileManager : IDisposable
    {
        private FileStream fileStream;
        private long partCount;
        private long fileLength;
        private Image fileImage;
        private int partSize = 1024 * 30; //1M的速度读取文件
        private string filePath;
        private string fileName;
        private string fileFullPathName;
        private DateTime lastSendTime;
        private Timer sendTimer;
        private int interval = 5000;
        private static readonly int SendTimeout = 5000;

        private string fileMd5;

        public event ReadFileBufferEventHandler ReadFileBuffer;
        public event EventHandler SendFileTimeout;

        public FileStream FileStream
        {
            get { return fileStream; }
            set { fileStream = value; }
        }

        public long PartCount
        {
            get { return partCount; }
            set { partCount = value; }
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

        public string FileMd5
        {
            get { return fileMd5; }
            set { fileMd5 = value; }
        }

        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        public string FileFullPathName
        {
            get { return fileFullPathName; }
            set { fileFullPathName = value; }
        }

        public Timer SendTimer
        {
            get
            {
                if (sendTimer == null)
                {
                    sendTimer = new Timer(
                                                    new TimerCallback(delegate(object obj)
                                                    {
                                                        TimeSpan ts = DateTime.Now - lastSendTime;
                                                        if (ts.TotalMilliseconds > SendTimeout)
                                                        {
                                                            lastSendTime = DateTime.Now;
                                                            OnSendFileTimeout(EventArgs.Empty);
                                                        }
                                                    }),
                                                    null,
                                                    Timeout.Infinite,
                                                    interval);
                }
                return sendTimer;
            }
            set { sendTimer = value; }
        }



        public SendFileManager(string filePath , string fileName)
        {
            this.fileName = fileName;
            this.filePath = filePath;
            this.fileFullPathName = filePath + fileName;
            Create(fileFullPathName);
        }


        private void Create(string fileName)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(fileName);
                fileImage = Icon.ExtractAssociatedIcon(fileName).ToBitmap();
                fileStream = fileInfo.OpenRead();
                fileLength = fileStream.Length;
                partCount = fileLength / partSize;
                if (fileLength / partSize != 0)
                {
                    partCount++;
                }
                fileMd5 = MD5Helper.CreateMD5(fileName);
            }
            catch (Exception ex)
            {
                throw new NetTcpSendFileException
                    (NetTcpSendFileErrono.CreateSendFileManagerFail,
                      "创建发送文件控制失败");
            }
        }


        public byte[] Read(int index)
        {
            try
            {
                int size = partSize;
                if (FileLength - partSize * index < partSize)
                {
                    size = (int)(FileLength - partSize * index);
                }
                byte[] readBuffer = new byte[size];
                fileStream.Position = index * partSize;
                int readLength = fileStream.Read(readBuffer, 0, readBuffer.Length);
                ReadFileBufferEventArgs e = null;
                if (readLength < partSize)
                {
                    byte[] realBuffer = new byte[readLength];
                    Buffer.BlockCopy(readBuffer, 0, realBuffer, 0, readLength);
                    e = new ReadFileBufferEventArgs(index, realBuffer);
                    OnReadFileBuffer(e);
                    return realBuffer;
                }
                else
                {
                    e = new ReadFileBufferEventArgs(index, readBuffer);
                    OnReadFileBuffer(e);
                    return readBuffer;
                }
            }
            catch (Exception ex)
            {
                throw new NetTcpSendFileException(NetTcpSendFileErrono.ReadFileFail, "读文件块出错");
            }
        }

        public virtual void OnReadFileBuffer(ReadFileBufferEventArgs e)
        {
            if (ReadFileBuffer != null)
            {
                ReadFileBuffer(this, e);
            }
        }

        protected virtual void OnSendFileTimeout(EventArgs e)
        {
            if (SendFileTimeout != null)
            {
                SendFileTimeout(this, e);
            }
        }

        public void Dispose()
        {
            if (fileStream != null)
            {
                fileStream.Close();
            }
        }

    }
}
