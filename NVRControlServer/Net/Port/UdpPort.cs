using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using NVRControlServer.Net.Port;

namespace NVRControlServer.Net.Port
{
    public class UdpLanPort : LanPort
    {
        private byte[] receiveData = new byte[1024];

        public override bool Initialization(string ip, int port)
        {
            mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //服务器端的IP和端口，IPAddress.Any实际是：0.0.0.0，表示任意，基本上表示本机IP 
            IPEndPoint server = new IPEndPoint(IPAddress.Any, 11000);
            //Socket对象跟服务器端的IP和端口绑定
            mSocket.Bind(server);
            //客户端的IP和端口，端口 0 表示任意端口
            IPEndPoint clients = new IPEndPoint(IPAddress.Any, 0);

            EndPoint epSender = (EndPoint)clients;
            //开始异步接收消息  接收后，epSender存储的是发送方的IP和端口 
            mSocket.BeginReceiveFrom(receiveData, 0, receiveData.Length, SocketFlags.None, 
                ref epSender, new AsyncCallback(ReceiveData), epSender); 
            Console.WriteLine("Listening..."); 
            Console.ReadLine();
            return true;
        }


        private void ReceiveData(IAsyncResult iar)
        {
            IPEndPoint client = new IPEndPoint(IPAddress.Any, 0);
            EndPoint epSender = (EndPoint)client;
            int recv = mSocket.EndReceiveFrom(iar, ref epSender);
            Console.WriteLine("Client:" + Encoding.ASCII.GetString(receiveData, 0, recv));
            byte[] sendData = Encoding.ASCII.GetBytes("hello");

            mSocket.BeginSendTo(sendData, 0, sendData.Length, SocketFlags.None, 
                epSender, new AsyncCallback(SendData), epSender);

            receiveData = new byte[1024];

            mSocket.BeginReceiveFrom(receiveData, 0, receiveData.Length, SocketFlags.None, 
               ref epSender, new AsyncCallback(ReceiveData), epSender); 

        }

        private void SendData(IAsyncResult iar)
        {
            mSocket.EndSend(iar);
        }

    }
}
