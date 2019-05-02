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
        public Boolean UseProxy { get; set; }

        public LoginClient(string url,string mediaType,Encoding encoding,ProxyQueque proxyList,Boolean useProxy)
        {
            Url = url;
            MediaType = mediaType;
            Encoding = encoding;
            ProxyQueque = proxyList;
            UseProxy = useProxy;
            _activeProxy = null;
        }

        public LoginClient(string url)
        {
            Url = url;
            InitlizeBase();
        }

        public LoginClient(string url, ProxyQueque proxyList,Boolean useProxy)
        {
            InitlizeBase();
            Url = url;
            ProxyQueque = proxyList;
            UseProxy = useProxy;
           
        }

        public LoginClient(string url, ProxyQueque proxyList)
        {
            Url = url;
            ProxyQueque = proxyList;
            InitlizeBase();
        }

        private void InitlizeBase() {
            MediaType = "application/x-www-form-urlencoded";
            Encoding = Encoding.UTF8;
            _activeProxy = null;
            UseProxy = false;
        }

        public virtual async Task<LoginResult> Login(LoginData loginData)
        {
            if(UseProxy && ProxyQueque.IsEnd)
            {
                var result = new LoginResult();
                result.IsSucces = false;
                result.Message = "No proxy in proxy queque";
                return result;
            }

            HttpClient httpClient = GetHttpClient();

            try {
                StringContent stringContent = new StringContent(loginData.RequestBody, Encoding, MediaType);
                var response = await httpClient.PostAsync(Uri, stringContent);

                LoginResult result = new LoginResult();
                result.Response = response;
                result.Username = loginData.Username;
                result.Passwrod = loginData.Password;
                if (response.IsSuccessStatusCode)
                {
                    result.IsSucces = true;
                }

                ProxyQueque.ResetProxyQueque();

                return result;
            } catch (HttpRequestException e) {
                /*
                 * The connection attempt failed because the 
                 * linked page did not respond correctly 
                 * after a set period of time or 
                 * the connection created failed 
                 * because the connected host did not respond
                 * 
                 * Proxy is not working or something else happend
                 */

                var result = new LoginResult();
                if (UseProxy)
                {
                    result = await Login(loginData);
                    return result;
                }

                result.IsSucces = false;
                result.Message = e.Message;
                return result;
            } catch (Exception  e) {
                /*
                 * Some diferent exception occured
                 */
                var result = new LoginResult();
                result.IsSucces = false;
                result.Message = e.Message;
                return result;
                
            }
            finally {
                httpClient.Dispose();
            }

            
        }

        private HttpClient GetHttpClient()
        {
            
            if(UseProxy)
            {
                return GetHttpClientWithProxy();
            }

            HttpClient httpClient = new HttpClient();

            return httpClient;
        }

        private HttpClient GetHttpClientWithProxy()
        {
            _activeProxy = ProxyQueque.Proxy;
            var httpHandler = new HttpClientHandler();
            httpHandler.Proxy = _activeProxy.WebProxy;
            httpHandler.UseProxy = true;
            var httpClient = new HttpClient(httpHandler);
            return httpClient;
        }

    }
}
