using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientLoginBot.Bll.Base
{
    public class LoginClient : ILoginClient
    {
        private readonly LoginCredential _loginCredential;

        public LoginProxy ActiveProxy { get; set; }

        public LoginClient(LoginCredential loginCredential)
        {
            _loginCredential = loginCredential;
        }

        public async Task<LoginResult> Login(string url,string body)
        {
            var uri = new Uri(url);
            var stringContent = new StringContent(body);
            HttpClient httpClient = null;

            if (ActiveProxy != null)
            {
                var httpHandler = new HttpClientHandler();
                httpHandler.Proxy = ActiveProxy.WebProxy;
                httpClient = new HttpClient(httpHandler);
            } else
            {
                httpClient = new HttpClient();
            }

            var response  = await httpClient.PostAsync(uri, stringContent);
            LoginResult result = new LoginResult(_loginCredential.Username,_loginCredential.Password, response.IsSuccessStatusCode, response);
            return result;
        }
    }
}
