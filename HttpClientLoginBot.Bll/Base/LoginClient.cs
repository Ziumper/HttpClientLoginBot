using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientLoginBot.Bll.Base
{
    public abstract class LoginClient: ILoginClient<LoginResult>
    {
        protected LoginProxy _activeProxy;
        public string Url { get; set; }
        public string MediaType { get; set; }  //"application/x-www-form-urlencoded
        public Uri Uri { get { return new Uri(Url); } }
        public Encoding Encoding { get; set; }
        public ProxyQueque ProxyQueque { get; set; }
        public bool UseProxy { get; set; }


        public LoginClient(string url)
        {
            Url = url;
            InitlizeBase();
        }
        
        public LoginClient(string url, ProxyQueque proxyQueque)
        {
            Url = url;
            ProxyQueque = proxyQueque;
            InitlizeBase();
        }

        private void InitlizeBase() {
            MediaType = "application/x-www-form-urlencoded";
            Encoding = Encoding.UTF8;
            _activeProxy = null;
            UseProxy = false;
        }

        public virtual async Task<LoginResult> LoginAsync(LoginData loginData)
        {
            HttpClient httpClient = GetHttpClient();

            StringContent stringContent = new StringContent(loginData.RequestBody, Encoding, MediaType);
            var response = await httpClient.PostAsync(Uri, stringContent);

            httpClient.Dispose();

            LoginResult result = new LoginResult();
            result.Response = response;
            result.Username = loginData.Username;
            result.Passwrod = loginData.Password;
            if (response.IsSuccessStatusCode)
            {
                result.IsSucces = true;
                return result;
            }

            result.IsSucces = false;
            result.Message = "Failed on trying to login,check response for more information";

            return result;
        }

        private HttpClient GetHttpClient()
        {
            
            if(UseProxy && !ProxyQueque.IsEnd)
            {
                return GetHttpClientWithProxy();
            }

            HttpClient httpClient = new HttpClient();

            return httpClient;
        }

        private HttpClient GetHttpClientWithProxy()
        {
            var isActiveProxySet = _activeProxy != null;
            if(!isActiveProxySet)
            {
                _activeProxy = ProxyQueque.Proxy;
            }
            
            var httpHandler = new HttpClientHandler();
            httpHandler.Proxy = _activeProxy.WebProxy;
            httpHandler.UseProxy = true;
            var httpClient = new HttpClient(httpHandler);
            return httpClient;
        }

    }
}
