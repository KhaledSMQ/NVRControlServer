#region ************************文件说明************************************
/// 作者(Author)：                     ShunBin Huang
/// 
/// 日期(Create Date)：            2014.6.11
/// 
/// 功能：                                    传输接口基类 
///
/// 修改记录(Revision History)：无
///
#endregion *****************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace NVRControlServer.Net.Port
{

    public enum CommunicationType
    {
        IMAGE_TYPE,
        INT_TYPE,
        FLOAT_TYPE,
        STRUCT_TYPE
    };

    public abstract class LanPort
    {
        protected Socket mSocket;
        protected bool isInit;
        protected Mutex sendDataMutex;

        public LanPort()
        {

        }

        public virtual bool Initialization(string ip, int port) { return true; }
        public bool ConnectionTest(long timeout) { return true; }
        public bool Close() { return true; }
        public virtual int Send(byte[] buffer, int len, CommunicationType type) { return 0; }
        public virtual int Recv(byte[] buffer, int len, CommunicationType type, long timeout) { return 0; }

    }
}
