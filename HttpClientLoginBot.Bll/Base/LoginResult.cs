using System;
using System.Collections.Generic;
using System.Text;

namespace HttpClientLoginBot.Bll.Base
{
    public class LoginResult
    {
        private readonly string _username;
        private readonly string _password;
        private readonly bool _isSucces;

        public LoginResult(string username, string password, bool isSucces)
        {
            _username = username;
            _password = password;
            _isSucces = isSucces;
        }
    }
}
