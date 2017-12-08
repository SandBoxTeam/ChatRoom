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

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="data">消息</param>
        /// <returns>已发送Data字节</returns>
        public int Send(Data data)
        {
            return Send(data.Data_Byte);
        }

        public Data Receive()
        {
            // 总接收量
            int totalResultLength = 0;

            // 接收数据量
            int resultLength;

            // 数据容器
            byte[] totalResult = null;

            // 临时容器
            byte[] tempResult = null;

            // 接收容器
            byte[] result = null;

            do
            {
                result = new byte[1024];

                // 接收数据
                resultLength = Receive(result);

                if (totalResultLength == 0)
                {
                    totalResult = new byte[resultLength];

                    Array.Copy(result, totalResult, resultLength);

                    totalResultLength += resultLength;
                }
                else
                {
                    // 更新总接收量
                    totalResultLength += resultLength;

                    // 使用总接收量创建临时容器
                    tempResult = new byte[totalResultLength];

                    // 将数据容器中的数据放入临时容器
                    Array.ConstrainedCopy(totalResult, 0, tempResult, 0, totalResult.Length);
                    // 将接收容器的数据放入临时容器
                    Array.ConstrainedCopy(result, 0, tempResult, totalResult.Length, resultLength);

                    // 使用总接收量创建临时容器
                    totalResult = new byte[totalResultLength];

                    // 将临时容器的数据放入数据容器
                    totalResult = tempResult;
                }

            } while (resultLength == 1024);

            return new Data(totalResult);
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