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

        public virtual StringContent StringContent { get {
            return new StringContent(RequestBody,Encoding.UTF8,"application/x-www-form-urlencoded");
        }}

        public virtual Uri Uri {get {return new Uri(Url);}}
        

        public LoginCredential(string username, string password){
            Username = username;
            Password = password;
        }

        

    }
}
