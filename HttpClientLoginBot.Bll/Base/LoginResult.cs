using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace HttpClientLoginBot.Bll.Base
{
    public class LoginResult
    {
        private readonly string _username;
        private readonly string _password;
        private readonly bool _isSucces;
        private readonly HttpResponseMessage _response;

        public LoginResult(string username, string password, bool isSucces, HttpResponseMessage response)
        {
            _username = username;
            _password = password;
            _isSucces = isSucces;
            _response = response;
        }
    }
}
