using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVRControlServer.Net.Model.NetException
{
    public enum NetTcpSendFileErrono : int
    {
        CreateSendFileManagerFail,
        ReadFileFail, 
        SendFileAgain,
    }


    public class NetTcpSendFileException : Exception
    {
        private NetTcpSendFileErrono erroNo;
        private string erroMessage;

        public NetTcpSendFileErrono ErroNo
        {
            get { return erroNo; }
            set { erroNo = value; }
        }

        public string ErroMessage
        {
            get { return erroMessage; }
            set { erroMessage = value; }
        }


        public NetTcpSendFileException(NetTcpSendFileErrono erroNo,
                                                                        string erroMessage)
        {
            this.erroNo = erroNo;
            this.erroMessage = erroMessage;
        }

        public String ToString()
        {
            return "发送视频文件错误:" + erroMessage + "\n"
                            + "错误代码:" + erroNo;
        }

    }
}
