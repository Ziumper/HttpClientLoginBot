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
        public ProxyList ProxyList { get; set; }

        public LoginClient(string url,string mediaType,Encoding encoding,ProxyList proxyList)
        {
            Url = url;
            MediaType = mediaType;
            Encoding = encoding;
            ProxyList = proxyList;

            _activeProxy = null;
        }

        public LoginClient(string url)
        {
            Url = url;
            InitlizeBase();
        }

        public LoginClient(string url, ProxyList proxyList)
        {
            Url = url;
            ProxyList = proxyList;
            InitlizeBase();
        }

        private void InitlizeBase() {
            MediaType = "application/x-www-form-urlencoded";
            Encoding = Encoding.UTF8;
            _activeProxy = null;
        }

        public virtual async Task<LoginResult> Login(LoginData loginData)
        {
            HttpClient httpClient = GetHttpClient();

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
            catch (HttpRequestException e)
            {
                /*
                 * The connection attempt failed because the 
                 * linked page did not respond correctly 
                 * after a set period of time or 
                 * the connection created failed 
                 * because the connected host did not respond
                 * 
                 * Proxy is not working.
                 */
                throw e;
            }catch (Exception  e)
            {
                /*
                 * Some diferent exception occured
                 */
                throw e;
            }
            finally
            {
                httpClient.Dispose();
            }

            
        }

        private HttpClient GetHttpClient()
        {
            HttpClient httpClient = new HttpClient();

            var isProxyListMoreThenZero = ProxyList?.Count > 0;
            if(isProxyListMoreThenZero)
            {
                _activeProxy = ProxyList.Proxy;
                var httpHandler = new HttpClientHandler();
                httpHandler.Proxy = _activeProxy.WebProxy;
                httpHandler.UseProxy = true;
                httpClient = new HttpClient(httpHandler);
            }

            return httpClient;
        }
    }
}
