using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace HttpClientLoginBot.Bll.Base
{
    public abstract class LoginClient : IDisposable
    {
        private readonly HttpClient _client;
        private readonly List<LoginProxy> _proxyList;

        public LoginClient(List<LoginProxy> proxyList)
        {
            _proxyList = proxyList;
            _client = new HttpClient();
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        public LoginResult Login(string url,LoginCredential credential)
        {
            throw new NotImplementedException();
        }
    }
}
