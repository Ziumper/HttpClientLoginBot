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

        public async new Task<TibiaLoginResult> Login(LoginData loginCredential)
        {
            var baseResult = await base.Login(loginCredential);

            var result = new TibiaLoginResult(baseResult);

            await result.Validate();

            if (result.IsBlockIpErrorOccured)
            {
                SetToUseProxyForAvoidBlockIpError();
                ResetActiveProxyToGetNextProxyInProxyQuequeToLogin();

                if(ProxyQueque.IsEnd)
                {
                    ProxyQueque.ResetProxyQueque();
                    throw new TibiaQuequeProxyEnd();
                }

                result = await Login(loginCredential);
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
