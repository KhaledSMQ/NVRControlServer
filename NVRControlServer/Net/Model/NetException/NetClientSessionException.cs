using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVRControlServer.Net.Model.NetException
{
    public enum NetClientSessionErrono : int
    {
        SendData,
        ReceiveData
    }

    public class NetClientSessionException : Exception
    {
        private NetClientSessionErrono erroNo;
        private string erroMessage;

        public NetClientSessionErrono ErroNo
        {
            get { return erroNo; }
            set { erroNo = value; }
        }

        public string ErroMessage
        {
            get { return erroMessage; }
            set { erroMessage = value; }
        }


        public NetClientSessionException(NetClientSessionErrono erroNo,
                                                                        string erroMessage)
        {
            this.erroNo = erroNo;
            this.erroMessage = erroMessage;
        }

        public String ToString()
        {
            return "客户端通话错误:" + erroMessage + "\n"
                            + "错误代码:" + erroNo;
        }

    }
}
