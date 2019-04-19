using HttpClientLoginBot.Bll.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace HttpClientLoginBot.Bll.Tibia
{
    public class TibiaLoginData : LoginData
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string RequestBody { get => "loginname=" + Username + "&loginpassword=" + Password + "&page=overview&Login.x=0&Login.y=0"; }
    }
}
