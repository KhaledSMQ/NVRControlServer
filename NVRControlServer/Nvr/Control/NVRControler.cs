#region ************************文件说明************************************
/// 作者(Author)：                             黄顺彬  
/// 
/// 日期(Create Date)：                     2014.7.13
/// 
/// 功能:                                                NVR控制类
///
/// 修改记录(Revision History)：     2014.11.3 增加状态控制
///                                                                                 自动重置连接
///                                                                                 发现连接控制错误                                           
#endregion *********************************************************************

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

using NVRControlServer.NVR.SDK;
using NVRControlServer.Nvr.Modle;
using System.Timers;
using System.Net.NetworkInformation;


namespace NVRControlServer.NVR.Control
{
    #region 结构体信息
    /// <summary> NVR设备状态枚举类型</summary>
    public enum NVRStatus : byte
    {
        //无效状态
        Null,
        //在线状态
        Online,
        //离线状态
        Offline,
        //空闲状态
        Free,
    }

    /// <summary>视频参数结构体</summary>
    public struct VideoEffect
    {
        /// <summary>亮度</summary>
        public uint BrightValue;
        /// <summary>对比度</summary>
        public uint ContrastValue;
        /// <summary>饱和度</summary>
        public uint SaturationValue;
        /// <summary>色度</summary>
        public uint HueValue;
    }

    //public struct NvrSearchFileStruct
    //{
    //    private string fileName;

    //    public string FileName
    //    {
    //        get { return fileName; }
    //        set { fileName = value; }
    //    }
    //    private int startYear;

    //    public int StartYear
    //    {
    //        get { return startYear; }
    //        set { startYear = value; }
    //    }
    //    private int startMonth;

    //    public int StartMonth
    //    {
    //        get { return startMonth; }
    //        set { startMonth = value; }
    //    }
    //    private int startDay;

    //    public int StartDay
    //    {
    //        get { return startDay; }
    //        set { startDay = value; }
    //    }
    //    private int startHour;

    //    public int StartHour
    //    {
    //        get { return startHour; }
    //        set { startHour = value; }
    //    }
    //    private int startMinute;

    //    public int StartMinute
    //    {
    //        get { return startMinute; }
    //        set { startMinute = value; }
    //    }
    //    private int startSecond;

    //    public int StartSecond
    //    {
    //        get { return startSecond; }
    //        set { startSecond = value; }
    //    }
    //    private int endYear;

    //    public int EndYear
    //    {
    //        get { return endYear; }
    //        set { endYear = value; }
    //    }
    //    private int endMonth;

    //    public int EndMonth
    //    {
    //        get { return endMonth; }
    //        set { endMonth = value; }
    //    }
    //    private int endDay;

    //    public int EndDay
    //    {
    //        get { return endDay; }
    //        set { endDay = value; }
    //    }
    //    private int endHour;

    //    public int EndHour
    //    {
    //        get { return endHour; }
    //        set { endHour = value; }
    //    }
    //    private int endMinute;

    //    public int EndMinute
    //    {
    //        get { return endMinute; }
    //        set { endMinute = value; }
    //    }
    //    private int endSecond;

    //    public int EndSecond
    //    {
    //        get { return endSecond; }
    //        set { endSecond = value; }
    //    }


    //    override
    //    public string ToString()
    //    {

    //        return null;
    //    }
    //}

    public class NvrSearchFileStruct
    {
        public string SearchFileName { get; set; }
        public string SearchFileStartTime { get; set; }
        public string SearchFileEndTime { get; set; }

        public NvrSearchFileStruct(string searchFileName, string searchFileStartTime, string searchFileEndTime)
        {
            SearchFileName = searchFileName;
            SearchFileStartTime = searchFileStartTime;
            SearchFileEndTime = searchFileEndTime;
        }
    }
    #endregion 结构体信息

    public class NVRControler
    {
        #region 1.变量属性

        #region 1.1 变量
        private CHCNetSDK.NET_DVR_DEVICEINFO_V30 nvrDeviceInfo;//模拟设备信息
        private CHCNetSDK.NET_DVR_IPPARACFG_V40 nvrStruIpParaCfg;//IP设备资源及IP通道资源配置结构体
        private Dictionary<int, CHCNetSDK.NET_DVR_IPCHANINFO> nvrChanInfoDictionary;//通道信息字典
        private CHCNetSDK.NET_DVR_STREAM_MODE nvrChannelStruStreamMode;//流模式
        private Int32 nvrUserHandle = -1;//NVR登陆标识
        private Int32 nvrFindHandle = -1;//NVR查找文件标识
        private Int32 nvrDownHandle = -1;//NVR下载标识

