using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NVRControlServer.Net.Model;
using NVRControlServer.Net.Interface;
using NVRControlServer.Net.Utils;
using NVRControlServer.DataBase.Modle;
using NVRControlServer.DataBase.SqlServerDAL;
using NVRControlServer.Net.Vo;

namespace NVRControlServer.Net.Control
{
    public class SearchFileByTimeHandle : ICommandHandle
    {
        private ClientSession clientSession;
        private ClientCommand clientCommand;
        private ICommandHandle netxCommandHandle;

        public SearchFileByTimeHandle()
        {

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
            if (clientCommand.CommanKind == COMKIND.RequestSearchFileByTime)
            {

                string replayMsg = string.Empty;
                byte[] replayMsgByte = null;

                string msgBuffer = Transform.Byte2String(clientCommand.AdditionMsg);
                string[] nMsgBuffer = Transform.GetStrings(msgBuffer, ',');

                int searchNvrId = (int)Transform.String2Int(nMsgBuffer[0]);
                int searchChannelId = (int)Transform.String2Int(nMsgBuffer[1]);
                DateTime searchStartTime = (DateTime)Transform.String2DateTime(nMsgBuffer[2]);
                DateTime searchEndTime = (DateTime)Transform.String2DateTime(nMsgBuffer[3]);

                VideoInfoData videoInfoData = new VideoInfoData();
                videoInfoData.nvrId = searchNvrId;
                videoInfoData.channelId = searchChannelId;
                videoInfoData.startTime = searchStartTime;
                videoInfoData.endTime = searchEndTime;

                VideoInfoDAL videoInfoDal = new VideoInfoDAL();

                try
                {
                    List<VideoInfoData> videoInfoList =
                        (List<VideoInfoData>)videoInfoDal.GetVideoByTime(videoInfoData);

                    if (videoInfoList.Count != 0)
                    {
                        for (int i = 0; i < videoInfoList.Count; i++)
                        {
                            videoInfoData = videoInfoList[i];

                            string fileName = videoInfoData.fileName;
                            string startTime = Transform.DateTime2String(videoInfoData.startTime);
                            string endTime = Transform.DateTime2String(videoInfoData.endTime);
                            string nvrId = videoInfoData.nvrId.ToString();
                            string nvrName = videoInfoData.nvrName;
                            string channelId = videoInfoData.channelId.ToString();
                            string channelName = videoInfoData.channelName;
                            string fileTag = videoInfoData.fileTag;
                            string fileSize = videoInfoData.fileSize.ToString();

                            VideoInfoDataVo videoInfoDataVo =
                                new VideoInfoDataVo(fileName, startTime, endTime, 
                                    nvrId, nvrName, channelId, 
                                    channelName,fileTag, fileSize);

                            replayMsgByte = (byte[])Transform.Serialize(videoInfoDataVo);
                            clientSession.Send(MSGKIND.ResponseSearchFileByTime, EXERESULT.ExcuteSuccess, replayMsgByte);
                        }
                    }
                    else
                    {
                        replayMsg = "搜索指定文件失败!";
                        replayMsgByte = (byte[])Transform.String2Byte(replayMsg);
                        clientSession.Send(MSGKIND.ResponseSearchFileByTime, EXERESULT.ExcuteFail, replayMsgByte);
                    }

                }
                catch
                {
                    replayMsg = "搜索指定文件发生异常情况!";
                    replayMsgByte = (byte[])Transform.String2Byte(replayMsg);
                    clientSession.Send(MSGKIND.ResponseSearchFileByTime, EXERESULT.ExcuteFail, replayMsgByte);
                }
            }
            else
            {
                if (netxCommandHandle != null)
                {
                    netxCommandHandle.Process();
                }
            }
        }
    }
}
