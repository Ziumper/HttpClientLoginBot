using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientLoginBot.Bll.Base
{
    public abstract class LoginClient : ILoginClient
    {
        public LoginProxy ActiveProxy { get; set; }
        public string Url { get; set; }
        public string RequestBody { get; set; }
        public Uri Uri { get { return new Uri(Url); } }
        public virtual StringContent StringContent {get {return new StringContent(RequestBody, Encoding.UTF8, "application/x-www-form-urlencoded");}}

        public LoginClient(string url, string requestBody)
        {
            Url = url;
            RequestBody = requestBody;
        }

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

            var response  = await httpClient.PostAsync(Uri, StringContent);
            LoginResult result = new LoginResult();
            result.Response = response;
            result.Username = loginCredential.Username;
            result.Passwrod = loginCredential.Password;
            return result;
        }
    }
}