        private uint nvrLastErr = 0;//NVR最后错误代码
        private string nvrLastErrString = "";

        private int nvrId;
        private string nvrName;//NVR名字
        private string nvrIp;//NVR IP地址
        private Int16 nvrPort;//NVR端口
        private string nvrUserName;//NVR用户名
        private string nvrPassword;//NVR用户密码
        private Int16 nvrMaxChannelNum;//最大通道数
        private static bool nvrInitSDK = false;//初始化标志
        private NVRStatus nvrStatus = NVRStatus.Null;//NVR网络状态
        private string nvrLogPath = "C:\\SdkLog\\";//日志保存路径
        private NVRChannel[] nvrChannels;//通道数组

        private Timer checkNvrErroTimer;//用于检查NVR状态
        private int turnNvrStateFlag = 0;
        private int checkNvrErroFlag = 0;

        private object lockObject;
        private object turnStateObject;
        private object checkNvrErrObject;
        
        private NVRState nvrState = null;
        private NVRState oldNvrState;
        private NVROffLineState nvrOffLineState; //NVR设备离线状态
        private NVROnLineState nvrOnLineState;//NVR设备在线状态
        private NVRRefreshChannelFailState nvrRefreshChannelFailState;//NVR设备更新通道信息失败
        private NVRRefreshChannelSuccessState nvrRefreshChannelSuccessState;//NVR设备更新通道信息成功

        private NVRChannelRealPlayFalseState nvrChannelRealPlayFalseState;

        private System.Threading.AutoResetEvent waiter;


        private Ping nvrPingSender;
        private PingOptions options;

        #endregion 1.1 变量

        #region 1.2属性
        public int NvrId
        {
            get { return nvrId; }
            set { nvrId = value; }
        }

        public string NvrName
        {
            get { return this.nvrName; }
            set { this.nvrName = value; }
        }

        public string NvrIP
        {
            get { return this.nvrIp; }
            set { this.nvrIp = value; }
        }

        public Int16 NvrPort
        {
            get { return this.nvrPort; }
            set { this.nvrPort = value; }
        }

        public string NvrUserName
        {
            get { return this.nvrUserName; }
            set { this.nvrUserName = value; }
        }

        public string NvrPassword
        {
            get { return this.nvrPassword; }
            set { this.nvrPassword = value; }
        }

        public NVRStatus NvrStatus
        {
            get { return this.nvrStatus; }
            set { this.nvrStatus = value; }
        }

        public bool IsInitSDK
        {
            get { return nvrInitSDK; }
        }

        public Int16 NvrMaxChannelNum
        {
            get { return this.nvrMaxChannelNum; }
            set { this.nvrMaxChannelNum = value; }
        }

        public NVRChannel[] NvrChannels
        {
            get { return this.nvrChannels; }
            set { this.nvrChannels = value; }
        }

        public string NvrLogPath
        {
            get { return this.nvrLogPath; }
            set { this.nvrLogPath = value; }
        }

        public string NvrStatusString
        {
            get
            {
                switch (this.nvrStatus)
                {
                    case NVRStatus.Online: return "在线";
                    case NVRStatus.Offline: return "离线";
                    case NVRStatus.Free: return "空闲";
                    default: return "未知状态";
                }
            }
        }

        public uint NvrLastErr
        {
            get { return nvrLastErr; }
            set { nvrLastErr = value; }
        }

        public NVRState NvrState
        {
            get 
            { 
                lock(LockObject)
                return nvrState; 
            }
            set 
            { 
                lock(LockObject)
                this.nvrState = value; 
            }
        }

        public NVRState OldNvrState
        {
            get 
            {
                lock(LockObject)
                return oldNvrState; 
            }
            set 
            { 
                lock(LockObject)
                this.oldNvrState = value; 
            }
        }

        public object LockObject
        {
            get
            {
                if (lockObject == null)
                    lockObject = new object();
                return lockObject;
            }
            set { this.lockObject = value; }
        }

        public object CheckNvrErrObject
        {
            get
            {
                if (checkNvrErrObject == null)
                    checkNvrErrObject = new object();
                return checkNvrErrObject;
            }
            set { this.checkNvrErrObject = value; }
        }


