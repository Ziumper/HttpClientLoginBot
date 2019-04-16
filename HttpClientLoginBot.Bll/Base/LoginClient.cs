using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientLoginBot.Bll.Base
{
    public class LoginClient : ILoginClient
    {
        public LoginProxy ActiveProxy { get; set; }
        
        public async Task<LoginResult> Login(LoginCredential loginCredential)
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

            var response  = await httpClient.PostAsync(loginCredential.Uri, loginCredential.StringContent);
            LoginResult result = new LoginResult(loginCredential.Username,loginCredential.Password, response);
            return result;
        }
    }
}
