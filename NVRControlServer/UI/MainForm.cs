#region ************************文件说明************************************
/// 作者(Author)：                     WSN640 黄顺彬
/// 
/// 日期(Create Date)：              2014.6.11
/// 
/// 功能：                                 程序主界面
///
/// 修改记录(Revision History)：无
///
#endregion *****************************************************************

//#define ApplicationDebug
#define Log

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.IO;

using NVRControlServer.Net.Control;
using NVRControlServer.Net.Model;
using NVRControlServer.Net.Utils;
using NVRControlServer.NVR.Control;
using NVRControlServer.Storage.Control;
using NVRControlServer.Storage.Utils;
using NVRControlServer.Utils.Log;

namespace NVRControlServer.UI
{

    public partial class MainForm : Form
    {

        #region 1. 变量属性

        #region 1.1 变量
        private string serverIp = "192.168.1.15";
        //private const string serverIp = "127.0.0.1";
        //private const string serverIp = "192.168.100.12";
        private  int serverPort = 8889;
        private  int serverListenNum = 100;
        private Server controlServer;

        private  string nvrDeviceConfigFile = Application.StartupPath + "\\NVR_SMS.ini";//定义设备信息配置文件
        private  string sysytemLogFile = Application.StartupPath + "\\Log.txt"; //定义系统日志文件路径
        private  string autoVideoFigFile = Application.StartupPath + "\\VideoSet.ini";//定义自动录像配置文件
        private string backupVideoFigFile = Application.StartupPath + "\\BackupSet.ini";//定义备份配置文件
        //private  string storageDataPath = "D:/视频存储/";  //数据存储目录
        private string storageDataPath = "E:/视频存储/";
        private string backupDataPath = "E:/历史视频存储/";

        private List<NVRControler> nvrList;  //保存所有Nvr设备
        private HashSet<ClientSession> clientSet = new HashSet<ClientSession>(); //存储用户列表
        private StorageDataCenter storageDataCenter;  //视频数据存储控制中心
        private BackupDataCenter backupDataCenter; //视频数据备份控制中心

        private Thread mlistenThread = null;
        private Socket clientSocket;               //连接客户端socket
        private TcpSendFile tcpSendFile;

        #endregion 1.1 变量

        #region 1.2 属性
        public List<NVRControler> NvrList
        {
            get { return this.nvrList; }
            set { this.nvrList = value; }
        }

        public StorageDataCenter StorageControlCenter
        {
            get { return this.storageDataCenter; }
            set { this.storageDataCenter = value; }
        }

        public string StorageDataPath
        {
            get { return this.storageDataPath; }
            set { this.storageDataPath = value; }
        }

        #endregion 1.2 属性

        #endregion 1.变量属性


        public MainForm()
        {
            //Application.Run(new LogoForm());//运行初始化窗口
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Initialization();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (controlServer != null)
            if (storageDataCenter != null)
                storageDataCenter.StopAllTask();
        }

    
        private bool Initialization()
        {
            #region 1.1控制服务器监听端口初始化
            controlServer = new Server();
            if (controlServer.Initialization(serverIp, serverPort, serverListenNum))
            {
                AddItemToListBox(string.Format("开始在{0}:{1}监听客户端连接,最大连接数为{2}", serverIp, serverPort, serverListenNum));
                Log.WriteLog(LogType.Trace, "服务器启动成功");
                buttonStart.Enabled = false;
                buttonStop.Enabled = true;
                mlistenThread = new Thread(ListenClientConnect);
                mlistenThread.Start();
            }
            else
            {
                AddItemToListBox(string.Format("在{0}:{1}监听客户端连接失败", serverIp, serverPort));
                Log.WriteLog(LogType.Error, "服务器启动失败");
                return false;
            }
            #endregion 1.1控制服务器监听端口初始化

            #region 1.2读取NVR配置文件,初始化设备列表
            if ((nvrList = ReadConfig(nvrDeviceConfigFile)) != null)
            {
                AddItemToListBox("读取配置文件成功");
                Log.WriteLog(LogType.Trace, "读取配置文件成功");
            }
            else
            {
                AddItemToListBox("读取配置文件失败");
                Log.WriteLog(LogType.Error, "读取配置文件失败");
                return false;
            }
            #endregion 1.2读取NVR配置文件,初始化设备列表

            #region 1.3 界面初始化
            UpadateNvrStausListView();
            #endregion 1.3界面初始化

            #region 1.4 数据存储中心初始化
            storageDataCenter = new StorageDataCenter(storageDataPath, nvrList, autoVideoFigFile);
            if (storageDataCenter.Initalization())
            {
                Log.WriteLog(LogType.Trace, "数据存储中心控制初始化成功");
                storageDataCenter.StartAutoVideo();
            }
            else
            {
                Log.WriteLog(LogType.Error, "数据存储中心控制初始化失败");
            }
            #endregion 1.4 数据存储中心初始化
      
            #region 1.5 备份数据中心初始化
            backupDataCenter = new BackupDataCenter(backupDataPath, nvrList, backupVideoFigFile);
            if (backupDataCenter.Initalization())
            {
                Log.WriteLog(LogType.Trace, "数据存储中心控制初始化成功");
            }
            else
            {
                Log.WriteLog(LogType.Error, "备份数据存储中心控制初始化失败");
            }
            #endregion 1.5 备份数据中心初始化

            tcpSendFile = new TcpSendFile();
   
            return true;
        }

