using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace HttpClientLoginBot.Bll.Base
{
    public class LoginCredential
    {
        protected readonly string _username;
        protected readonly string _password;

        public string Username { get { return _username; } }
        public string Password { get { return _password; } }
        public bool UseProxy { }

        public LoginCredential(string username, string password){
            _username = username;
            _password = password;
        }

        

    }
}
