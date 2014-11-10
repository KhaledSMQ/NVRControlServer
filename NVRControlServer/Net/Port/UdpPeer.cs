using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using NVRControlServer.Net.Model;
using System.Net;

namespace NVRControlServer.Net.Port
{
    public class UdpPeer : IDisposable
    {
        private UdpClient udpClient;
        private int udpPort = 8889;
        private bool udpStarted;

        public event ReceiveDataEventHandler ReceiveData;

        public UdpClient UdpClient
        {
            get
            {
                if (udpClient == null)
                {
                    bool success = false;
                    while (!success)
                    {
                        try
                        {
                            udpClient = new UdpClient(udpPort);
                            success = true;
                        }
                        catch(SocketException ex)
                        {
                            udpPort++;
                            if (udpPort > 66535)
                            {
                                success = true;
                                throw ex;
                            }
                        }
                    }
                    uint IOC_IN = 0x80000000;
                    uint IOC_VENDOR = 0x18000000;
                    uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
                    udpClient.Client.IOControl(
                            (int)SIO_UDP_CONNRESET,
                            new byte[] { Convert.ToByte(false) },
                            null);
                }
                return udpClient;
            }
        }

        public int UdpPort
        {
            get { return udpPort; }
            set { udpPort = value; }
        }

        public bool UdpStarted
        {
            get { return udpStarted; }
            set { udpStarted = value; }
        }

        public UdpPeer(int udpPort)
        {
            this.udpPort = udpPort;
        }

        public void Start()
        {
            if (!udpStarted)
            {
                udpStarted = true;
                ReceiveInternal();
            }
        }

        public void Send(IDataCell cell, IPEndPoint remoteIp)
        {
            byte[] buffer = cell.ToBuffer();
            SendInternal(buffer, remoteIp);
        }

        protected void SendInternal(byte[] buffer, IPEndPoint remoteIp)
        {
            if (!udpStarted)
            {
                throw new ApplicationException("UDP Closed");
            }
            try
            {
                UdpClient.BeginSend(
                                buffer,
                                buffer.Length,
                                remoteIp,
                                new AsyncCallback(SendCallBack),
                                null);
            }
            catch (SocketException ex)
            {
                throw ex;
            }
        }

        private void SendCallBack(IAsyncResult result)
        {
            try
            {
                UdpClient.EndSend(result);
            }
            catch (SocketException ex)
            {
                throw ex;
            }
        }

        protected void ReceiveInternal()
        {
            if (!udpStarted)
            {
                return;
            }
            try
            {
                UdpClient.BeginReceive(
                        new AsyncCallback(ReceiveCallBack),
                        null);
             }
            catch (SocketException ex)
            {
                throw ex;
            }
        }


        private void ReceiveCallBack(IAsyncResult result)
        {
            if (!udpStarted)
            {
                return;
            }
            IPEndPoint remoteIp = new IPEndPoint(IPAddress.Any, 0);
            byte[] buffer = null;
            try
            {
                buffer = UdpClient.EndReceive(result, ref remoteIp);
            }
            catch (SocketException ex)
            {
                throw ex;
            }
            finally
            {
                ReceiveInternal();
            }
            OnReceiveData(new ReceiveDataEventArgs(buffer, remoteIp));
        }


        protected virtual void OnReceiveData(ReceiveDataEventArgs e)
        {
            if (ReceiveData != null)
            {
                ReceiveData(this, e);
            }
        }

        public void Dispose()
        {
            udpStarted = false;
            if (udpClient != null)
            {
                udpClient.Close();
                udpClient = null;
            }
        }


    }
}
