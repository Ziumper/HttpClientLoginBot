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
        }

        public async virtual void Run () {
            foreach (var loginData in _loginDataList) {

                var result = new LoginResult();
                while(!result.IsFinished)
                {
                    result = await _loginClient.Login(loginData);
                }
                result.Save(_resultFileName);
            }

        }
    }
}