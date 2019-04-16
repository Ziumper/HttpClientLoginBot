using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace HttpClientLoginBot.Bll.Base
{
    public class LoginCredential
    {
        public string Username { get; set; }
        public string Password { get ; set; }

        public string RequestBody {get; set;}
        public string Url {get;set;}

        public LoginCredential(string username, string password){
            Username = username;
            Password = password;
        }

        

    }
}
