using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using NVRControlServer.Net.Interface;
using NVRControlServer.Net.Model;
using NVRControlServer.Net.Utils;
using NVRControlServer.DataBase.Modle;
using NVRControlServer.DataBase.SqlServerDAL;
using NVRControlServer.Net.Vo;

namespace NVRControlServer.Net.Control
{
    public class UpdateFileTagByNameHandle : ICommandHandle
    {

        private ClientSession clientSession;
        private ClientCommand clientCommand;
        private ICommandHandle netxCommandHandle;

        public UpdateFileTagByNameHandle()
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
            if (clientCommand.CommanKind == COMKIND.RequestUpdateFileTag)
            {
                string replayMsg = string.Empty;
                byte[] replayMsgByte = null;

                VideoInfoDataVo videoInfoDataVo =
                    (VideoInfoDataVo)Transform.Deserialiaze(clientCommand.AdditionMsg);

                VideoInfoData videoInfoData = new VideoInfoData();
                videoInfoData.nvrId = Transform.String2Int(videoInfoDataVo.NvrId);
                videoInfoData.channelId = Transform.String2Int(videoInfoDataVo.ChannelId);
                videoInfoData.fileName = videoInfoDataVo.FileName;
                videoInfoData.fileTag = videoInfoDataVo.FileTag;

                VideoInfoDAL videoInfoDal = new VideoInfoDAL();
                try
                {
                    if (videoInfoDal.UpdateVideoTagByName(videoInfoData))
                    {
                        replayMsg = "更新文件标签成功!";
                        replayMsgByte = (byte[])Transform.String2Byte(replayMsg);
                        clientSession.Send(MSGKIND.ResponseUpdateFileTag, EXERESULT.ExcuteSuccess, replayMsgByte);
                    }
                    else
                    {
                        replayMsg = "更新文件标签失败!";
                        replayMsgByte = (byte[])Transform.String2Byte(replayMsg);
                        clientSession.Send(MSGKIND.ResponseUpdateFileTag, EXERESULT.ExcuteFail, replayMsgByte);
                    }
                }
                catch
                {
                    replayMsg = "更新文件标签异常!";
                    replayMsgByte = (byte[])Transform.String2Byte(replayMsg);
                    clientSession.Send(MSGKIND.ResponseUpdateFileTag, EXERESULT.ExcuteFail, replayMsgByte);
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
