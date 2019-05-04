using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientLoginBot.Bll.Base
{
    public class LoginClient: ILoginClient<LoginResult>
    {
        protected LoginProxy _activeProxy;
        public string Url { get; set; }
        public string MediaType { get; set; }  //"application/x-www-form-urlencoded
        public Uri Uri { get { return new Uri(Url); } }
        public Encoding Encoding { get; set; }
        public ProxyQueque ProxyQueque { get; set; }
        public bool UseProxy { get; set; }
        public double TimeoutTime { get; set; }

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
            TimeoutTime = 6000;
        }

        public virtual async Task<LoginResult> LoginAsync(LoginData loginData)
        {
            using (HttpClient httpClient = GetHttpClient())
            {
                var result = await Login(loginData, httpClient);
                return result;
            }
             
        }

        public virtual async Task<LoginResult> LoginAsync(LoginData loginData,LoginProxy loginProxy)
        {
            using (HttpClient httpClient = GetHttpClientWithProxy(loginProxy))
            {
                try
                {
                    var result = await Login(loginData, httpClient);
                    return result;
                }
                catch (Exception exception)
                {
                    var result = new LoginResult(loginData);
                    result.Message = exception.Message;
                    result.IsSucces = false;
                    return result;
                }
            }
               
        }

        private async Task<LoginResult> Login(LoginData loginData,HttpClient httpClient)
        {
            StringContent stringContent = new StringContent(loginData.RequestBody, Encoding, MediaType);
            var response = await httpClient.PostAsync(Uri, stringContent);

            httpClient.Dispose();

            LoginResult result = new LoginResult();
            result.Response = response;
            result.Username = loginData.Username;
            result.Password = loginData.Password;
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
            httpClient.Timeout = TimeSpan.FromSeconds(TimeoutTime);

            return httpClient;
        }

        private HttpClient GetHttpClientWithProxy()
        {
            var isActiveProxySet = _activeProxy != null;
            if(!isActiveProxySet)
            {
                _activeProxy = ProxyQueque.Proxy;
            }

            var httpClient = GetHttpClientWithProxy(_activeProxy);
            return httpClient;
        }

        private HttpClient  GetHttpClientWithProxy(LoginProxy loginProxy)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.Proxy = loginProxy.WebProxy;
            httpHandler.UseProxy = true;
            var httpClient = new HttpClient(httpHandler);
            httpClient.Timeout = TimeSpan.FromMilliseconds(TimeoutTime);
            return httpClient;
        }

    }
}
