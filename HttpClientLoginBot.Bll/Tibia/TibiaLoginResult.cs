using HttpClientLoginBot.Bll.Base;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientLoginBot.Bll.Tibia
{
    public class TibiaLoginResult : LoginResult
    {
        private readonly string _blockIpErrorText;

        public bool IsBlockIpErrorOccured { get; private set; }

        public TibiaLoginResult(LoginResult result)
        {
            IsSucces = result.IsSucces;
            Message = result.Message;
            Response = result.Response;
            IsBlockIpErrorOccured = false;
            _blockIpErrorText = "Wrong account data has been entered from your IP address too often. You are unable to log in from this IP address for the next 30 minutes. Please wait.";
        }

        public async Task Validate()
        {
            if (!IsSucces)
            {
                return;
            }

            await CheckBlockIpError();
            if(!IsBlockIpErrorOccured)
            {
                ValidateIfContainsSecureSessionIdCookie();
            }

            if (IsSucces)
            {
                Message = "Logged Succesfully";
            } else
            {
                Message = "Wrong credentials";
            }
        }

        private async Task CheckBlockIpError()
        {
            var content = await Response.Content.ReadAsStringAsync();
            var containsBlockIpErrorMessage = content.Contains(_blockIpErrorText);
            if (containsBlockIpErrorMessage)
            {
                IsBlockIpErrorOccured = true;
            }

        }

        private void ValidateIfContainsSecureSessionIdCookie()
        {
            IEnumerable<string> cookies = new List<string>();
            var haveValues = Response.Headers.TryGetValues("Set-Cookie", out cookies);

            if (haveValues)
            {
                foreach (var item in cookies)
                {
                    ValidateIfCookieContainsSecureSessionId(item);
                }
            }
        }

        private void ValidateIfCookieContainsSecureSessionId(string cookie)
        {
            if (cookie.Contains("SecureSessionID"))
            {
                IsSucces = true;
                return;
            }
            IsSucces = false;
        }
    }
}
