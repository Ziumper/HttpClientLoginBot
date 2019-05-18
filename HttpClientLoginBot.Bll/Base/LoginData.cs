using System;
using System.Collections.Generic;
using System.Text;

namespace HttpClientLoginBot.Bll.Base
{
    public class LoginData
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public virtual string RequestBody { get; }
    }
}