        public object TurnStateObject
        {
            get
            {
                if (turnStateObject == null)
                    turnStateObject = new object();
                 return turnStateObject;
            }
        }

        public NVROnLineState NvrOnLineState
        {
            get { return this.nvrOnLineState; }
            set { this.nvrOnLineState = value; }
        }

        public Timer CheckNvrErroTimer
        {
            get
            {
                if (checkNvrErroTimer == null)
                {
                    checkNvrErroTimer = new Timer();
                    checkNvrErroTimer.Interval = 5000;
                    checkNvrErroTimer.Enabled = true;
                    checkNvrErroTimer.Elapsed += new ElapsedEventHandler(CheckNvrPingStatus);
                    return checkNvrErroTimer;
                }
                else
                    return checkNvrErroTimer;
            }
        }


        #endregion 1.2属性

        #endregion 1.变量属性

        #region 2.构造函数

        #region 2.1无参构造函数
        public NVRControler()
        {
            HK_Nvr_Init();
        }
        #endregion 2.1无参构造函数

        #region 2.2有参构造函数
        public NVRControler(string name, string ip, Int16 port, string usr, string pwd, Int16 maxchannelnum)
        {
            NvrChannels = new NVRChannel[maxchannelnum];

            for (int i = 0; i != maxchannelnum; i++)
            {
                NvrChannels[i] = new NVRChannel(i, IPCStatus.Free);
            }

            this.NvrName = name;
            this.NvrIP = ip;
            this.NvrPort = port;
            this.NvrUserName = usr;
            this.NvrPassword = pwd;
            this.NvrMaxChannelNum = maxchannelnum;
            HK_Nvr_Init();
            nvrChanInfoDictionary = new Dictionary<int, CHCNetSDK.NET_DVR_IPCHANINFO>();

            nvrOffLineState = new NVROffLineState(this); //NVR设备离线状态
            nvrRefreshChannelFailState = new NVRRefreshChannelFailState(this);//NVR设备刷新通道失败状态
            nvrRefreshChannelSuccessState = new NVRRefreshChannelSuccessState(this);//NVR设备刷新通道成功状态
            NvrState = nvrOffLineState;  //NVR设备当前状态

            nvrPingSender = new Ping();
            waiter = new System.Threading.AutoResetEvent(false);
            nvrPingSender.PingCompleted += new PingCompletedEventHandler(PingCompletedCallback);
            options = new PingOptions(64, true);

        }
        #endregion 2.2有参构造函数

        #region 2.3海康SDK初始化
        private Boolean HK_Nvr_Init()
        {
            if (nvrInitSDK == true)
                return false;

            if ((nvrInitSDK = CHCNetSDK.NET_DVR_Init()) == false)
                return false;
            else
            {
                nvrChanInfoDictionary = new Dictionary<int, CHCNetSDK.NET_DVR_IPCHANINFO>();
                CHCNetSDK.NET_DVR_SetLogToFile(3, nvrLogPath, true);//日志保存
                return true;
            }
        }
        #endregion 2.3海康SDK初始化

        #endregion 2.构造函数

        #region 3.公有方法

        #region 3.1登录操作

        #region 3.1.1登录
        public Boolean Login()
        {
            Console.WriteLine("尝试登录");
            nvrUserHandle = CHCNetSDK.NET_DVR_Login_V30(this.NvrIP, this.NvrPort,
                this.NvrUserName, this.NvrPassword, ref this.nvrDeviceInfo);

            if (nvrUserHandle < 0)
            {
                Console.WriteLine("登录失败");
                nvrLastErr = CHCNetSDK.NET_DVR_GetLastError();
                nvrLastErrString = "登录NVR设备失败";
                nvrStatus = NVRStatus.Offline;
                nvrState = nvrOffLineState;
                return false;
            }
            else
            {
                nvrState = nvrOnLineState;
                nvrLastErr = CHCNetSDK.NET_DVR_GetLastError();
                nvrLastErrString = "";
                return true;
                //if (RefreshChannelInfo())
                //{
                //    if (nvrDeviceInfo.byIPChanNum > 0)
                //    {
                //        Console.WriteLine("登录成功");
                //        nvrStatus = NVRStatus.Online;
                //        return true;
                //    }
                //    else
                //        return false;
                //}
                //else
                //    return false;
            }
        }
        #endregion 3.1.1登录

