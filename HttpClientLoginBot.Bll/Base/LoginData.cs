using System;
using System.Collections.Generic;
using System.Text;

namespace HttpClientLoginBot.Bll.Base
{
    public interface LoginData
    {
        string Username { get; set; }
        string Password { get; set; }
        string RequestBody { get; }
    }
}
