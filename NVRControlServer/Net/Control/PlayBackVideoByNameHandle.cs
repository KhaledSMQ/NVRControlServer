using System;
using System.Collections.Generic;
using System.Threading;


using NVRControlServer.Net.Interface;
using NVRControlServer.Net.Model;
using NVRControlServer.Net.Vo;
using NVRControlServer.Net.Utils;
using NVRControlServer.DataBase.Modle;
using NVRControlServer.DataBase.SqlServerDAL;
using System.Diagnostics;
using System.IO;

namespace NVRControlServer.Net.Control
{
    public class PlayBackVideoByNameHandle : ICommandHandle
    {
        private ClientSession clientSession;
        private ClientCommand clientCommand;
        private ICommandHandle netxCommandHandle;
        private PlayBackVideoSendQueue videoQueue;
        private SendPlayBackVideo sendBackVideo;
        private Thread splitVideo;
        private AutoResetEvent autoEvent;
        private int cellVideoLength = 5;

        public PlayBackVideoByNameHandle()
        {
            autoEvent = new AutoResetEvent(false);
        }

        public void SetHandleModel(ClientSession session, ClientCommand command)
        {
            this.clientSession = session;
            this.clientCommand = command;
        }

        public void SetNextHandler(ICommandHandle nextHandle)
        {
            this.netxCommandHandle = nextHandle;
        }

        public void Process()
        {
            if (clientCommand.CommanKind == COMKIND.RequestPlayBackVideoByName)
            {
                string replayMsg = string.Empty;
                byte[] replayMsgByte = null;
                VideoInfoDataVo videoInfoDataVo = 
                    (VideoInfoDataVo)Transform.Deserialiaze(clientCommand.AdditionMsg);
                VideoInfoData videoInfoData = new VideoInfoData();
                videoInfoData.nvrId = Transform.String2Int(videoInfoDataVo.NvrId);
                videoInfoData.channelId = Transform.String2Int(videoInfoDataVo.ChannelId);
                videoInfoData.fileName = videoInfoDataVo.FileName;
                VideoInfoDAL videoInfoDal = new VideoInfoDAL();
                try
                {
                    List<VideoInfoData> videoList =
                        (List<VideoInfoData>)videoInfoDal.GetSingleVideoByCond(videoInfoData);
                    if(videoList.Count >0)
                    {
                        VideoInfoData videoFile = videoList[0];
                        string videoName = videoFile.fileName;
                        string videoFullPath = videoFile.filePath + videoFile.fileName;
                        string videoCache = clientSession.PlayBackVideoCache;
                        if (videoFullPath != null)
                        {
                            videoQueue = new PlayBackVideoSendQueue();
                            //开始文件发送线程
                            sendBackVideo =new SendPlayBackVideo(clientSession, videoQueue);
                            sendBackVideo.StartSend();
                            //进行文件切分线程
                            object[] objs = new object[] { videoName, videoFullPath, videoQueue, videoCache };
                             splitVideo = new Thread(SpliteVideo);
                             splitVideo.Start(objs);
                        }
                    }
                    else
                    {
                        replayMsg = "未找到指定文件!";
                        replayMsgByte = (byte[])Transform.String2Byte(replayMsg);
                        clientSession.Send(MSGKIND.ResponsePlayBackVideoByName, EXERESULT.ExcuteFail, replayMsgByte);
                    }
                }
                catch
                {
                    replayMsg = "查找文件异常!";
                    replayMsgByte = (byte[])Transform.String2Byte(replayMsg);
                    clientSession.Send(MSGKIND.ResponsePlayBackVideoByName, EXERESULT.ExcuteFail, replayMsgByte);
                }
            }
            else if (clientCommand.CommanKind == COMKIND.RequestStopPlayBackVideo)
            {



            }
            else
            {
                if (netxCommandHandle != null)
                {
                    netxCommandHandle.Process();
                }
            }
        }

        private void SpliteVideo(object o)
        {
            object[] objs = o as object[];
            string videoFileName = objs[0] as string;
            string videoFullPath = objs[1] as string;
            PlayBackVideoSendQueue videoQueue
                   = objs[2] as PlayBackVideoSendQueue;
            string videoCache = objs[3] as string;

            if (videoFullPath != null && File.Exists(videoFullPath))
            {
                string args = string.Empty;
                long splitCellNum = 0;
                string soruceVideo = videoFullPath;

                TimeSpan startTime = new TimeSpan(0,0,0); //开始切分的时间
                TimeSpan  videoLength = new TimeSpan(0,0,this.cellVideoLength); //每段切分长度
                TimeSpan allTime = new TimeSpan(0,15,0);

                while(startTime < allTime)
                {
                    string targetVideo = GetSplitFileName(videoFileName, splitCellNum++);
                    string targetVideoFullPath = videoCache + @"\" + targetVideo;
                    args = "-ss ";
                    args += startTime.ToString() + " ";
                    args += "-i ";
                    args += soruceVideo + " ";
                    args += "-vcodec ";
                    args += "copy ";
                    args += "-acodec ";
                    args += "copy ";
                    args += "-t ";
                    args += videoLength.ToString() + " ";
                    args += targetVideoFullPath;
                    //Excute("ffmpeg.exe", args, (s, ee) => Console.Write(ee.Data));
                    Excute("ffmpeg.exe", args, null);
                    autoEvent.WaitOne();
                    videoQueue.Enqueue(targetVideoFullPath);
                    startTime += videoLength;
                }
            }
        }

        private void Excute(string exe, string arg, DataReceivedEventHandler output)
        {
            using (var p = new Process())
            {
                p.StartInfo.FileName = exe;
                p.StartInfo.Arguments = arg;
                p.StartInfo.UseShellExecute = false;    //输出信息重定向
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.OutputDataReceived += output;
                p.ErrorDataReceived += output;
                p.Start();                    //启动线程
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
                p.WaitForExit(1000);     //如果先执行完则退出
                //  p.WaitForExit();            //等待进程结束
                autoEvent.Set();
                //Console.WriteLine("这条进程结束了");
            }
        }

        private string GetSplitFileName(string sourceFile, long num)
        {
            int index = sourceFile.IndexOf(".avi");
            string newString = sourceFile.Insert(index, "cell" + num.ToString());
            return newString;
        }

    }
}
