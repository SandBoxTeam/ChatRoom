using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace Assets.Script.Client
{
    class Server : Socket
    {
        /// <summary>
        /// 服务器IP地址
        /// </summary>
        public IPAddress AddressIP { get; }

        /// <summary>
        /// 服务器端口
        /// </summary>
        public int ServerPort { get; }

        public void Connect()
        {
            Connect(AddressIP, ServerPort);
        }

        public Server(IPAddress addressIP, int serverPort) : base(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        {
            AddressIP = addressIP;
            ServerPort = serverPort;
        }

        public Server(SocketInformation socketInformation) : base(socketInformation)
        {
        }

        public Server(SocketType socketType, ProtocolType protocolType) : base(socketType, protocolType)
        {
        }

        public Server(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType) : base(addressFamily, socketType, protocolType)
        {
        }
    }
}
