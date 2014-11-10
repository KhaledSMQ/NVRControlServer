using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ConfigurationPattern;

namespace NVRControlServer.Config
{
    [ConfigurationPattern(TPattern.XML)]
    public class SystemXmlConfig : Configuration
    {
        #region 1.变量属性

        #region 1.1 变量
        private const string xmlConfigFilePath ="./Config/SystemXmlCfg.xml";
        private int randId;
        private string serverListenIp = "";
        private string serverListenPort = "";
        private string serverVideoSavePath = "";
        private string zspNvrName = "";
        private string zspNvrIp = "";
        private string zspNvrPort = "";
        private string zspNvrUser = "";
        private string zspNvrPassword = "";
        private string zspNvrMax = "";
        private string ydpNvrName = "";
        private string ydpNvrIp = "";
        private string ydpNvrPort = "";
        private string ydpNvrUser = "";
        private string ydpNvrPassword = "";
        private string ydpNvrMax = "";
       
        #endregion 1.1 变量

        #region 1.2 属性
        [ConfigurationIgnore]
        [XmlIgnore]
        public int RandId
        {
            get{ return randId; }
            set{ randId = value; }
        }

        public string ServerListenIp
        {
            get { return serverListenIp; }
            set { serverListenIp = value; }
        }

        public string ServerListenPort
        {
            get { return serverListenPort; }
            set { serverListenPort = value; }
        }

        public string ServerVideoSavePath
        {
            get { return serverVideoSavePath; }
            set { serverVideoSavePath = value; }
        }

        public string XmlConfigFilePath
        {
           get { return xmlConfigFilePath; }
        }

        public string ZSPNvrName
        {
            get { return zspNvrName; }
            set { zspNvrName = value; }
        }

        public string ZSPNvrIp
        {
            get { return zspNvrIp; }
            set { this.zspNvrIp = value; }
        }
        public string ZSPNvrPort
        {
            get { return this.zspNvrPort; }
            set { this.zspNvrPort = value; }
        }

        public string ZSPNvrUser
        {
            get { return zspNvrUser; }
            set { zspNvrUser = value; }
        }

        public string ZSPNvrPassword
        {
            get { return zspNvrPassword; }
            set { zspNvrPassword = value; }
        }

        public string ZSPNvrMax
        {
            get { return zspNvrMax; }
            set { zspNvrMax = value; }
        }


        public string YDPNvrName
        {
            get { return ydpNvrName; }
            set { ydpNvrName = value; }
        }

        public string YDPNvrIp
        {
            get { return this.ydpNvrIp; }
            set { this.ydpNvrIp = value; }
        }

        public string YDPNvrPort
        {
            get { return this.ydpNvrPort; }
            set { this.ydpNvrPort = value; }
        }

        public string YDPNvrUser
        {
            get { return ydpNvrUser; }
            set { ydpNvrUser = value; }
        }

        public string YDPNvrPassword
        {
            get { return ydpNvrPassword; }
            set { ydpNvrPassword = value; }
        }

        public string YDPNvrMax
        {
            get { return ydpNvrMax; }
            set { ydpNvrMax = value; }
        }


        #endregion 1.2属性

        #endregion 1.变量属性

        #region 2.构造方法
        #region 2.1 无参构造
        public SystemXmlConfig()
            : base(xmlConfigFilePath)
        {
            Random rand = new Random();
            randId = rand.Next();
        }
        #endregion 2.1 无参构造

        #region 2.2 有参构造
        public SystemXmlConfig(string m_xmlConfigXmlFilePath)
            : base(m_xmlConfigXmlFilePath)
        {
            Random rand = new Random();
            randId = rand.Next();
        } 

        #endregion 2.2 有参构造

        #endregion 2. 构造方法

        #region 3.私有方法
        #endregion 3.私有方法

        #region 4.共有方法
        #endregion 4. 共有方法



    }
}
