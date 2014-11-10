#define ApplicationDebug
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

using NVRControlServer.Net.Utils;
using NVRControlServer.Net.Model.Event;
using NVRControlServer.Net.Model.NetException;
using NVRControlServer.Net.Control;


namespace NVRControlServer.Net.Model
{
        public enum ClientStaus : int
        {
            login = 0,
            logout,
        };

        public class ClientSession : IDisposable
        {
                #region 1.变量属性

                #region 1.1变量

                private string clientName = string.Empty; /// 客户端名
                private string clientIp = string.Empty;/// 客户端IP地址
                private int clientPort = -1;/// 客户端端口 
                private Socket clientSocket = null;/// 客户端Socket
                private ClientStaus clientStatus;/// 客户端状态
                private TcpPort clientTcpPort = null;/// 客户端对应的tcp传输接口
                private Thread clientReceiveThread;   /// 接客户端数据线程 
                private static int CLIENTBUFFERSIZE =2 * 1024 * 1024; ///接收数据缓冲区大小 
                private bool receiveFlag = true;
                private TcpSendFile tcpSendFile;

                /// <summary> 声明客户端处理请求委托 </summary>
                public delegate void ServiceHandler(ClientSession clientSession,CommunicateMsg msg);
                /// <summary> 声明客户端请求事件 </summary>
                public event ServiceHandler receiveClientDataEvent;

                /// <summary>向状态栏添加信息 </summary>
                public delegate void AddStatuListHandle(string msg);
                /// <summary> 向状态栏添加信息事件</summary>
                public event AddStatuListHandle addEvent;

                /// <summar>向服务器状态栏添加信息</summar>
                public delegate void AddServerStatusListViewHandle(string[] msg);
                /// <summar>向服务器状态栏添加信息事件</summar>
                public event AddServerStatusListViewHandle addServerStatusEvent;

              
                public event TcpPortReceiveDataEventHandler TcpPortReceiveData;

                #endregion 1.1 变量

                #region 1.2 属性
                public string ClientName
                {
                    get { return this.clientName; }
                    set { this.clientName = value; }
                }

                public string ClientIp
                {
                    get { return this.clientIp; }
                    set { this.clientIp = value; }
                }

                
                public Socket ClientSocket
                {
                    get { return this.clientSocket; }
                    set { this.clientSocket = value; }
                }

                public TcpPort ClientTcpPort
                {
                    get { return this.clientTcpPort; }
                }

                public ClientStaus ClientStatus
                {
                    get { return this.clientStatus; }
                    set { this.clientStatus = value; }
                }

                #endregion 1.2 属性

                #endregion 1.成员变量

                #region 2.构造函数
                public ClientSession(Socket clientsocket)
                {
                    this.clientSocket = clientsocket;
                    this.clientName = "Unknown";
                    this.clientIp = clientsocket.RemoteEndPoint.AddressFamily.ToString();
                    this.clientStatus = ClientStaus.login;
                    this.clientTcpPort = new TcpPort(clientsocket);
                    clientReceiveThread = new Thread(new ThreadStart(Receive));
                    clientReceiveThread.IsBackground = true;
                    clientReceiveThread.Start();
                }
                #endregion 2.构造函数

                #region 3.私有函数

                #region 3.1 等待接收客户端数据线程
                private void Receive()
                {
                    while (receiveFlag)
                    {
                            try
                            {
                                byte[] receiveBuf = new byte[CLIENTBUFFERSIZE];
                                int receiveNum =  clientTcpPort.Receive(receiveBuf);
                                byte[] receiveData = new byte[receiveNum];
                                Array.Copy(receiveBuf, 0, receiveData, 0, receiveNum);
                                CommunicateMsg CommandMsg = Utils.Transform.GetClientCommand(receiveData);
                                if (receiveClientDataEvent != null)
                                {
                                    receiveClientDataEvent(this, CommandMsg);
                                }
                            }
                            catch (Exception ex)
                            {
                                if(ex.GetType() == typeof(NetTcpPortException))
                                {
                                    NetTcpPortException netTcpPortException = ex as NetTcpPortException;
                                    if (netTcpPortException.ErroNo == NetTcpPortErrono.ConnectAborted ||
                                         netTcpPortException.ErroNo == NetTcpPortErrono.ReceiveErro)
                                    {
                                        Dispose();
                                    }
                                    else
                                        continue;
                                }
                                if (addEvent != null)
                                {
                                    addEvent("接收"+clientIp+"客户端数据出错");
                                }
                            }
                    }
                }

                private void OnReceiveData(TcpPortReceiveDataEventArgs e)
                {
                    if (TcpPortReceiveData != null)
                    {
                        TcpPortReceiveData(this, e);
                    }
                }
                #endregion 3.1等待接收客户端线程
            
                #region 3.2向客户端发送数据
                public void Send(Msgkind msgKind,ExecuteResult executeResult,byte[] addtionMsg)
                {
                    try
                    {
                        byte[] serverMsgData = Utils.Transform.addHeadServerMessage(msgKind, executeResult, addtionMsg);
                        ClientTcpPort.Send(serverMsgData);
                    }
                    catch (Exception ex)
                    {
                        throw new NetClientSessionException(NetClientSessionErrono.SendData,"发送数据错误");
                    }
                }
                #endregion 3.2向客户端发送数据

                public void Dispose()
                {
                    receiveFlag = false;
                    if (ClientTcpPort != null && ClientTcpPort.TcpPortConnected == true)
                    {
                        ClientTcpPort.TcpPortShutDown();
                    }
                    if (clientReceiveThread != null && clientReceiveThread.IsAlive)
                    {
                        clientReceiveThread.Abort();
                        clientReceiveThread = null;
                    }
                }

                #endregion 3.私有函数

        }
}
