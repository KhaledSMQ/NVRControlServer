using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;
using NVRControlServer;


namespace NVRControlServer.Net.Model
{

    class Server
    {
        #region 1.变量属性

        #region 1.1 变量
        private Socket clientSocket;
        private Socket serverSocket;
        private TcpPort tcpPort;
        private int serverPort;
        private static int BUFFSIZE = 1024;
        private bool isAccept = false;
        private bool isListenning = false;
        private bool isBusy = false;

        private TcpListener serverTcpListener;

        #endregion 1.1 变量

        #region 1.2 属性
        public bool IsAccept
        {
            get
            {
                return this.isAccept;
            }
            set
            {
                this.isAccept = value;
            }
        }
        public bool IsListening
        {
            get
            {
                return this.isListenning;
            }
            set
            {
                this.isListenning = value;
            }
        }
        public bool IsBusy
        {
            get
            {
                return this.isBusy;
            }
            set
            {
                this.isBusy = value;
            }
        }
        #endregion 1.2 属性

        #endregion 1.变量属性

        public Server()
        {

        }

        public bool Initialization(string serverIp, int serverPort, int listenNum)
        {
            try
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress[] addrIP = Dns.GetHostAddresses(serverIp);
                IPAddress IPAddr = addrIP[0];
                IPEndPoint IPEPoint = new IPEndPoint(IPAddr, serverPort);
                serverSocket.Bind(IPEPoint);
                serverSocket.Listen(listenNum);
                if (serverSocket != null)
                {
                    isAccept = true;
                    return true;
                }
                else
                {
                    isAccept = false;
                    return false;
                }
            }
            catch (SocketException ex)
            {
                return false;
                throw ex;
            }
        }

        public Socket Accept()
        {
            return serverSocket.Accept();
        }

        public void Close()
        {
            serverSocket.Close();
        }

        private void ListenClientConnect()
        {
            TcpClient newClient = null;
            while (true)
            {
                ListenClientDelegate d = new ListenClientDelegate(ListenClient);
                IAsyncResult result = d.BeginInvoke(out newClient, null, null);
                while (result.IsCompleted == false)
                {
                    Thread.Sleep(250);
                }
                d.EndInvoke(out newClient, result);
                if (newClient != null)
                {

                }
            }
        }

        private  delegate void ListenClientDelegate(out TcpClient client);
        private void ListenClient(out TcpClient newClient)
        {
            try
            {
                newClient = serverTcpListener.AcceptTcpClient();
            }
            catch
            {
                newClient = null;
            }
        }
    }
}
