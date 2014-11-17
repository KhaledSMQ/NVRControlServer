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
using NVRControlServer.Net.Interface;


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
                
                private Socket clientSocket = null;/// 客户端Socket
                private ClientStaus clientStatus;/// 客户端状态
                private TcpPort clientTcpPort = null;/// 客户端对应的tcp传输接口
                private Thread clientReceiveThread;   /// 接客户端数据线程 
                private static int RECEIVEBUFFERSIZE =2 * 1024 * 1024; ///接收数据缓冲区大小 
                private bool receiveFlag = true;
                private string playBackVideoCache = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Cache";

                private ICommandHandle loginHandle;
                private ICommandHandle searchFileByTimeHandle;
                private ICommandHandle updateFileTagByNameHandle;
                private ICommandHandle playBackVideoByNameHandle;

                
                
                ///// <summary> 声明客户端处理请求委托 </summary>
                //public delegate void ServiceHandler(ClientSession clientSession,CommunicateMsg msg);
                ///// <summary> 声明客户端请求事件 </summary>
                //public event ServiceHandler receiveClientDataEvent;

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

                public string PlayBackVideoCache
                {
                    get { return playBackVideoCache; }
                    set { playBackVideoCache = value; }
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

                    loginHandle = new LoginHandle();
                    searchFileByTimeHandle = new SearchFileByTimeHandle();
                    updateFileTagByNameHandle = new UpdateFileTagByNameHandle();
                    playBackVideoByNameHandle = new PlayBackVideoByNameHandle();


                    loginHandle.SetNextHandler(searchFileByTimeHandle);
                    searchFileByTimeHandle.SetNextHandler(updateFileTagByNameHandle);
                    updateFileTagByNameHandle.SetNextHandler(playBackVideoByNameHandle);
                    
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
                                byte[] receiveBuf = new byte[RECEIVEBUFFERSIZE];
                                int receiveNum =  clientTcpPort.Receive(receiveBuf);
                                byte[] receiveData = new byte[receiveNum];
                                Array.Copy(receiveBuf, 0, receiveData, 0, receiveNum);
                                ClientCommand command = new ClientCommand();
                                command.FromBuffer(receiveData);
                                loginHandle.SetHandleModel(this, command);
                                searchFileByTimeHandle.SetHandleModel(this, command);
                                updateFileTagByNameHandle.SetHandleModel(this, command);
                                playBackVideoByNameHandle.SetHandleModel(this, command);
                                loginHandle.Process();
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
                public void Send(MSGKIND msgKind,EXERESULT executeResult,byte[] addtionMsg)
                {
                    try
                    {
                        //byte[] serverMsgData = Utils.Transform.addHeadServerMessage(msgKind, executeResult, addtionMsg);
                        ServerMessage msg = new ServerMessage(msgKind, executeResult, addtionMsg);
                        ClientTcpPort.Send(msg.ToBuffer());
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