        #region 3.1.2 注销登录
        public void LoginOut()
        {
            if (nvrUserHandle >= 0)
            {
                nvrChanInfoDictionary.Clear();
                CHCNetSDK.NET_DVR_Logout(nvrUserHandle);
                nvrUserHandle = -1;
            }
        }
        #endregion 3.1.2注销登录

        #endregion 3.1登录操作

        #region 3.2 通道信息操作

        #region 3.2.1 更新通道信息
        public Boolean RefreshChannelInfo()
        {
            if (nvrUserHandle < 0)
            {
                Console.WriteLine("刷新通道信息中登录失败");
                nvrLastErr = CHCNetSDK.NET_DVR_GetLastError();
                nvrLastErrString = "登录NVR设备失败";
                nvrStatus = NVRStatus.Offline;
                nvrState = nvrOffLineState;
                return false;
            }

            uint dwSize = (uint)Marshal.SizeOf(nvrStruIpParaCfg);
            IntPtr ptrIpParaCfgV40 = Marshal.AllocHGlobal((Int32)dwSize);
            Marshal.StructureToPtr(nvrStruIpParaCfg, ptrIpParaCfgV40, false);
            uint dwReturn = 0;

            if (!CHCNetSDK.NET_DVR_GetDVRConfig(nvrUserHandle, CHCNetSDK.NET_DVR_GET_IPPARACFG_V40, -1,
                ptrIpParaCfgV40, dwSize, ref dwReturn))
            {
                nvrLastErr = CHCNetSDK.NET_DVR_GetLastError();
                nvrLastErrString = "获取NVR设备通道信息失败";
                nvrState = nvrRefreshChannelFailState;
                return false;
            }
            else
            {
                nvrStruIpParaCfg = (CHCNetSDK.NET_DVR_IPPARACFG_V40)
                    Marshal.PtrToStructure(ptrIpParaCfgV40, typeof(CHCNetSDK.NET_DVR_IPPARACFG_V40));

                this.nvrChanInfoDictionary.Clear();
                byte byStreamType;
                for (int i = 0; i < nvrStruIpParaCfg.dwDChanNum; i++)
                {
                    byStreamType = nvrStruIpParaCfg.struStreamMode[i].byGetStreamType;
                    switch (byStreamType)
                    {
                        case 0:
                            dwSize = (uint)Marshal.SizeOf(this.nvrChannelStruStreamMode);
                            IntPtr ptrChanInfo = Marshal.AllocHGlobal((Int32)dwSize);
                            Marshal.StructureToPtr(nvrStruIpParaCfg.struStreamMode[i].uGetStream, ptrChanInfo, false);
                            CHCNetSDK.NET_DVR_IPCHANINFO m_struChanInfo = (CHCNetSDK.NET_DVR_IPCHANINFO)Marshal.PtrToStructure
                                (ptrChanInfo, typeof(CHCNetSDK.NET_DVR_IPCHANINFO));
                            this.nvrChanInfoDictionary.Add(i + (int)nvrStruIpParaCfg.dwStartDChan, m_struChanInfo);
                            this.NvrChannels[i].ChannelIndex = i;
                            this.NvrChannels[i].ChannelName = "摄像头 - " + i;
                            this.NvrChannels[i].ChannelStatus = (m_struChanInfo.byIPID == 0) ?
                                IPCStatus.Free : (m_struChanInfo.byEnable == 0) ? IPCStatus.Offline : IPCStatus.Online;
                            break;

                        default:
                            break;
                    }
                }
                nvrState = nvrRefreshChannelSuccessState;
                return true;
            }
        }
        #endregion  3.2.1更新通道信息

        #region 3.2.2 获取通道信息字符串数组
        public string[][] GetChannelInfo()
        {
            string[][] ChannelInfoList = new string[this.NvrChannels.Length][];
            for (int i = 0; i != this.NvrChannels.Length; i++)
            {
                ChannelInfoList[i] = new string[] { NvrChannels[i].ChannelName, NvrChannels[i].ChannelStatusString };
            }
            return ChannelInfoList;
        }
        #endregion 3.2.2 获取通道信息字符串数组

        #endregion 3.2通道信息操作

