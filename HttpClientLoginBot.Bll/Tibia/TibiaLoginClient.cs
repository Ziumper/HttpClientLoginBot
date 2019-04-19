using HttpClientLoginBot.Bll.Base;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HttpClientLoginBot.Bll.Tibia
{
    public class TibiaLoginClient : LoginClient
    {
        private readonly string _blockIpError;

        public TibiaLoginClient(string url) : base(url)
        {
            _blockIpError = "Wrong account data has been entered from your IP address too often. You are unable to log in from this IP address for the next 30 minutes. Please wait.";
        }

        public override async Task<LoginResult> Login(LoginData loginCredential)
        {
            var result = await base.Login(loginCredential);

            result.IsSucces = false;

            await ValidateResponse(result);
            
            return result;
        }

        private async Task ValidateResponse(LoginResult result)
        {
            var isResponseSucces = result.Response.IsSuccessStatusCode;
            if (isResponseSucces)
            {
                var content = await result.Response.Content.ReadAsStringAsync();
                var containsBlockIpErrorMessage = content.Contains(_blockIpError);
                if (containsBlockIpErrorMessage)
                {
                    result.IsFinished = false;
                    return;
                }

                ValidateCookieHeaders(result);
            }
        }

        private void ValidateCookieHeaders(LoginResult result)
        {
            var headers = result.Response.Headers;
            ValidateIfContainsSecureSessionIdCookie(headers,result);
        }

        private void ValidateIfContainsSecureSessionIdCookie(HttpResponseHeaders headers,LoginResult result)
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

        private void ValidateIfCookieContainsSecureSessionId(string cookie,LoginResult result)
        {
            if (cookie.Contains("SecureSessionID"))
            {
                SetSuccesAndFinishedResult(result);
                return;
            }
        }
        
        private void SetSuccesAndFinishedResult(LoginResult result)
        {
            result.IsFinished = true;
            result.IsSucces = true;
        }
    }
}
