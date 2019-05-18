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
        public TibiaLoginClient(string url,ProxyQueque proxyQueque) : base(url,proxyQueque)
        {
            
        }

        public async new Task<TibiaLoginResult> LoginAsync(LoginData loginCredential)
        {
            var baseResult = await base.LoginAsync(loginCredential);

            var result = new TibiaLoginResult(baseResult);

            await result.Validate();

            if (result.IsBlockIpErrorOccured)
            {
                if(ProxyQueque.IsEnd)
                {
                    result = await TryToLoginWithoutProxyWhenBlockIpErrorOccuredMeanTime(result, loginCredential);
                    return result;
                }

                SetToUseProxyForAvoidBlockIpError();
                ResetActiveProxyToGetNextProxyInProxyQuequeToLogin();

                result = await LoginAsync(loginCredential);
            }

            return result;
        }

        private async Task<TibiaLoginResult> TryToLoginWithoutProxyWhenBlockIpErrorOccuredMeanTime(TibiaLoginResult result,LoginData loginCredential)
        {
            if (UseProxy)
            {
                UseProxy = false;
                result = await LoginAsync(loginCredential);
            }
            else
            {
                ProxyQueque.ResetProxyQueque();
                result.Username = loginCredential.Username;
                result.Password = loginCredential.Password;
                result.Message = "Block ip error occured";
                result.IsSuccess = false;
            }

            return result;
        }

        private void ResetActiveProxyToGetNextProxyInProxyQuequeToLogin()
        {
            var IsUsedProxyBefore = _activeProxy != null;
            if (IsUsedProxyBefore)
            {
                _activeProxy = null;
            }
        }

        private void SetToUseProxyForAvoidBlockIpError()
        {
            UseProxy = true;
        }


    }
}