        #region 服务器端口连接监听线程
        private void ListenClientConnect()
        {
            while (controlServer.IsAccept)
            {
                try
                {
                    clientSocket = controlServer.Accept();
                    ClientSession clientSession = new ClientSession(clientSocket);
                    //controlServer.IsListening = true;
                    //clientSession.ClientIp = (clientSocket.RemoteEndPoint as IPEndPoint).Address.ToString();
                    //clientSession.ClientPort = (clientSocket.RemoteEndPoint as IPEndPoint).Port;
                    //AddItemToServerStatusListView(new string[] { "进入", mClient.ClientIp, mClient.ClientPort.ToString() });
                    clientSession.receiveClientDataEvent += new ClientSession.ServiceHandler(ServiceProcess);
                    clientSession.addEvent += new ClientSession.AddStatuListHandle(AddItemToListBox);
                    //mClient.addServerStatusEvent += new ClientSession.AddServerStatusListViewHandle(AddItemToServerStatusListView);
                }
                catch(Exception ex)
                {
                    AddItemToListBox("服务器接收客户端连接出错");
                }
            }
        }
        #endregion 服务器端口连接监听线程

        #region 处理客户端请求

        private void ServiceProcess(ClientSession clientSession, CommunicateMsg msg)
        {
            object[] objects = new object[] { clientSession, msg };
            ThreadPool.QueueUserWorkItem(new WaitCallback(Processing), objects);
        }


