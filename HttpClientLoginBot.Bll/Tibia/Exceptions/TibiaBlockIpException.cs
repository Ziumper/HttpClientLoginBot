using HttpClientLoginBot.Bll.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace HttpClientLoginBot.Bll.Tibia.Exceptions
{
    public class TibiaBlockIpException : Exception
    {
        private string _message;

        public override string Message { get { return _message; } }
        public string ProxyAddres { get; }

        public TibiaBlockIpException()
        {
            Initialize();
        }

        public TibiaBlockIpException(string proxyAddres)
        {
            ProxyAddres = proxyAddres;
        }

        private void Initialize()
        {
            _message = "Tibia block ip exception occured";
        }
    }
}
