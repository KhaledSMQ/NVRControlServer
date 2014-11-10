//#define ApplicationDebuge

using System;
using System.Net.Sockets;

using NVRControlServer.Net.Model.NetException;

namespace NVRControlServer.Net.Model
{
    public class TcpPort
    {

        #region 1. 变量属性

        #region 1.1 变量
        private Socket tcpPortSocket = null;
        private bool tcpPortConnected = false;
        private String serverIp = "127.0.0.1";
        private int serverPort = 8889;
        private static int tcpPortSendOutTime = 5;
        private static int tcpPortReceiveOutTime = 5; //接收超时
        private static int tcpPortReceiveOutTimes = 3;//接收次数
        #endregion 1.1 变量

        #region 1.2 属性
        public String ServerIp
        {
            get { return serverIp; }
            set { serverIp = value; }
        }

        public int ServerPort
        {
            get { return serverPort; }
            set { serverPort = value; }
        }

        public Socket TcpPortSocket
        {
            get
            {
                if (tcpPortSocket == null)
                {
                    try
                    {
                        tcpPortSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        return tcpPortSocket;
                    }
                    catch (SocketException ex)
                    {
                        throw new NetTcpPortException(NetTcpPortErrono.GetSocketFail, "无法创建Tcp端口");
                    }
                }
                else
                    return tcpPortSocket;
            }
            set { this.tcpPortSocket = value; }
        }

        public bool TcpPortConnected
        {
            get
            {
                if (TcpPortSocket == null)
                {
                    tcpPortConnected = false;
                    return false;
                }
                else if (TcpPortSocket.Connected)
                {
                    tcpPortConnected = true;
                    return true;
                }
                else
                    return false;
            }
            set { this.tcpPortConnected = value; }
        }

        #endregion 1.2 属性

        #endregion 1. 变量属性

        #region 2. 构造方法

        #region 2.1 有参构造
        public TcpPort(String serverIp, int serverPort)
        {
            this.serverIp = serverIp;
            this.serverPort = serverPort;
            this.tcpPortSocket = TcpPortSocket;
        }

        public TcpPort(Socket tcpPortSocket)
        {
            this.tcpPortSocket = tcpPortSocket;
        }
        #endregion 2.1 有参构造

        #endregion 2. 构造方法

        #region 3. 私有方法
        /// <summary>
        ///  检查socket状态
        /// </summary>
        /// <param name="second">等待响应的秒数</param>
        /// <param name="mode">响应的操作</param>
        /// <returns>响应结果</returns>
        private bool CheckSocketStatus(int second, SelectMode mode)
        {
            bool flag = false;
            int msecond = second < 0 ? -1 : second * 1000;

            if (tcpPortSocket != null)
            {
                switch (mode)
                {
                    case SelectMode.SelectRead:
                        flag = TcpPortSocket.Poll(msecond, SelectMode.SelectRead);
                        break;
                    case SelectMode.SelectWrite:
                        flag = TcpPortSocket.Poll(msecond, SelectMode.SelectWrite);
                        break;
                    case SelectMode.SelectError:
                        flag = TcpPortSocket.Poll(msecond, SelectMode.SelectError);
                        break;
                }
                return flag;
            }
            else
                return false;
        }
        #endregion 3. 私有方法

        #region 4. 公有方法
        public void TcpPortShutDown()
        {
            TcpPortSocket.Shutdown(SocketShutdown.Both);
        }

        public void ReceiveTimeOut(int second)
        {
            second *= 1000;
            if (second < 0)
            {
                second = -1;
            }
            TcpPortSocket.ReceiveTimeout = second;
        }

        public bool Connect(string serverIp, int serverPort)
        {
            try
            {
                TcpPortSocket.Connect(serverIp, serverPort);
                if (TcpPortSocket.Connected)
                {
                    TcpPortConnected = true;
                    return true;
                }
                else
                {
                    TcpPortConnected = false;
                    return false;
                }
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.ConnectionAborted)
                    throw new NetTcpPortException(NetTcpPortErrono.ConnectAborted, "与服务器连接中断");
                else if (ex.SocketErrorCode == SocketError.ConnectionRefused)
                    throw new NetTcpPortException(NetTcpPortErrono.ConnectionRefused,"服务器主动拒绝连接");
                else if (ex.SocketErrorCode == SocketError.ConnectionReset)
                    throw new NetTcpPortException(NetTcpPortErrono.ConnectionReset,"远程服务器主动重置");
                else
                    throw new NetTcpPortException(NetTcpPortErrono.UnkownErro,"无法识别的错误");
            }
        }

