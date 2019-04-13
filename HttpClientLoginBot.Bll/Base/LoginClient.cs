using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace HttpClientLoginBot.Bll.Base
{
    public abstract class LoginClient : IDisposable,ILoginClient
    {
        private readonly HttpClient _client;
        private readonly List<LoginProxy> _proxyList;
        private readonly LoginCredential _loginCredential;

        public LoginClient(List<LoginProxy> proxyList,LoginCredential loginCredential)
        {
            _proxyList = proxyList;
            _client = new HttpClient();
            _loginCredential = loginCredential;
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        public LoginResult Login(string url)
        {

            throw new NotImplementedException();
        }
    }
}
