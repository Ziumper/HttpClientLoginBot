using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace HttpClientLoginBot.Bll.Base {
    public class LoginResult {
        public string Username { get; set; }
        public string Passwrod { get; set; }
        public HttpResponseMessage Response {get;set;}
        public bool IsSucces { get; set; }
        public bool IsFinished { get; set; }
      
        public LoginResult()
        {
            IsSucces = false;
            

        }

        public void Save(string resultFilePath)
        {
            if(IsSucces)
            {
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(resultFilePath), true))
                {
                    outputFile.WriteLine(Username + ":" + Passwrod);
                }
            }
        }

        
    }
}