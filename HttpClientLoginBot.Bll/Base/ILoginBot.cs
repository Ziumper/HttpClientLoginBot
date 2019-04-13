using System;
using System.Collections.Generic;
using System.Text;

namespace HttpClientLoginBot.Bll.Base
{
    public interface ILoginBot
    {
        void LoadCredentials(string seperator);
        void LoadProxyList(string seperator);
        void Run();
    }
}
