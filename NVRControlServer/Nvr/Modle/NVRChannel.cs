#region ************************文件说明************************************
/// 作者(Author)：                             Shunbin Hunag  
/// 
/// 日期(Create Date)：                     2014.7.13
/// 
/// 功能：                                             NVR通道信息类
///
/// 修改记录(Revision History)：     无
///
#endregion *********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVRControlServer.NVR.Control
{
    /// <summary> 摄像头状态枚举类型 </summary>
    public enum IPCStatus : byte
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

    /// <summary>设备通道类</summary>
    public class NVRChannel
    {
        #region 1.变量属性

        #region 1.1变量
        private string channelName;                                 //通道名
        private int channelIndex;                                       //通道数
        private IPCStatus channelStatus;                       //通道状态
        private Int32 channelRealPlayHandle;               //通道实时预览句柄
        #endregion 1.1变量

        #region 1.2属性
        public string ChannelName
        {
            get { return this.channelName; }
            set { this.channelName = value; }
        }

        public int ChannelIndex
        {
            get { return this.channelIndex; }
            set { this.channelIndex = value; }
        }

        ///<summary>获取或设置通道状态</summary>
        public IPCStatus ChannelStatus
        {
            get { return this.channelStatus; }
            set { this.channelStatus = value; }
        }

        public Int32 ChannelRealPlayHandle
        {
            get { return this.channelRealPlayHandle; }
            set { this.channelRealPlayHandle = value; }
        }

        /// <summary>数字通道号：从32开始，其值为ChannelIndex+32</summary>
        public int ChannelNum
        {
            get { return (ChannelIndex != -1) ? ChannelIndex + 32 : -1; }
        }

        /// <summary>获取通道状态字符串</summary>
        public string ChannelStatusString
        {
            get
            {
                switch (this.ChannelStatus)
                {
                    case IPCStatus.Online: return "在线";
                    case IPCStatus.Offline: return "离线";
                    case IPCStatus.Free: return "空闲";
                    default: return null;
                }
            }
        }
        #endregion 1.2索引

        #endregion 1.属性变量

        #region 2.构造函数

        #region 2.1 无参构造
        public NVRChannel()
        {
            this.ChannelIndex = -1;
            this.ChannelName = null;
            this.ChannelStatus = IPCStatus.Null;
        }
        #endregion 2.1 无参构造

        #region 2.2 有参构造
        /// <summary>
        /// 构建一个通道
        /// </summary>
        /// <param name="ChannelName">通道名称</param>
        /// <param name="ChannelIndex">通道索引</param>
        /// <param name="Status">通道状态</param>
        public NVRChannel(string ChannelName, int ChannelIndex, IPCStatus Status)
        {
            this.ChannelName = ChannelName;
            this.ChannelIndex = ChannelIndex;
            this.ChannelStatus = Status;
        }

        /// <summary>
        /// 构建一个通道
        /// </summary>
        /// <param name="ChannelIndex">通道索引</param>
        /// <param name="Status">通道状态</param>
        public NVRChannel(int ChannelIndex, IPCStatus Status)
        {
            this.ChannelIndex = ChannelIndex;
            this.ChannelStatus = Status;
        }
        #endregion 2.2有参构造

        #endregion 2.构造函数

        #region 3.公有方法

        #region 3.1更新通道信息
        /// <summary>
        /// 更新通道信息
        /// </summary>
        /// <param name="ChannelName">通道名称</param>
        /// <param name="ChannelIndex">通道索引</param>
        /// <param name="Status">通道状态</param>
        public void ReflashInfo(string ChannelName, int ChannelIndex, IPCStatus Status)
        {
            this.ChannelName = ChannelName;
            this.ChannelIndex = ChannelIndex;
            this.ChannelStatus = Status;
        }
        #endregion 3.1更新通道信息

        #endregion 3.公有方法
    }
}