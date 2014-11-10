using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


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

    public enum Msgkind : int
    {
        ResponseLogin=0,
        ResponseLogout,
        ResponseGetNvrChannelInfo,
        ResponseSearchFile,
        ResponseSendFile,
        ResponseSendFilePack,
        ResponseCancelSendFile,
        ResponseCancelReceiveFile,
    };

    public enum ExecuteResult : int
    {
        ExcuteSuccess = 0,
        ExcuteFail,
    }; 

    public enum Command : int
    {
        RequestLogin=0,
        RequestLogout,
        RequestGetNvrChannelInfo,
        RequestSearchFile,
        RequestSendFile,
        RequestSendFilePack,
        RequestCancelSendFile,
        RequestCancelReceiveFile,
    };

    public enum Identify : int
    {
        LocalClient = 0,
        RemoteClient
    };
    #endregion 传输信息枚举类型

    public class CommunicateMsg
    {

        #region 1.变量属性
        private Msgkind messageKind;
        private Command commanKind;
        private Identify rightIdentify;
        private ExecuteResult execrResult;
        private byte[] additionMsg;

        public Msgkind MessageKind
        {
            set { this.messageKind = value; }
            get { return this.messageKind; }
        }

        public Command CommandKind
        {
            set{this.commanKind = value;}
            get{return this.commanKind;}
        }

        public Identify RightIdentify
        {
            set{this.rightIdentify = value;}
            get{return this.rightIdentify;}
        }

        public ExecuteResult ExecrResult
        {
            set{this.execrResult = value;}
            get{return this.execrResult;}
        }

        public byte[] AdditionMsg
        {
            set{this.additionMsg = value;}
            get{return this.additionMsg;}
        }


        #endregion 1.变量属性

        #region 2.构造函数
        public CommunicateMsg()
        {
            this.messageKind = 0;
            this.commanKind = 0;
            this.rightIdentify = 0;
            this.execrResult = 0;
            this.additionMsg = null;
        }

        /// <summary>
        /// 服务器发送的信息类型
        public CommunicateMsg(Msgkind msgkind, ExecuteResult exeresult, byte[] data)
        {
            this.messageKind = msgkind;
            this.execrResult = exeresult;
            this.additionMsg = data;
        }

        /// 客户端发送的信息类型
        public CommunicateMsg(Command comkind, Identify identify, byte[] data)
        {
            this.commanKind = comkind;
            this.rightIdentify = identify;
            this.additionMsg = data;
        }
        #endregion 2.构造函数


    }
}