        #region 3.3.1 启动实时预览
        public bool ChannelRealPlay(int channelSelectIndex)
        {
            if (nvrUserHandle < 0)
            {
                nvrLastErr = CHCNetSDK.NET_DVR_GetLastError();
                nvrLastErrString = "未登录NVR设备";
                nvrState = nvrChannelRealPlayFalseState;
                nvrState.NVRChannelRealPlay(channelSelectIndex);
                return false;
            }

            if (channelSelectIndex < 0 || channelSelectIndex > this.nvrMaxChannelNum - 1)
            {
                nvrLastErr = CHCNetSDK.NET_DVR_GetLastError();
                nvrLastErrString = "选择通道错误";
                return false;
            }

            CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = new CHCNetSDK.NET_DVR_PREVIEWINFO();
            lpPreviewInfo.lChannel = nvrChanInfoDictionary.Keys.ElementAt(channelSelectIndex);//预览的设备通道
            lpPreviewInfo.dwStreamType = 0;
            lpPreviewInfo.dwLinkMode = 0;
            lpPreviewInfo.bBlocked = true; 
            IntPtr pUser = new IntPtr();
            nvrChannels[channelSelectIndex].ChannelRealPlayHandle
                = CHCNetSDK.NET_DVR_RealPlay_V40(nvrUserHandle, ref lpPreviewInfo, null, pUser);
            if (nvrChannels[channelSelectIndex].ChannelRealPlayHandle  < 0)
            {
                nvrLastErr = CHCNetSDK.NET_DVR_GetLastError();
                nvrLastErrString = "预览失败";
                return false;
            }
            else
                return true;
        }
        #endregion 3.3.1启动实时预览

        #region 3.3.2 关闭实时预览
        public Boolean StopChannelRealPlay(int channelSelectIndex)
        {
            if (channelSelectIndex < 0 || channelSelectIndex > this.nvrMaxChannelNum - 1)
            {
                nvrLastErr = CHCNetSDK.NET_DVR_GetLastError();
                nvrLastErrString = "选择通道错误";
                return false;
            }
            Int32 channelRealHanle = nvrChannels[channelSelectIndex].ChannelRealPlayHandle;
            if (!CHCNetSDK.NET_DVR_StopRealPlay(channelRealHanle))
            {
                nvrLastErr = CHCNetSDK.NET_DVR_GetLastError();
                nvrLastErrString = "关闭实时预览失败";
                return false;
            }
            return true;
        }
        #endregion 3.3.2 关闭实时预览

        #region 3.3.3 开始录像
        public Boolean Record(int channelSelectIndex, string fullFileName)
        {
            if(channelSelectIndex < 0 || channelSelectIndex > this.nvrMaxChannelNum - 1)
            {
                nvrLastErr = CHCNetSDK.NET_DVR_GetLastError();
                nvrLastErrString = "选择通道错误";
                return false;
            }
            Int32 channelRealPlayHandle  = this.nvrChannels[channelSelectIndex].ChannelRealPlayHandle;
            int lChannel = nvrChanInfoDictionary.Keys.ElementAt(channelSelectIndex); //通道号
            if(!CHCNetSDK.NET_DVR_MakeKeyFrame(nvrUserHandle, lChannel))
            {
                nvrLastErr = CHCNetSDK.NET_DVR_GetLastError();
                nvrLastErrString = "设置固定祯失败";
                return false;
            }
            if (!CHCNetSDK.NET_DVR_SaveRealData(channelRealPlayHandle, fullFileName))
            {
                nvrLastErr = CHCNetSDK.NET_DVR_GetLastError();
                nvrLastErrString = "开始录像失败";
                return false;
            }
            return true;
        }
        #endregion 3.3.3 开始录像

        #region 3.3.4 停止录像
        public Boolean RecordStop(int channelSelectIndex)
        {
            if(channelSelectIndex < 0 || channelSelectIndex > this.nvrMaxChannelNum - 1)
            {
                nvrLastErr = CHCNetSDK.NET_DVR_GetLastError();
                nvrLastErrString = "选择通道错误";
                return false;
            }
            Int32 ChannelRealPlayHandle = this.nvrChannels[channelSelectIndex].ChannelRealPlayHandle;
            if (!CHCNetSDK.NET_DVR_StopSaveRealData(ChannelRealPlayHandle))
                return false;
            return true;
        }
        #endregion 3.3.4 停止录像

        #region 3.3.5 状态信息操作

        #region 3.3.5.1 获取上一次信息错误代码
        public uint GetLastError()
        {
            return CHCNetSDK.NET_DVR_GetLastError();
        }
        #endregion 3.3.5.2 获取上一次信息错误代码

