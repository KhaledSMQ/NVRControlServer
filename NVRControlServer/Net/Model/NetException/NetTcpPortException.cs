using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVRControlServer.Net.Model.NetException
{
    public enum NetTcpPortErrono : int
    {
        GetSocketFail,
        ConnectTimeOut,
        ConnectFail,
        ConnectAborted,
        ConnectionRefused,
        ConnectionReset,
        ReceiveErro,
        SendTimeOut,
        ReceiveBuffErro,
        SendErro,
        UnkownErro,
        HostDown,
    }

    public class NetTcpPortException : Exception
    {
        private NetTcpPortErrono erroNo;
        private string erroMessage;

        public string ErroMessage
        {
            get { return erroMessage; }
            set { erroMessage = value; }
        }

        public NetTcpPortErrono ErroNo
        {
            get { return erroNo; }
            set { erroNo = value; }
        }

        public NetTcpPortException(NetTcpPortErrono erroNo, string erroMessage)
        {
            this.erroNo = erroNo;
            this.erroMessage = erroMessage;
        }

        public string ToString()
        {
            return "Tcp端口错误 : " + erroMessage +"\n"
                         + "错误代码:" + erroNo;
        }
    }
}
