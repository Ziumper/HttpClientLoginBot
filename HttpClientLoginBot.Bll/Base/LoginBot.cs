using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HttpClientLoginBot.Bll.Base {
    public abstract class LoginBot : ILoginBot {
        protected readonly ILoginClient<LoginResult> _loginClient;
        protected List<LoginData> _loginDataList;
        protected string _resultFileName;
        protected ProxyList _proxyList;

        public bool UseProxy { get; set; }

        public LoginBot(
            string resultFileName,
            ILoginClient<LoginResult> loginClient,
            List<LoginData> credentials,
            ProxyList proxyList
        ) {
            _resultFileName = resultFileName;
            _loginClient = loginClient;
            _loginDataList = credentials;
            _proxyList = proxyList;
            UseProxy = false;
        }

        public async virtual void Run () {
            foreach (var loginData in _loginDataList) {

                var result = new LoginResult();
                while(!result.IsFinished)
                {
                    if (UseProxy)
                    {
                        SetProxy();
                    } else
                    {
                        _loginClient.ActiveProxy = null;
                    }
                    result = await _loginClient.Login(loginData);
                }
                result.Save(_resultFileName);
            }

        }

        protected void SetProxy()
        {
            if (!_proxyList.IsEndOfProxyList)
            {
                InitalizeProxy();
            } else
            {
                UnsetProxy();
            }
            
        }

        private void UnsetProxy()
        {
            _loginClient.ActiveProxy = null;
        }

        private void InitalizeProxy()
        {
            var isLoginClientProxySet = _loginClient.ActiveProxy != null;
            if (!isLoginClientProxySet)
            {
                SetCurrentProxy();
            }
            else
            {
                SetNextProxy();
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