        /// <summary>
        /// 根据字符个数接收
        /// </summary>
        /// <param name="data">存储字符数组</param>
        /// <param name="size">字符个数</param>
        /// <returns>已接受的字符个数</returns>
        public int Receive(byte[] data, int size)
        {
            if (TcpPortSocket == null || !TcpPortSocket.Connected)
            {
                throw new NetTcpPortException(NetTcpPortErrono.ConnectFail,"未连接远程主机");
            }

            if (data == null || data.Length == 0)
            {
                throw new NetTcpPortException(NetTcpPortErrono.ReceiveBuffErro,"接收远程主机缓冲区错误");
            }

            int total = 0;
            int data_left = size;
            int recv_num = 0;
            int lastTotal = 0;
            int times = 0;

            while (total < size)
            {
                if (CheckSocketStatus(tcpPortReceiveOutTime, SelectMode.SelectWrite))
                {
                    try
                    {
                        recv_num = TcpPortSocket.Receive(data, total, data_left, SocketFlags.None);
                    }
                    catch (SocketException ex)
                    {
                       if (ex.SocketErrorCode == SocketError.ConnectionAborted)
                            throw new NetTcpPortException(NetTcpPortErrono.ConnectAborted,"与远程主机连接中断");
                        else if (ex.SocketErrorCode == SocketError.ConnectionRefused)
                            throw new NetTcpPortException(NetTcpPortErrono.ConnectionRefused,"远程主机主动中断连接");
                        else if (ex.SocketErrorCode == SocketError.HostDown)
                            throw new NetTcpPortException(NetTcpPortErrono.HostDown,"远程主机关机");
                        else
                        {
                            if (total == 0 || (lastTotal == total && times > tcpPortReceiveOutTimes))
                                throw new NetTcpPortException(NetTcpPortErrono.ReceiveErro,"接收远程主机数据超时");
                            else if (lastTotal == total)
                                ++times;
                            else
                                times = 0;
                        }
                    }
                    total += recv_num;
                    data_left -= recv_num;
                }
            }
            return total;
        }

        /// <summary>
        /// 接收字符数组
        /// </summary>
        /// <param name="data">存储字符数组</param>
        /// <returns>接受到的字符个数</returns>
        public int Receive(byte[] data)
        {
            byte[] temp = new byte[3];
            int size;
            int recv_num;
            try
            {
                Receive(temp, 2);
                size = Utils.Transform.parseByte(temp);
                if (size == 0)
                    return 0;
                recv_num = Receive(data, size);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return recv_num;
        }

        public int Send(byte[] data)
        {
            if (TcpPortSocket == null || !TcpPortSocket.Connected)
            {
                throw new NetTcpPortException(NetTcpPortErrono.ConnectFail,"未连接远程主机");
            }
            int size = data.Length;
            int total = 0;
            int data_left = size;
            int send_num;
            try
            {
                while (total < size)
                {
                    if (CheckSocketStatus(tcpPortSendOutTime, SelectMode.SelectWrite))
                    {
                        send_num = TcpPortSocket.Send(data, total, data_left, SocketFlags.None);
                        total += send_num;
                        data_left -= send_num;
                    }
                    else
                        continue;
                }
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.TimedOut)
                    throw new NetTcpPortException(NetTcpPortErrono.SendTimeOut, "向远程主机发送数据超时");
                else if (ex.SocketErrorCode == SocketError.ConnectionAborted)
                    throw new NetTcpPortException(NetTcpPortErrono.ConnectAborted,"与远程主机连接中断");
                else if (ex.SocketErrorCode == SocketError.ConnectionRefused)
                    throw new NetTcpPortException(NetTcpPortErrono.ConnectionRefused,"远程主机主动中断连接");
                else if (ex.SocketErrorCode == SocketError.HostDown)
                    throw new NetTcpPortException(NetTcpPortErrono.HostDown,"远程主机关机");
                else
                    throw new NetTcpPortException(NetTcpPortErrono.UnkownErro,"发生未知网络错误");
            }
            return total;
        }
        #endregion 4.公有方法
    }
}
