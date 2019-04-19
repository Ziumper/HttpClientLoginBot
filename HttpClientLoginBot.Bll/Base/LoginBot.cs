using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HttpClientLoginBot.Bll.Base {
    public class LoginBot : ILoginBot {
        protected readonly string _pathToCredentials;
        protected readonly string _pathoToProxyFileList;
        protected readonly ILoginClient _loginClient;
        protected List<LoginData> _loginData;
        protected string _resultFileName;
        protected ProxyList _proxyList;

        public bool UseProxy { get; set; }

        public LoginBot(
            string pathToCredentials,
            string pathToProxyFileList,
            string resultFileName,
            ILoginClient loginClient,
            List<LoginData> credentials,
            ProxyList proxyList
        ) {
            _pathoToProxyFileList = pathToProxyFileList;
            _pathToCredentials = pathToCredentials;
            _resultFileName = resultFileName;
            
            _loginClient = loginClient;
            _loginData = credentials;
            _proxyList = proxyList;

            UseProxy = true;
        }

        public async void Run () {
            foreach (var loginData in _loginData) {

                var result = new LoginResult();
                while(!result.IsFinished)
                {
                    if (UseProxy)
                    {
                        SetProxy();
                    }
                    result = await _loginClient.Login(loginData);
                }
                result.Save(_resultFileName);
            }

        }

        private void SetProxy()
        {
            if (_loginClient.ActiveProxy == null)
            {
                _loginClient.ActiveProxy = _proxyList.CurrentProxy;
            }

            if (_loginClient.ActiveProxy != null)
            {
                SetNextProxy();
            }
            
        }

        private void SetNextProxy()
        {
            _loginClient.ActiveProxy = _proxyList.NextProxy;
        }

       
        
    }
}