using System;
using System.Collections.Generic;
using System.Text;

namespace HttpClientLoginBot.Bll.Base
{
    public interface ILoginBot
    {
        LoginResult Login(string url);
        void SaveResult(LoginResult result);
        void LoadCredentials(string seperator);
        void LoadProxyList(string seperator);
    }
}
