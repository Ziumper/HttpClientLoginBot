using HttpClientLoginBot.Bll.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace HttpClientLoginBot.Bll.Tibia.Exceptions
{
    public class TibiaQuequeProxyEnd : Exception
    {
        private string _message;

        public override string Message { get { return _message; } }
        public string ProxyAddres { get; }

        public TibiaQuequeProxyEnd()
        {
            Initialize();
        }

        public TibiaQuequeProxyEnd(string proxyAddres)
        {
            ProxyAddres = proxyAddres;
        }

        private void Initialize()
        {
            _message = "Tibia login client proxy queque end";
        }
    }
}
