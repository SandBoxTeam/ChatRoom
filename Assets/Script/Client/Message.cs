using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Client
{
    class Message
    {
        public int ClientID { get; set; }

        public string ClientName { get; set; }

        public int ToClientID { get; set; }

        public bool Sign { get; set; }

        public List<ClientList> OnlineClientList { get; set; }

        public DateTime Time { get; set; }

        public string Msg { get; set; }
    }
}