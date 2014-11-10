using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using NVRControlServer.Net.Model;
using System.Threading;
using NVRControlServer.Net.Utils;
using NVRControlServer.Net.Model;
using NVRControlServer.Net.Model.Event;
using System.Drawing;
using NVRControlServer.Net.Model.NetException;

namespace NVRControlServer.Net.Control
{
    public class TcpSendFile : IDisposable
    {
        private TcpPort tcpPort;
        private String fileMd5;
        private Dictionary<string, SendFileManager> sendFileManagerList;
        private object syncLock = new object();

        public event FileSendEventHandler FileSendAccept;
        public event FileSendEventHandler FileSendRefuse;
        public event FileSendEventHandler FileSendCancel;
        public event FileSendBufferEventHandler FileSendBuffer;
        public event FileSendEventHandler FileSendComplete;


        public TcpSendFile()
        {

        }

        public TcpSendFile(TcpPort tcpPort, string fileMd5)
        {
            this.tcpPort = tcpPort;
            this.fileMd5 = fileMd5;
        }

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

        public bool CanSend(SendFileManager sendFileManager)
        {
            return !SendFileManagerList.ContainsKey(sendFileManager.FileMd5);
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

                clientSession.Send(Msgkind.ResponseSendFile,
                                               ExecuteResult.ExcuteSuccess,
                                              Utils.Transform.Serialize(ts));
                
        }


        public void CancelSend(ClientSession clientSession,string md5)
        {
            SendFileManager sendFileManager;
            if (SendFileManagerList.TryGetValue(
                md5,
                out sendFileManager))
            {
                lock (syncLock)
                {
                    SendFileManagerList.Remove(md5);
                    sendFileManager.Dispose();
                    sendFileManager = null;
                }
                clientSession.Send(Msgkind.ResponseCancelSendFile,
                                                ExecuteResult.ExcuteSuccess,
                                                Utils.Transform.string2Byte(md5));
            }
        }


        private void RequestCancelReceiveFile(ClientSession clientSession,string md5)
        {
            SendFileManager sendFileManager;
            if (SendFileManagerList.TryGetValue(
                md5,
                out sendFileManager))
            {
                OnFileSendCancel(
                    new FileSendEventArgs(sendFileManager));
                lock (syncLock)
                {
                    SendFileManagerList.Remove(md5);
                    sendFileManager.Dispose();
                    sendFileManager = null;
                }
                clientSession.Send(Msgkind.ResponseCancelReceiveFile,
                                            ExecuteResult.ExcuteSuccess,
                                            Transform.string2Byte(string.Format("已取消发送文件{0}", fileMd5)));
            }
        }



        public void Send(ClientSession  clientSession,string fileMd5)
        {
            SendFileManager sendFileManager;
            if (SendFileManagerList.TryGetValue(
                fileMd5, out sendFileManager))
            {
                try
                {
                    for (int i = 0; i < sendFileManager.PartCount; i++)
                    {
                        if (SendFileManagerList.ContainsKey(fileMd5))
                        {
                            byte[] fileData = sendFileManager.Read(i);
                            TraFransfersFile traFransfersFile = new TraFransfersFile(sendFileManager.FileMd5, i, fileData);
                            byte[] filePackBytes = Transform.Serialize(traFransfersFile);
                            clientSession.Send(Msgkind.ResponseSendFilePack, ExecuteResult.ExcuteSuccess, filePackBytes);
                        }
                        else
                            break;
                    }
                }
                catch (Exception ex)
                {
                    
                }
            }
        }

        private void SendFileManageReadFileBuffer(
             object sender, ReadFileBufferEventArgs e)
        {
            SendFileManager sendFileManager = sender as SendFileManager;
            Console.WriteLine("读取文件块{0},大小为{1}", e.Index, e.Buffer.Length);
            if (sendFileManager.PartCount-1 == e.Index)
            {
                Console.WriteLine("完成文件块读取");
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
            }
        }

        /// <summary>
        /// 发送控制对象超时处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendFileManagerSendFileTimeout(object sender ,EventArgs e)
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


        public byte[] ReadFilePartData(string fileMd5, int partIndex)
        {
              SendFileManager sendFileManager;
              if (SendFileManagerList.TryGetValue(
                  fileMd5, out sendFileManager))
              {
                  byte[] fileData = sendFileManager.Read(partIndex);
                  if (fileData != null)
                      return fileData;
                  else
                      return null;
              }
              else
              {
                  return null;
              }
        }



        protected virtual void OnFileSendAccept(FileSendEventArgs e)
        {
            if (FileSendAccept != null)
            {
                FileSendAccept(this, e);
            }
        }

        protected virtual void OnFileSendRefuse(FileSendEventArgs e)
        {
            if (FileSendRefuse != null)
            {
                FileSendRefuse(this, e);
            }
        }

        protected virtual void OnFileSendCancel(FileSendEventArgs e)
        {
            if (FileSendCancel != null)
            {
                FileSendCancel(this, e);
            }
        }

        protected virtual void OnFileSendComplete(FileSendEventArgs e)
        {
            if (FileSendComplete != null)
            {
                FileSendComplete(this, e);
            }
        }

        protected virtual void OnFileSendBuffer(FileSendBufferEventArgs e)
        {
            if (FileSendBuffer != null)
            {
                FileSendBuffer(this, e);
            }
        }

        public void Dispose()
        {
            if (sendFileManagerList != null &&
                sendFileManagerList.Count > 0)
            {
                foreach (SendFileManager sendFileManager
                                    in sendFileManagerList.Values)
                {
                    sendFileManager.Dispose();
                }
                sendFileManagerList.Clear();
            }
        }

    }
}
