using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace HttpClientLoginBot.Bll.Base {
    public class LoginResult {
        private readonly string _username;
        private readonly string _password;

        private readonly HttpResponseMessage _response;

        public HttpResponseMessage Response {
            get { return _response; }
        }

        public bool IsSucces { get { return _response.IsSuccessStatusCode; } }

        public LoginResult (string username, string password, HttpResponseMessage response) {
            _username = username;
            _password = password;
            _response = response;
        }

        public void Save(string resultFileName)
        {
            if(IsSucces)
            {

            }
        }
    }
}