        private void Processing(object o)
        {
            object[] values = (object[])o;
            ClientSession clientSession = (ClientSession)values[0];
            CommunicateMsg communicateMsg = (CommunicateMsg)values[1];

            int commandKind = (int)communicateMsg.CommandKind;
            int clientIdentify = (int)communicateMsg.RightIdentify;
            byte[] commandAddtionMsg = (byte[])communicateMsg.AdditionMsg;

            Msgkind msgKind;
            ExecuteResult executeResult;
            string replayMsg = string.Empty;
            string msgBuffer = string.Empty;
            string[] nMsgBuffer;
            byte[] replayMsgByte = null;

            try
            {
                   switch (commandKind)
                        {
                            //登陆
                            case (int)Command.RequestLogin:
                                msgKind = Msgkind.ResponseLogin;
                                try
                                {
                                    //for (int j = 0; j < this.NvrList.Count; j++)
                                    //{
                                    //    NVRControler nvrControler = this.NvrList[j];
                                    //    Console.WriteLine(nvrControler.NvrName + "登录成功!");
                                    //    NVRChannel[] nvrChannels = nvrControler.NvrChannels;
                                    //    for (int i = 0; i < nvrChannels.Length; i++)
                                    //    {
                                    //        string channelStorageDataPath = this..StorageDataPath +
                                    //                nvrControler.NvrName + "/" + i + "/";
                                    //        StorageTaskParameter storageThreadParameter = new StorageTaskParameter
                                    //                (j, i, 15, 2, nvrControler, channelStorageDataPath, new TimeSpan(0, 0, 1), true);
                                    //        DateTime sheduleTime = DateTime.Now;
                                    //        ScheduleExecutionOnce future = new ScheduleExecutionOnce(sheduleTime);
                                    //        StorageTask sheduleTask = new StorageTask(future);
                                    //        sheduleTask.Job += new TimerCallback(sheduleTask.Execute);
                                    //        sheduleTask.JobParam = storageThreadParameter;
                                    //        storageDataCenter.AddTask(sheduleTask);
                                    //    }
                                    //}
                                     executeResult = ExecuteResult.ExcuteSuccess;
                                    replayMsg = "成功开启教学视频录像";
                                }
                                catch(Exception ex)
                                {
                                      executeResult = ExecuteResult.ExcuteSuccess;
                                       replayMsg = "开启教学视频录像失败";
                                }
                                replayMsgByte = Transform.string2Byte(replayMsg);
                                clientSession.Send(msgKind, executeResult, replayMsgByte);
                                break;

                            //退出
                            case (int)Command.RequestLogout:
                                msgKind = Msgkind.ResponseLogout;
                                try
                                {
                                        executeResult = ExecuteResult.ExcuteSuccess;
                                        replayMsg = "成功关闭教学视频录像!";
                                }
                                catch(Exception ex)
                                {
                                    executeResult = ExecuteResult.ExcuteFail;
                                        replayMsg = "关闭教学视频录像失败!";
                                }
                                replayMsgByte = Transform.string2Byte(replayMsg);
                                clientSession.Send(msgKind, executeResult, replayMsgByte);
                                break;

                            //更新nvr和通道状态
                            case (int)Command.RequestGetNvrChannelInfo:
                                int nvrselect = Transform.Bytes2Int(commandAddtionMsg);
                                string[][] strs = nvrList[nvrselect].GetChannelInfo();
                                replayMsgByte = Transform.Serialize(strs);
                                msgKind = Msgkind.ResponseGetNvrChannelInfo;
                                if (replayMsgByte == null)
                                {
                                    executeResult = ExecuteResult.ExcuteFail;
                                    replayMsg = "无法获得通道状态信息";
                                    replayMsgByte = Transform.string2Byte(replayMsg);
                                }
                                else
                                {
                                    executeResult = ExecuteResult.ExcuteSuccess;
                                }
                                clientSession.Send(msgKind, executeResult, replayMsgByte);
                                break;
#region 
                            ////云台转动控制
                            //case 3:
                            //    msgBuffer = Transform.byte2String(msgdata);
                            //    nMsgBuffer = Transform.getStrings(msgBuffer);
                            //    nvrselect = Transform.string2int(nMsgBuffer[0]);
                            //    int channelselect = Transform.string2int(nMsgBuffer[1]);
                            //    int ptzkind = Transform.string2int(nMsgBuffer[2]);
                            //    int begstop = Transform.string2int(nMsgBuffer[3]);
                            //    int speed = Transform.string2int(nMsgBuffer[4]);
                            //    NVRStatus status =(NVRStatus)nvrList[nvrselect].NvrStatus;
                            //    if (status == NVRStatus.Offline || 
                            //            status == NVRStatus.Null)
                            //    {
                            //        replayMsg = "the nvr not online or busy";
                            //        replayMsgByte = Transform.string2Byte(replayMsg);
                            //        executeResult = Exeresult.ExcuteFail;
                            //    }
                            //    else
                            //    {
                            //        if (nvrList[nvrselect].PTZControl(channelselect, ptzkind, begstop, speed))
                            //        {
                            //            if (begstop == 0)
                            //            {
                            //                Thread.Sleep(500);
                            //                begstop = 1;
                            //                nvrList[nvrselect].PTZControl(channelselect, ptzkind, begstop, speed);
                            //            }
                            //            replayMsg = "control success!";
                            //            replayMsgByte = Transform.string2Byte(replayMsg);
                            //            executeResult = Exeresult.ExcuteSuccess;
                            //        }
                            //        else
                            //        {
                            //            replayMsg = nvrList[nvrselect].GetLastErrorString();
                            //            replayMsgByte = Transform.string2Byte(replayMsg);
                            //            executeResult = Exeresult.ExcuteFail;
                            //        }
                            //    }
                            //     msgKind = Msgkind.ack;
                            //     tcpport.Send(msgKind, executeResult, replayMsgByte);
                            //    break;

                            ////Get或Set图像配置
                            //case 4:
                            //    msgKind = Msgkind.ack;
                            //    int NvrIndex = (int)msg.AdditionMsg[0];
                            //    int lChannelIndex = (int)msg.AdditionMsg[1];
                            //    uint BrightValue = (uint)msg.AdditionMsg[2];
                            //    uint ContrastValue = (uint)msg.AdditionMsg[3];
                            //    uint SaturationValue = (uint)msg.AdditionMsg[4];
                            //    uint HueValue = (BrightValue + ContrastValue + SaturationValue) / 3;
                            //    if (nvrList[NvrIndex].SetVideoEffect(lChannelIndex, 
                            //        BrightValue,ContrastValue,SaturationValue,HueValue))
                            //    {
                            //        executeResult = Exeresult.ExcuteSuccess;
                            //        replayMsg = "Image Configura success!";
                            //    }
                            //    else
                            //    {
                            //        executeResult = Exeresult.ExcuteSuccess;
                            //        replayMsg = "Image Configura Failed!";

                            //    }
                            //    replayMsgByte = Transform.string2Byte(replayMsg);
                            //    tcpport.Send(msgKind, executeResult, replayMsgByte);
                            //    break;
#endregion 
                            
                            //搜索视频文件信息
                            case (int)Command.RequestSearchFile:
                                msgKind = Msgkind.ResponseSearchFile;
                                msgBuffer = Transform.byte2String(commandAddtionMsg);
                                nMsgBuffer = Transform.GetStrings(msgBuffer,',');
                                int searchNvrId = (int)Transform.string2int(nMsgBuffer[0]);
                                int searchChannelId = (int)Transform.string2int(nMsgBuffer[1]);
                                DateTime searchStartTime = (DateTime)Transform.String2DateTime(nMsgBuffer[2]);
                                DateTime searchEndTime = (DateTime)Transform.String2DateTime(nMsgBuffer[3]);
                                string searchPath = storageDataPath + "/" + nvrList[searchNvrId].NvrName + "/" + searchChannelId;
                                List<SearchVideoFileInfo> searchFileList = FileHelper.SearchVideoFileByTime(
                                                                            searchPath, searchStartTime, searchEndTime);
                                if (searchFileList.Count != 0)
                                {
                                    executeResult = ExecuteResult.ExcuteSuccess;
                                    int fileListCount = searchFileList.Count;
                                    string[] fileStrings = new string[fileListCount];
                                    for(int i =0;i<searchFileList.Count; i++)
                                    {
                                        SearchVideoFileInfo file = searchFileList[i];
                                        string fileName = file.VideoFileName;
                                        string fileLength = file.VideoFileLength;
                                        string fileStartTime = file.VideoFileStartTime.ToString("yyyyMMddHHmmss");
                                        string fileEndTime = file.VideoFileEndTime.ToString("yyyyMMddHHmmss");
                                        fileStrings[i] = fileName + "," + fileLength + ","
                                                            + fileStartTime + "," + fileEndTime;
                                        replayMsgByte = (byte[])Transform.Serialize(fileStrings[i]);
                                        clientSession.Send(msgKind, executeResult, replayMsgByte);
                                    }
                                }
                                else
                                {
                                    executeResult = ExecuteResult.ExcuteFail;
                                    replayMsg  = "未搜索到指定文件!";
                                    replayMsgByte = (byte[])Transform.Serialize(replayMsg);
                                    clientSession.Send(msgKind, executeResult, replayMsgByte);
                                }                                                                                                                                                                                                                                                                                                 
                                break;

                            //下载文件
                            case (int)Command.RequestSendFile:
                                msgBuffer = Transform.byte2String(commandAddtionMsg);
                                nMsgBuffer = Transform.GetStrings(msgBuffer, '_');
                                int downLoadNvrId = Convert.ToInt32(nMsgBuffer[0]);
                                int downLoadChannelId = Convert.ToInt32(nMsgBuffer[1]);
                                string downLoadFileName = msgBuffer;
                                string downLoadFilePath = storageDataPath + nvrList[downLoadNvrId].NvrName
                                                            + "/" + downLoadChannelId + "/";
                                msgKind = Msgkind.ResponseSendFile;
                                try
                                {
                                    SendFileManager sendFileManager = new SendFileManager(downLoadFilePath, downLoadFileName);
                                    if (tcpSendFile.CanSend(sendFileManager))
                                    {
                                        tcpSendFile.StartSend(clientSession, sendFileManager);
                                        executeResult = ExecuteResult.ExcuteSuccess;
                                    }
                                    else
                                    {
                                        executeResult = ExecuteResult.ExcuteFail;
                                        replayMsg = "无法重复下载或无法找到" + sendFileManager.FileName;
                                        replayMsgByte = (byte[])Transform.string2Byte(replayMsg);
                                        clientSession.Send(msgKind, executeResult, replayMsgByte);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    executeResult = ExecuteResult.ExcuteFail;
                                    replayMsg = ex.ToString();
                                    replayMsgByte = (byte[])Transform.string2Byte(replayMsg);
                                    clientSession.Send(msgKind, executeResult, replayMsgByte);
                                }
                                break;

                            //发送文件包
                            case (int)Command.RequestSendFilePack:
                                ResponeTraFransfersFile responeTraFransfersFile =
                                    (ResponeTraFransfersFile)Transform.Deserialiaze(commandAddtionMsg);
                                if(responeTraFransfersFile.Index == 0)
                                {
                                    tcpSendFile.Send(clientSession, responeTraFransfersFile.MD5); 
                                }
                                break;

                           //取消发送文件
                            case (int)Command.RequestCancelSendFile:
                                 responeTraFransfersFile = (ResponeTraFransfersFile)Transform.Deserialiaze(commandAddtionMsg);
                                 tcpSendFile.CancelSend(clientSession, responeTraFransfersFile.MD5);
                                break;
                            
                            //取消正在发送文件
                            case (int)Command.RequestCancelReceiveFile:
                                msgBuffer = Transform.byte2String(commandAddtionMsg);
                                tcpSendFile.CancelSend(clientSession, msgBuffer);
                                break;

                            default:
                                break;

                            ////其他与云台控制相似
                            //default:
                            //    msgKind = Msgkind.ack;
                            //    executeResult = Exeresult.ExcuteFail;
                            //    replayMsg = "cannot recognize the command";
                            //    replayMsgByte = System.Text.Encoding.Default.GetBytes(replayMsg);
                            //    tcpport.Send(msgKind, executeResult, replayMsgByte);
                            //string ip       =  (tcpport.TcpSocket.RemoteEndPoint as IPEndPoint).Address.ToString();
                            //string  port  =  (tcpport.TcpSocket.RemoteEndPoint as IPEndPoint).Port.ToString();
                            //switch(comkind)
                            //{
                            //    case 0:
                            //        AddItemToServerStatusListView(new string[] { "登录", ip, port });
                            //        break;
                            //    case 1:
                            //        AddItemToServerStatusListView(new string[] { "注销", ip, port });
                            //        break;
                            //    case 2:
                            //        AddItemToServerStatusListView(new string[] { "获取通道", ip, port });
                            //        break;
                            //    case 3:
                            //         AddItemToServerStatusListView(new string[] { "云台控制", ip, port });
                            //        break;                  
                            //}

                            //    AddItemToListBox("成功处理客户端操作");
                            //}
                        }
                }
                catch (Exception ex)
                {
//                    controlServer.IsBusy = false;
//#if ApplicationDebug
//                    MessageBox.Show("未能成功处理客户端操作");
//#endif
//                    msgKind = Msgkind.ack;
//                    executeResult = Exeresult.ExcuteFail;
//                    replayMsg = "control fail!";
//                    replayMsgByte = System.Text.Encoding.Default.GetBytes(replayMsg);
//                    tcpport.Send(msgKind, executeResult, replayMsgByte);

//                    AddItemToListBox("未能成功处理客户端操作" + ex.ToString());
                }
        }
        #endregion

        #region 读取配置文件初始化NVR控制器列表
        private List<NVRControler> ReadConfig(string filepath)
        {
            try
            {
                List<NVRControler> NvrList = new List<NVRControler>();
                StreamReader configInfo = new StreamReader(nvrDeviceConfigFile, Encoding.Unicode);
                string flag = configInfo.ReadLine();
                while (flag == "NVR-BEGIN")
                {
                    string NvrName = configInfo.ReadLine();
                    string NvrIP = configInfo.ReadLine();
                    Int16 NvrPort = Int16.Parse(configInfo.ReadLine());
                    string NvrUser = configInfo.ReadLine();
                    string NvrPassword = configInfo.ReadLine();
                    Int16 NvrMaxChnnelNum = Int16.Parse(configInfo.ReadLine());
                    NvrList.Add(new NVRControler(NvrName, NvrIP, NvrPort, NvrUser, NvrPassword, NvrMaxChnnelNum));
                    flag = configInfo.ReadLine();
                }
                return NvrList;
            }
            catch (IOException ex)
            {
                return null;
            }
        }
        #endregion

        #region 获取Nvr状态字节数组（0/1/2)
        private byte[] GetNvrListStatus(List<NVRControler> nrvlist)
        {
            byte[] statusbytes = new byte[nrvlist.Count];
            int i = 0;
            char ch;
            foreach (NVRControler nvrcontroler in nrvlist)
            {
                NVRStatus status = (NVRStatus)nvrcontroler.NvrStatus;
                if (status == NVRStatus.Online)
                    ch = '0';
                else if (status == NVRStatus.Offline)
                    ch = '1';
                else if (status == NVRStatus.Free)
                    ch = '2';
                else
                    ch = '3';
                statusbytes[i++] = (byte)ch;
            }
            return statusbytes;
        }
        #endregion

        #region 更新Nvr状态列表listview
        private void UpadateNvrStausListView()
        {
            nvrList[0].Login();
            nvrList[0].RefreshChannelInfo();

            nvrList[1].Login();
            nvrList[1].RefreshChannelInfo();

            //CheckNvrStatus_Timer.Enabled = true;
            //CheckNvrStatus_Timer.Start();

            //foreach (NVRControler nvrcontroler in nvrList)
            //{
            //    nvrcontroler.Login();
                
            //    NVRStatus status = (NVRStatus)nvrcontroler.NvrStatus;
            //    if (status == NVRStatus.Online)
            //        AddItemToListBox(string.Format(nvrcontroler.NvrName + "连接成功"));
            //    else if (status == NVRStatus.Offline)
            //        AddItemToListBox(string.Format(nvrcontroler.NvrName + "连接失败"));
            //    else if (status == NVRStatus.Free)
            //        AddItemToListBox(string.Format(nvrcontroler.NvrName + "处于空闲状态"));
            //    else
            //        AddItemToListBox(string.Format(nvrcontroler.NvrName + "NULL状态"));
                
            //}

            //this.NVRStatus_listView.Items.Clear();
            //foreach (NVRControler nvrcontroler in nvrList)
            //{
            //    ListViewItem lvitem = new ListViewItem(new string[] { nvrcontroler.NvrName, nvrcontroler.NvrIP, (nvrcontroler.NvrPort).ToString(), nvrcontroler.NvrStatusString });
            //    this.NVRStatus_listView.Items.Add(lvitem);
            //}
        }
        #endregion

        #region 添加服务器状态listview信息
        private delegate void AddItemToServerStatusListVewDelegate(string[] strs);
        private void AddItemToServerStatusListView(string[] strs)
        {
            if (ServerStatus_listView.InvokeRequired)
            {
                AddItemToServerStatusListVewDelegate d = AddItemToServerStatusListView;
                ServerStatus_listView.Invoke(d, new object[] { strs });
            }
            else
            {
                ListViewItem item  = new ListViewItem(new string[]{ strs[0], strs[1], strs[2], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")});
                this.ServerStatus_listView.Items.Add(item);
            }
        }
        #endregion 添加服务器状态listview信息

        #region listbox中添加状态信息
        private delegate void AddItemToListBoxDelegate(string str);
        /// <summary>
        ///  在listbox中添加状态信息
        /// </summary>
        /// <param name="str">要追加的内容</param>
        private void AddItemToListBox(string str)
        {
            if (listBoxStatus.InvokeRequired)
            {
                AddItemToListBoxDelegate d = AddItemToListBox;
                listBoxStatus.Invoke(d, str);
            }
            else
            {
                listBoxStatus.Items.Add("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]" + str);
                listBoxStatus.SelectedIndex = listBoxStatus.Items.Count - 1;
                listBoxStatus.ClearSelected();
            }
        }
        #endregion

        #region  nvr状态列表栏中添加信息
        private delegate void AddItemToListViewNVRDelegate(string str);
        /// <summary>
        /// 在nvr状态列表栏中添加信息
        /// </summary>
        /// <param name="str"></param>
        private void AddItemToListBoxNVR(string str)
        {
 
        }
        #endregion

        #region 检查连接客户端状态
        /// <summary>
        /// 检查连接服务器客户端的状态，剔除已断开
        /// </summary>
        /// <param name="ptzclientset"></param>
        /// <returns></returns>
        private int CheckClientOnline(HashSet<ClientSession> ptzclientlist)
        {
            int online = 0;
            foreach (ClientSession client in ptzclientlist)
            {
                if (!client.ClientSocket.Connected)
                {
                    ptzclientlist.Remove(client);
                }
                else
                    online++;
            }
            return online;
        }
        #endregion 检查连接客户端状态

        #region 根据传输的状态byte数组更新本地nvr列表状态
        private bool UpdateNvrStatus(byte[] statusbytes, List<NVRControler> nrvlist)
        {
            int i = 0;
            if (statusbytes.Length != nvrList.Count || nvrList.Count == 0)
            {
                return false;
            }
            else
            {
                foreach (NVRControler nvrcontroler in nrvlist)
                {
                    switch (statusbytes[i++])
                    {
                        case (byte)'0': nvrcontroler.NvrStatus = NVRStatus.Online;
                            break;
                        case (byte)'1': nvrcontroler.NvrStatus = NVRStatus.Offline;
                            break;
                        case (byte)'2': nvrcontroler.NvrStatus = NVRStatus.Free;
                            break;
                        default: nvrcontroler.NvrStatus = NVRStatus.Null;
                            break;
                    }
                }
                return true;
            }
        }
        #endregion

        #region 更新时间
        private void TimeShow_timer_Tick(object sender, EventArgs e)
        {
            time_Day();
            labelX_Time.Text = DateTime.Now.ToLongTimeString();
        }

        private void time_Day()
        {
            labelX_Date.Text = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString();
        }
        #endregion

        #region 显示操作信息
        private void ShowControlWarnInMessageBox(bool controlSuccess, string warnMessage, NVRControler nvrControler)
        {
            BeginInvoke(new MethodInvoker(delegate()
            {
                if (controlSuccess)
                {
                    MessageBox.Show(this,
                        warnMessage,
                         "操作成功",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(this,
                           string.Format(warnMessage + ":\n 错误信息:{0}\n 错误代码:{1}",
                           nvrControler.GetLastErrorString(),
                           nvrControler.GetLastError()),
                           "操作失败",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error);
                }
            }));
        }

        private void ShowControlWarnInMessageBox(bool controlSuccess, string warnMessage)
        {
            BeginInvoke(new MethodInvoker(delegate()
            {
                if (controlSuccess)
                {
                    MessageBox.Show(this,
                        warnMessage,
                         "操作成功",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(this,
                          warnMessage,
                           "操作失败",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error);
                }
            }));
        }

        private void ShowControlExceptionInMessageBox(Exception ex)
        {
            BeginInvoke(new MethodInvoker(delegate()
            {
                MessageBox.Show(this,
             (string.Format("{0}", ex.ToString())),
             "异常信息",
             MessageBoxButtons.OK,
             MessageBoxIcon.Error);
            }));
        }
        #endregion 显示操作信息

        #region 网络配置窗口按钮
        private void Config_button_Click(object sender, EventArgs e)
        {
            ConfigForm m_ConfigForm = new ConfigForm();
            m_ConfigForm.Show();
        }
        #endregion 网络配置窗口按钮

        #region 关于窗口按钮
        private void About_button_Click(object sender, EventArgs e)
        {
            AboutForm m_AboutForm = new AboutForm();
            m_AboutForm.Show();
        }
        #endregion 关于窗口按钮

        #region 录像配置窗口按钮
        private void RecordConfig_button_Click(object sender, EventArgs e)
        {
            RecordConfigForm reconfigForm = new RecordConfigForm(this);
            reconfigForm.Show();
        }
        #endregion 录像配置窗口按钮

        #region 退出按钮
        private void Quit_button_Click(object sender, EventArgs e)
        {
            if (controlServer != null)
                controlServer.Close();
            if (storageDataCenter != null)
                storageDataCenter.StopAllTask();
        }
        #endregion 退出按钮

        private void CheckNvrStatus_Timer_Tick(object sender, EventArgs e)
        {
            //for (int i = 0; i < nvrList.Count; i++)
            //{
            //    nvrList[i].TurnNvrState();
            //}
        }


    }
}