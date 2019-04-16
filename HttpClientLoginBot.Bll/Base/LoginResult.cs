using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace HttpClientLoginBot.Bll.Base {
    public class LoginResult {
        public string Username { get; set; }
        public string Passwrod { get; set; }
        public HttpResponseMessage Response {get;set;}
        public bool IsSucces { get; set; }

        public LoginResult()
        {
            IsSucces = false;
        }

        public void Save(string resultFileName)
        {
            if(IsSucces)
            {
                //TODO Save file with result;
            }
        }

        
    }
}