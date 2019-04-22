using HttpClientLoginBot.Bll.Base;
using HttpClientLoginBot.Bll.Tibia.Exceptions;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HttpClientLoginBot.Bll.Tibia
{
    public class TibiaLoginClient : LoginClient, ILoginClient<TibiaLoginResult>
    {
        private readonly string _blockIpError;

        public TibiaLoginClient(string url) : base(url)
        {
            _blockIpError = "Wrong account data has been entered from your IP address too often. You are unable to log in from this IP address for the next 30 minutes. Please wait.";
        }

        public async new Task<TibiaLoginResult> Login(LoginData loginCredential)
        {
            var baseResult = await base.Login(loginCredential);

            var result = new TibiaLoginResult(baseResult);
            
            await ValidateResponse(result);
            
            return result;
        }


        private async Task ValidateResponse(TibiaLoginResult result)
        {
            var isResponseSucces = result.Response.IsSuccessStatusCode;
            if (isResponseSucces)
            {
                var content = await result.Response.Content.ReadAsStringAsync();
                var containsBlockIpErrorMessage = content.Contains(_blockIpError);
                if (containsBlockIpErrorMessage)
                {
                    if(ActiveProxy != null)
                    {
                        throw new TibiaBlockIpException(ActiveProxy.FullAddres);
                    }
                    else
                    {
                        throw new TibiaBlockIpException();
                    }
                }

                ValidateCookieHeaders(result);
            }
        }

        private void ValidateCookieHeaders(TibiaLoginResult result)
        {
            var headers = result.Response.Headers;
            ValidateIfContainsSecureSessionIdCookie(headers,result);
        }

        private void ValidateIfContainsSecureSessionIdCookie(HttpResponseHeaders headers, TibiaLoginResult result)
        {
            IEnumerable<string> cookies = new List<string>();
            var haveValues = headers.TryGetValues("Set-Cookie", out cookies);
            
            if (haveValues)
            {
                foreach (var item in cookies)
                {
                    ValidateIfCookieContainsSecureSessionId(item, result);
                }
            }
        }

        private void ValidateIfCookieContainsSecureSessionId(string cookie, TibiaLoginResult result)
        {
            if (cookie.Contains("SecureSessionID"))
            {
                SetSuccesAndFinishedResult(result);
                return;
            }
            result.IsFinished = true;
            result.IsSucces = false;
        }
        
        private void SetSuccesAndFinishedResult(TibiaLoginResult result)
        {
            result.IsFinished = true;
            result.IsSucces = true;
        }
    }
}
