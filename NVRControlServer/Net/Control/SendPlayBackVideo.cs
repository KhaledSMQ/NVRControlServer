using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NVRControlServer.Net.Model;
using System.Threading;
using NVRControlServer.Net.Model.Event;
using System.IO;

namespace NVRControlServer.Net.Control
{
    public class SendPlayBackVideo
    {
        private ClientSession clientSession;
        private PlayBackVideoSendQueue videoQueue;
        private Thread workThread;
        private Dictionary<string, SendFileManager> sendFileManagerList;
        private object syncLock = new object();
        private AutoResetEvent autoEvent = new AutoResetEvent(false);

        public Dictionary<string, SendFileManager> SendFileManagerList
        {
            get
            {
                if (sendFileManagerList == null)
                {
                    sendFileManagerList = new Dictionary<string, SendFileManager>(10);
                }
                return sendFileManagerList;
            }
        }

        private void RemoveManager(string managerMd5)
        {
            SendFileManager sendFileManager;
            if (SendFileManagerList.TryGetValue(
                managerMd5,
                out sendFileManager))
            {

                lock (syncLock)
                {
                    SendFileManagerList.Remove(managerMd5);
                    sendFileManager.Dispose();
                    sendFileManager = null;
                }
            }
        }

        public void StartSend(ClientSession clientSession, SendFileManager sendFileManager)
        {
            SendFileManagerList.Add(sendFileManager.FileMd5, sendFileManager);

            sendFileManager.ReadFileBuffer +=
                new ReadFileBufferEventHandler(SendFileManageReadFileBuffer);
            sendFileManager.SendFileTimeout +=
                new EventHandler(SendFileManagerSendFileTimeout);

            TraFransfersFileStart ts = new TraFransfersFileStart(
                sendFileManager.FileMd5,
                sendFileManager.FileName,
                sendFileManager.FileImage,
                sendFileManager.FileLength,
                sendFileManager.PartCount,
                sendFileManager.PartSize);

            clientSession.Send(MSGKIND.PlayBackVideoStart,EXERESULT.ExcuteSuccess,Utils.Transform.Serialize(ts));
        }

        private void SendFileManageReadFileBuffer(
            object sender, ReadFileBufferEventArgs e)
        {
            SendFileManager sendFileManager = sender as SendFileManager;
            Console.WriteLine("读取文件块{0},大小为{1}", e.Index, e.Buffer.Length);

            if (sendFileManager.PartCount - 1 == e.Index)
            {
                Console.WriteLine("完成一个视频文件读取");

                if (File.Exists(sendFileManager.FileFullPathName))
                    File.Delete(sendFileManager.FileFullPathName);

                if (SendFileManagerList.TryGetValue(
                    sendFileManager.FileMd5, out sendFileManager))
                {
                    Console.WriteLine("结束这个文件发送控制");
                    lock (syncLock)
                    {
                        SendFileManagerList.Remove(sendFileManager.FileMd5);
                        sendFileManager.Dispose();
                        sendFileManager = null;
                    }
                }
                autoEvent.Set();
            }
            TraFransfersFile traFransfersFile =
                new TraFransfersFile(sendFileManager.FileMd5, e.Index, e.Buffer);
            clientSession.Send(MSGKIND.PlayBackVideoFilePack,
                EXERESULT.ExcuteSuccess, Utils.Transform.Serialize(traFransfersFile));
        }

        // 发送控制对象超时处理
        private void SendFileManagerSendFileTimeout(object sender, EventArgs e)
        {
            SendFileManager sendFileManager = sender as SendFileManager;
            if (SendFileManagerList.TryGetValue(
               sendFileManager.FileMd5,
               out sendFileManager))
            {
                lock (syncLock)
                {
                    SendFileManagerList.Remove(sendFileManager.FileMd5);
                    sendFileManager.Dispose();
                    sendFileManager = null;
                }
            }
        }


        public SendPlayBackVideo(ClientSession session, PlayBackVideoSendQueue queue)
        {
            this.clientSession = session;
            this.videoQueue = queue;
            workThread = new Thread(Work);
        }

        public void StartSend()
        {
            workThread.Start();
        }

        public void StopSend()
        {
            workThread.Join();
        }

        private void Work()
        {
            while (true)
            {
                //有可能取到队列空值
                if (videoQueue != null && videoQueue.Count() > 0)
                {
                    try
                    {
                        string fileFullPath = videoQueue.Dequeue();
                        if (fileFullPath != null)
                        {
                            Console.WriteLine("发送线程读到的文件名字" + fileFullPath);
                            int index = fileFullPath.LastIndexOf("\\");
                            string fielPath = fileFullPath.Substring(0, fileFullPath.LastIndexOf("\\") + 1);
                            string fileName = fileFullPath.Substring(fileFullPath.LastIndexOf("\\") + 1,
                                fileFullPath.LastIndexOf(".") - (fileFullPath.LastIndexOf("\\") + 1));
                            string fileExc = fileFullPath.Substring(fileFullPath.LastIndexOf(".") + 1,
                                fileFullPath.Length - fileFullPath.LastIndexOf(".") - 1);

                            SendFileManager sendFileManager = new SendFileManager(fielPath, fileName + "." + fileExc);
                            StartSend(clientSession, sendFileManager);
                            for (int i = 0; i < sendFileManager.PartCount; i++)
                            {
                                sendFileManager.Read(i);
                            }
                            autoEvent.WaitOne();
                        }
                    }
                    catch
                    {
                        continue;
                    }

                }
            }
        }

    }
}
