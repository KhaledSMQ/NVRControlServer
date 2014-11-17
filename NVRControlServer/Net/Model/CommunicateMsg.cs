using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NVRControlServer.Net.Utils;
using NVRControlServer.Net.Interface;


//客户端发送tcp命令消息格式
//**********************************************
// |------------------------------------------
// |com_kind(1) | identify(1) | addtion_msg |
// |------------------------------------------
//**********************************************

//服务器回复tcp消息格式
//***********************************************************
// |---------------------------------------------------------
// |msg_kind(1)  | exec_result(1) |  addtion_msg |
// |---------------------------------------------------------
//***********************************************************


namespace NVRControlServer.Net.Model
{
    #region 传输信息枚举类型
    public enum MSGKIND : int
    {
        ResponseLogin = 0,
        ResponseLogout,
        ResponseGetNvrChannelInfo,
        ResponseSearchFileByTime,
        ResponseSendFile,
        ResponseSendFilePack,
        ResponseCancelSendFile,
        ResponseCancelReceiveFile,
        ResponseUpdateFileTag,
        ResponsePlayBackVideoByName,
        PlayBackVideoStart,
        PlayBackVideoFilePack,
        ResponseStopPlayBackVideo
    };


    public enum EXERESULT : int
    {
        ExcuteSuccess = 0,
        ExcuteFail,
    };

    public enum COMKIND : int
    {
        RequestLogin = 0,
        RequestLogout,
        RequestGetNvrChannelInfo,
        RequestSearchFileByTime,
        RequestSendFile,
        RequestSendFilePack,
        RequestCancelSendFile,
        RequestCancelReceiveFile,
        RequestUpdateFileTag,
        RequestPlayBackVideoByName,
        RequestStopPlayBackVideo
    };

    public enum IDENTIFY : int
    {
        LocalClient = 0,
        RemoteClient
    };
    #endregion 传输信息枚举类型



    public class ClientCommand : IDataFrame
    {
        public COMKIND CommanKind { get; set; }
        public IDENTIFY RightIdentify { get; set; }
        public byte[] AdditionMsg { get; set; }

        public ClientCommand()
        {


        }

        public ClientCommand(COMKIND comkind, IDENTIFY identify, byte[] data)
        {
            CommanKind = comkind;
            RightIdentify = identify;
            AdditionMsg = data;
        }


        public byte[] ToBuffer()
        {
            byte[] buffer;
            byte[] temp1 = Transform.MinInt2Bytes((int)CommanKind);
            byte[] temp2 = Transform.MinInt2Bytes((int)RightIdentify);

            if (AdditionMsg != null)
            {
                buffer = new byte[AdditionMsg.Length + 4];
                byte[] temp3 = Transform.Int2Bytes(AdditionMsg.Length + 2);
                Array.Copy(temp3, 0, buffer, 0, temp3.Length);
                Array.Copy(temp1, 0, buffer, temp3.Length, temp1.Length);
                Array.Copy(temp2, 0, buffer, temp3.Length + temp1.Length, temp2.Length);
                Array.Copy(AdditionMsg, 0, buffer, temp1.Length + temp2.Length + temp3.Length, AdditionMsg.Length);
            }
            else
            {
                buffer = new byte[4];
                byte[] temp3 = Transform.Int2Bytes(2);
                Array.Copy(temp3, 0, buffer, 0, temp3.Length);
                Array.Copy(temp1, 0, buffer, temp3.Length, temp1.Length);
                Array.Copy(temp2, 0, buffer, temp3.Length + temp1.Length, temp2.Length);
            }
            return buffer;
        }

        public void FromBuffer(byte[] buffer)
        {
            CommanKind = (COMKIND)((int)buffer[0]);
            RightIdentify = (IDENTIFY)((int)buffer[1]);
            byte[] temp = new byte[buffer.Length - 2];
            Array.Copy(buffer, 2, temp, 0, buffer.Length - 2);
            AdditionMsg = temp;
        }
    }




    public class ServerMessage : IDataFrame
    {
        public MSGKIND MessageKind { get; set; }
        public EXERESULT ExecrResult { get; set; }
        public byte[] AdditionMsg { get; set; }

        public ServerMessage()
        {

        }

        public ServerMessage(MSGKIND msgKind, EXERESULT exeResult, byte[] additionMsg)
        {
            MessageKind = msgKind;
            ExecrResult = exeResult;
            AdditionMsg = additionMsg;
        }

        public byte[] ToBuffer()
        {
            byte[] message;
            byte[] temp1 = Transform.MinInt2Bytes((int)ExecrResult); //单字节
            byte[] temp2 = Transform.MinInt2Bytes((int)MessageKind); //单字节

            if (AdditionMsg != null)
            {
                message = new byte[AdditionMsg.Length + 4];//1+1+2 = 4 
                byte[] temp3 = Transform.Int2Bytes(AdditionMsg.Length + 2); //双字节 
                Array.Copy(temp3, 0, message, 0, temp3.Length);
                Array.Copy(temp2, 0, message, temp3.Length, temp2.Length);
                Array.Copy(temp1, 0, message, temp3.Length + temp2.Length, temp1.Length);
                Array.Copy(AdditionMsg, 0, message, temp3.Length + temp2.Length + temp1.Length, AdditionMsg.Length);
            }
            else
            {
                message = new byte[4];
                byte[] temp3 = Transform.Int2Bytes(2);
                Array.Copy(temp3, 0, message, 0, temp3.Length);
                Array.Copy(temp2, 0, message, temp3.Length, temp2.Length);
                Array.Copy(temp1, 0, message, temp3.Length + temp2.Length, temp1.Length);
            }
            return message;
        }

        public void FromBuffer(byte[] buffer)
        {
            MessageKind = (MSGKIND)((int)buffer[0]);
            ExecrResult = (EXERESULT)((int)buffer[1]);
            byte[] temp = new byte[buffer.Length - 2];
            Array.Copy(buffer, 2, temp, 0, buffer.Length - 2);
            AdditionMsg = temp;
        }
    }


}
