using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace HttpClientLoginBot.Bll.Base
{
    public class LoginProxy
    {
        protected string _host;
        protected DateTime _lastTimeLogged;
        protected bool _isActive;
        protected int _port;
        protected readonly WebProxy _webProxy;

        public WebProxy WebProxy { get { return _webProxy; } }
        public bool IsActive { get { return _isActive; } }
        public string FullAddres { get { return string.Concat(_host, ":", _port); } }

        public LoginProxy(string host,string port)
        {
            _host = host;
            _port = int.Parse(port);
            var uriBulider = new UriBuilder();
            uriBulider.Host = host;
            uriBulider.Port = _port;
            _webProxy = new WebProxy();
            _webProxy.Address = uriBulider.Uri;
            _isActive = true;
        }

        

    }
}
