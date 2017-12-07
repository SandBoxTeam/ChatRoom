using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace Assets.Script.Client
{
    class Client
    {
        /// <summary>
        /// 服务器实例对象
        /// </summary>
        Server _Server;

        /// <summary>
        /// 服务器IP地址
        /// </summary>
        public IPAddress Server_AddressIP { get { return _Server.AddressIP; } }

        /// <summary>
        /// 服务器端口
        /// </summary>
        public int Server_Port { get { return _Server.ServerPort; } }

        public Client(IPAddress addressIP, int serverPort)
        {
            _Server = new Server(addressIP, serverPort);
        }

        public bool ConnectServer()
        {
            _Server.Connect();
        }
    }
}
