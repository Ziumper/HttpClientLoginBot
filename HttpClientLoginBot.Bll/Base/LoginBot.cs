using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HttpClientLoginBot.Bll.Base {
    public abstract class LoginBot : ILoginBot {
        protected readonly string _pathToCredentials;
        protected readonly string _pathoToProxyFileList;
        protected readonly ILoginClient _loginClient;
        protected List<LoginCredential> _credentials;
        protected string _resultFileName;
        protected ProxyList _proxyList;

        public bool UseProxy { get; set; }

        public LoginBot(
            string pathToCredentials,
            string pathToProxyFileList,
            string resultFileName,
            string url,
            ILoginClient loginClient,
            List<LoginCredential> credentials,
            ProxyList proxyList
        ) {
            _pathoToProxyFileList = pathToProxyFileList;
            _pathToCredentials = pathToCredentials;
            _resultFileName = resultFileName;
            
            _loginClient = loginClient;
            _credentials = credentials;
            _proxyList = proxyList;

            UseProxy = true;
        }

        public async void Run () {
            foreach (var credential in _credentials) {

                var result = new LoginResult();
                while(!result.IsSucces)
                {
                    if (UseProxy)
                    {
                        SetProxy();
                    }
                    result = await _loginClient.Login(credential);
                }
                result.Save(_resultFileName);
            }

        }

        private void SetProxy()
        {
            if (_loginClient.ActiveProxy != null)
            {
                SetNextProxy();
            }
            if (_loginClient.ActiveProxy == null)
            {
                SetCurrentProxy();
            }
        }

        private void SetNextProxy()
        {
            _loginClient.ActiveProxy = _proxyList.NextProxy;
        }

        private void SetCurrentProxy()
        {
            _loginClient.ActiveProxy = _proxyList.CurrentProxy;
        }

        
    }
}