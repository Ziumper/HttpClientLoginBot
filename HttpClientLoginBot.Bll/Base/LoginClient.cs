using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientLoginBot.Bll.Base
{
    public class LoginClient
    {
        public LoginProxy ActiveProxy { get; set; }
        public string Url { get; set; }
        public string MediaType { get; set; }  //"application/x-www-form-urlencoded
        public Uri Uri { get { return new Uri(Url); } }
        public Encoding Encoding { get; set; }

        public LoginClient(string url, string requestBody,string mediaType,Encoding encoding)
        {
            Url = url;
            MediaType = mediaType;
            Encoding = encoding;
        }

        public LoginClient(string url)
        {
            Url = url;
            MediaType = "application/x-www-form-urlencoded";
            Encoding = Encoding.UTF8;
        }

        public virtual async Task<LoginResult> Login(LoginData loginData)
        {
            HttpClient httpClient = null;

            if (ActiveProxy != null)
            {
                var httpHandler = new HttpClientHandler();
                httpHandler.Proxy = ActiveProxy.WebProxy;
                httpHandler.UseProxy = true;
                httpClient = new HttpClient(httpHandler);
            } else
            {
                httpClient = new HttpClient();
            }

            try
            {
                StringContent stringContent = new StringContent(loginData.RequestBody, Encoding, MediaType);
                var response = await httpClient.PostAsync(Uri, stringContent);

                LoginResult result = new LoginResult();
                result.Response = response;
                result.Username = loginData.Username;
                result.Passwrod = loginData.Password;
                result.IsFinished = true;
                if (response.IsSuccessStatusCode)
                {
                    result.IsSucces = true;
                }

                return result;
            }
            finally
            {
                httpClient.Dispose();
            }

            
        }

        
    }
}
