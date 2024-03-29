﻿using System;
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
                    result.IsSuccess = false;
                    return result;
                }
            }
               
        }

        private async Task<LoginResult> Login(LoginData loginData,HttpClient httpClient)
        {
            try
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
                    result.IsSuccess = true;
                    return result;
                }

                result.IsSuccess = false;
                result.Message = "Failed on trying to login,check response for more information";
                return result;
            }catch (ArgumentNullException)
            {
                var result = new LoginResult(loginData);
                result.Message = "Request was null";
                result.IsSuccess = false;
                return result;
            }catch (InvalidOperationException)
            {
                var result = new LoginResult(loginData);
                result.Message = "The request message was already sent by the HttpClient instance";
                result.IsSuccess = false;
                return result;
            }catch(HttpRequestException)
            {
                var result = new LoginResult(loginData);
                result.Message = "The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.";
                result.IsSuccess = false;
                return result;
            }catch(TaskCanceledException)
            {
                var result = new LoginResult(loginData);
                result.Message = "The request timed-out or the user canceled the request's ";
                result.IsSuccess = false;
                return result;
            }catch(Exception exception)
            {
                throw exception;
            }



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
