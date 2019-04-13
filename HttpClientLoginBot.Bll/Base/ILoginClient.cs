using System;
using System.Collections.Generic;
using System.Text;

namespace HttpClientLoginBot.Bll.Base
{
    public interface ILoginClient
    {
        LoginResult Login(string url);
    }
}
