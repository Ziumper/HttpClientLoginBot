using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace HttpClientLoginBot.Bll.Base {
    public class LoginResult {
        public string Username { get; set; }
        public string Password { get; set; }
        public HttpResponseMessage Response {get;set;}
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
      
        public LoginResult()
        {
            IsSuccess = false;
        }

        public LoginResult(LoginData loginData)
        {
            Username = loginData.Username;
            Password = loginData.Password;
            IsSuccess = false;
        }

        public void Save(string resultFilePath)
        {
            if(IsSuccess)
            {
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(resultFilePath), true))
                {
                    outputFile.WriteLine(Username + ":" + Password);
                }
            }
        }

        
    }
}