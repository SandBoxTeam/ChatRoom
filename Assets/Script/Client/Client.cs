using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

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

        public string ClientName { get { return _ClientName; } }

        public int ClientID { get { return _ClientID; } }

        public List<ClientList> OnlineClientList { get { return _OnlineClientList; } }

        public int OnlineClientCount { get { return _OnlineClientList.Count; } }

        List<ClientList> _OnlineClientList;

        string _ClientName;

        int _ClientID;

        public event ReceiveMessages_EventHandler ReceiveMessages_Event;

        public bool ConnectServer(IPAddress addressIP, int serverPort, string clientName)
        {
            try
            {
                _Server = new Server(addressIP, serverPort);

                _Server.Connect();

                _ClientName = clientName;

                return CheckConnectState();
            }
            catch (Exception ex)
            {
                return false;
            }

            bool CheckConnectState()
            {
                try
                {
                    Data result = _Server.Receive();

                    if (result.HeadInfo == HeadInformation.CheckConnectState)
                    {
                        _Server.Send(new Data(HeadInformation.CheckConnectState, new Message() { ClientName = _ClientName }));

                        result = _Server.Receive();

                        if (result.Data_Message.Sign)
                        {
                            _ClientID = result.Data_Message.ClientID;
                            _OnlineClientList = result.Data_Message.OnlineClientList;

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public bool SendMessages(string msg)
        {
            int result = _Server.Send(new Data(HeadInformation.Message, new Message() { ClientID = ClientID, ClientName = ClientName, ToClientID = 0, Time = DateTime.Now, Msg = msg }));

            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SendMessagesToClientByID(string msg, int toClientID)
        {
            int result = _Server.Send(new Data(HeadInformation.Message, new Message() { ClientID = ClientID, ClientName = ClientName, ToClientID = toClientID, Time = DateTime.Now, Msg = msg }));

            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void StartMessagesReceive()
        {
            new Thread(ReceiveServerMessages).Start();
        }

        void ReceiveServerMessages()
        {
            try
            {
                while (true)
                {
                    Data data = _Server.Receive();

                    // 触发事件
                    ReceiveMessages_Event?.Invoke(data);

                    if (data.HeadInfo == HeadInformation.ClientOnline)
                    {
                        _OnlineClientList.Add(new ClientList() { ClientName = data.Data_Message.ClientName, ClientID = data.Data_Message.ClientID });
                    }
                    else if (data.HeadInfo == HeadInformation.ClientOffline)
                    {
                        _OnlineClientList.Remove(_OnlineClientList.Find(i => i.ClientID == data.Data_Message.ClientID));
                    }
                    else if (data.HeadInfo == HeadInformation.ServerOffline)
                    {
                        DisconConnect();

                        Thread.CurrentThread.Abort();
                    }
                }
            }
            catch (Exception ex)
            {
                Thread.CurrentThread.Abort();
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void DisconConnect()
        {
            SendClientOfflineMessages();
        }

        void SendClientOfflineMessages()
        {
            _Server.Send(new Data(HeadInformation.ClientOffline, new Message() { ClientID = ClientID }));
        }
    }
}