        #region 3.3.5.2 获取上一次错误信息字符串
        public string GetLastErrorString()
        {
            return nvrLastErrString;
        }
        #endregion 3.3.5.2 获取上一次错误信息字符串

        #region 3.3.5.3 改变状态
        public void ChangeNVRStatus()
        {

            nvrState.Dispense();
        }


        #endregion 3.3.5.3 改变状态

        #endregion 3.3.5 状态信息操作

        #region 3.6 获取保存文件操作

        #region 3.6.1 获取保存文件信息
        public List<NvrSearchFileStruct> NvrSearchFileInfo(int channelSelectIndex, byte type, int startYear, int startMonth,
            int startDay, int startHour, int startMinute, int startSecond, int endYear, int endMonth, int endDay,
            int endHour, int endMinute, int endSecond)
        {
            if (channelSelectIndex < 0 || channelSelectIndex > 31)
            {
                nvrLastErrString = "错误的通道号";
                nvrLastErr = CHCNetSDK.NET_DVR_GetLastError();
                return null;
            }

            if (nvrUserHandle < 0)
            {
                nvrLastErrString = "未登录NVR设备";
                nvrLastErr = CHCNetSDK.NET_DVR_GetLastError();
                return null;
            }

            string str1, str2, str3;
            List<NvrSearchFileStruct> nvrSearchFileList = new List<NvrSearchFileStruct>();//存储搜索到的文件信息列表
            CHCNetSDK.NET_DVR_FILECOND_V40 struFileCond_V40 = new CHCNetSDK.NET_DVR_FILECOND_V40();

            struFileCond_V40.lChannel = nvrChanInfoDictionary.Keys.ElementAt(channelSelectIndex);  //通道号 Channel number
            struFileCond_V40.dwFileType = 0xff; //0xff-全部，0-定时录像，1-移动侦测，2-报警触发，...
            struFileCond_V40.dwIsLocked = 0xff; //0-未锁定文件，1-锁定文件，0xff表示所有文件（包括锁定和未锁定）
            struFileCond_V40.struStartTime.dwYear = (uint)startYear;
            struFileCond_V40.struStartTime.dwMonth = (uint)startMonth;
            struFileCond_V40.struStartTime.dwDay = (uint)startDay;
            struFileCond_V40.struStartTime.dwHour = (uint)startHour;
            struFileCond_V40.struStartTime.dwMinute = (uint)startMinute;
            struFileCond_V40.struStartTime.dwSecond = (uint)startSecond;
            struFileCond_V40.struStopTime.dwYear = (uint)endYear;
            struFileCond_V40.struStopTime.dwMonth = (uint)endMonth;
            struFileCond_V40.struStopTime.dwDay = (uint)endDay;
            struFileCond_V40.struStopTime.dwHour = (uint)endHour;
            struFileCond_V40.struStopTime.dwMinute = (uint)endMinute;
            struFileCond_V40.struStopTime.dwSecond = (uint)endSecond;

            nvrFindHandle = CHCNetSDK.NET_DVR_FindFile_V40(nvrUserHandle, ref struFileCond_V40);
            if (nvrFindHandle < 0)
            {
                nvrLastErr = CHCNetSDK.NET_DVR_GetLastError();
                nvrLastErrString = "搜索录像文件失败!";
                return nvrSearchFileList;
            }
            else
            {
                CHCNetSDK.NET_DVR_FINDDATA_V30 struFileData = new CHCNetSDK.NET_DVR_FINDDATA_V30(); ;
                while (true)
                {
                    int result = CHCNetSDK.NET_DVR_FindNextFile_V30(nvrFindHandle, ref struFileData);
                    if (result == CHCNetSDK.NET_DVR_ISFINDING)  //正在查找请等待 Searching, please wait
                    {
                        continue;
                    }
                    else if (result == CHCNetSDK.NET_DVR_FILE_SUCCESS) //获取文件信息成功 Get the file information successfully
                    {
                        str1 = struFileData.sFileName;
                        str2 = Convert.ToString(struFileData.struStartTime.dwYear) + "-" +
                            Convert.ToString(struFileData.struStartTime.dwMonth) + "-" +
                            Convert.ToString(struFileData.struStartTime.dwDay) + " " +
                            Convert.ToString(struFileData.struStartTime.dwHour) + ":" +
                            Convert.ToString(struFileData.struStartTime.dwMinute) + ":" +
                            Convert.ToString(struFileData.struStartTime.dwSecond);
                        str3 = Convert.ToString(struFileData.struStopTime.dwYear) + "-" +
                            Convert.ToString(struFileData.struStopTime.dwMonth) + "-" +
                            Convert.ToString(struFileData.struStopTime.dwDay) + " " +
                            Convert.ToString(struFileData.struStopTime.dwHour) + ":" +
                            Convert.ToString(struFileData.struStopTime.dwMinute) + ":" +
                            Convert.ToString(struFileData.struStopTime.dwSecond);

                        nvrSearchFileList.Add(new NvrSearchFileStruct(str1, str2, str3));
                    }
                    else if (result == CHCNetSDK.NET_DVR_FILE_NOFIND || result == CHCNetSDK.NET_DVR_NOMOREFILE)
                    {
                        break; //未查找到文件或者查找结束，退出
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return nvrSearchFileList;
        }
        #endregion 3.6.1 获取保存文件信息

        #region 3.6.2 按文件名下载文件
        public Boolean NvrDownLoadFile(string remoteVideoFileName, string localSaveVideoFileName)
        {
            if (nvrUserHandle < 0)
            {
                nvrLastErr = CHCNetSDK.NET_DVR_GetLastError();
                nvrLastErrString = "未登录NVR设备";
                return false;
            }

            if (nvrDownHandle >= 0)
            {
                nvrLastErr = CHCNetSDK.NET_DVR_GetLastError();
                nvrLastErrString = "正在下载文件";
                return false;
            }

            //如果没指定保存文件名，则按默认的路径文件名保存
            string sVideoFileName = (localSaveVideoFileName.Length == 0 ?
                "Downtest_" + remoteVideoFileName + ".mp4" : localSaveVideoFileName);

            nvrDownHandle = CHCNetSDK.NET_DVR_GetFileByName(nvrUserHandle, remoteVideoFileName, sVideoFileName);
            if (nvrDownHandle < 0)
            {
                nvrLastErr = CHCNetSDK.NET_DVR_GetLastError();
                nvrLastErrString = "下载文件失败";
                return false;
            }

            uint iOutValue = 0;
            if (!CHCNetSDK.NET_DVR_PlayBackControl_V40(nvrDownHandle, CHCNetSDK.NET_DVR_PLAYSTART,
                IntPtr.Zero, 0, IntPtr.Zero, ref iOutValue))
            {
                nvrLastErr = CHCNetSDK.NET_DVR_GetLastError();
                nvrLastErrString = "下载控制失败";
                return false;
            }
            else
                return true;
        }
        #endregion 3.6 按文件名下载文件

        #region 3.6.3 停止下载文件
        public Boolean NvrStopDownLoad()
        {
            if (nvrDownHandle >= 0)
            {
                if (!CHCNetSDK.NET_DVR_StopGetFile(nvrDownHandle))
                {



                    nvrLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    nvrLastErrString = "停止下载失败";
                    return false;
                }
            }
            nvrDownHandle = -1;
            return true;
        }

        #endregion 3.6.3 停止下载文件

        #region 3.6.4 获取下载进度
        public int NvrGetDownLoadPos()
        {
            int iPos = 0;
            iPos = CHCNetSDK.NET_DVR_GetDownloadPos(nvrDownHandle);
            return iPos;
        }
        #endregion 3.6.4 获取下载进度

        #region 3.6.5 按时间条件下载
        public Boolean NvrDownLoadFileByTime(int selectChannelIndex, int startYear, int startMonth,
            int startDay, int startHour, int startMinute, int startSecond, int endYear, int endMonth, int endDay,
            int endHour, int endMinute, int endSecond, string localSaveVideoFileName)
        {

            if (nvrUserHandle < 0)
            {
                Console.WriteLine("按时间条件下载登录失败");
                nvrLastErr = CHCNetSDK.NET_DVR_GetLastError();
                nvrLastErrString = "登录NVR设备失败";
                nvrStatus = NVRStatus.Offline;
                nvrState = nvrOffLineState;
                return false;
            }

            if (nvrDownHandle >= 0)
            {
                nvrLastErr = CHCNetSDK.NET_DVR_GetLastError();
                nvrLastErrString = "正在下载文件，请先停止下载";
                return false;
            }

            CHCNetSDK.NET_DVR_PLAYCOND struDownPara = new CHCNetSDK.NET_DVR_PLAYCOND();
            struDownPara.dwChannel = (uint)nvrChanInfoDictionary.Keys.ElementAt(selectChannelIndex);

            //设置录像查找的开始时间
            struDownPara.struStartTime.dwYear = (uint)startYear;
            struDownPara.struStartTime.dwMonth = (uint)startMonth;
            struDownPara.struStartTime.dwDay = (uint)startDay;
            struDownPara.struStartTime.dwHour = (uint)startHour;
            struDownPara.struStartTime.dwMinute = (uint)startMinute;
            struDownPara.struStartTime.dwSecond = (uint)startSecond;

            //设置录像查找的结束时间
            struDownPara.struStopTime.dwYear = (uint)endYear;
            struDownPara.struStopTime.dwMonth = (uint)endMonth;
            struDownPara.struStopTime.dwDay = (uint)endDay;
            struDownPara.struStopTime.dwHour = (uint)endHour;
            struDownPara.struStopTime.dwMinute = (uint)endMinute;
            struDownPara.struStopTime.dwSecond = (uint)endSecond;

            string sVideoFileName = (localSaveVideoFileName.Length == 0 ?
            "D:/Downtest_Channel" + struDownPara.dwChannel + ".mp4" : localSaveVideoFileName);

            nvrDownHandle = CHCNetSDK.NET_DVR_GetFileByTime_V40(nvrUserHandle, sVideoFileName, ref struDownPara);
            if (nvrDownHandle < 0)
            {
                nvrLastErr = CHCNetSDK.NET_DVR_GetLastError();
                nvrLastErrString = "按照时间下载视频文件失败";
                return false;
            }

            uint iOutValue = 0;
            if (!CHCNetSDK.NET_DVR_PlayBackControl_V40(nvrDownHandle, CHCNetSDK.NET_DVR_PLAYSTART, IntPtr.Zero, 0, IntPtr.Zero, ref iOutValue))
            {
                nvrLastErr = CHCNetSDK.NET_DVR_GetLastError();
                nvrLastErrString = "下载控制失败"; //下载控制失败，输出错误号
                if (NvrStopDownLoad())
                {
                }
                return false;
            }

            return true;
        }
        #endregion 3.6.5 按时间条件下载

        #endregion  3.6  获取保存文件操作


        private void CheckNvrPingStatus(object sender, ElapsedEventArgs e)
        {
            //Console.WriteLine("检查NVR正在使用的错误状态");
            lock (CheckNvrErrObject)
            {
                if (checkNvrErroFlag == 0)
                    checkNvrErroFlag = 1;
                else
                    return;
            }

            string data = "are you there";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;
            //Console.WriteLine("ping的NVR设备地址" + NvrIP);
            nvrPingSender.SendAsync(NvrIP, timeout, buffer, options, waiter);
            waiter.WaitOne();

            //if(nvrState != null)
            //    nvrState.Dispense();
            //Console.WriteLine("出现的错误" + NvrLastErr);
            //if (NvrLastErr == 7)
            //{
            //    Console.WriteLine("将设备转为离线状态");
            //    NvrState = nvrOffLineState;
            //}
            //if (OldNvrState == NvrState && NvrState == nvrOffLineState)
            //    NVRStateDispense();

            //if (OldNvrState != NvrState)
            //    NVRStateDispense();

            lock (CheckNvrErrObject)
                checkNvrErroFlag = 0;
            
        }

        private void PingCompletedCallback(object sender, PingCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                Console.WriteLine("Ping canceled.");
                ((System.Threading.AutoResetEvent)e.UserState).Set();
            }

            if (e.Error != null)
            {
                Console.WriteLine("Ping failed:");
                Console.WriteLine(e.Error.ToString());
                ((System.Threading.AutoResetEvent)e.UserState).Set();
            }

            PingReply reply = e.Reply;
            //Console.WriteLine(reply.Status);
            if (reply.Status == IPStatus.Success)
            {
                //Console.WriteLine("设备在线");
            }
            else
            {
                NvrState = nvrOffLineState;
                //Console.WriteLine("设备断线中");
            }
            ((System.Threading.AutoResetEvent)e.UserState).Set();
        }


        public void TurnNvrState()
        {
            //Console.WriteLine("更新状态");
            lock (TurnStateObject)
            {
                if (turnNvrStateFlag == 0)
                    turnNvrStateFlag = 1;
                else
                    return;
            }

            if (nvrState != null)
                nvrState.Dispense();
         
            lock (TurnStateObject)
                turnNvrStateFlag = 0;

        }
    
        #endregion 3.公有方法

    }
}