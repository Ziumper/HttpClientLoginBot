using HttpClientLoginBot.Bll.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientLoginBot.Bll.Base
{
    public class ProxyTesterClient : LoginClient
    {
        public List<LoginProxy> ProxyList { get; private set; }


        public ProxyTesterClient(string url, ProxyQueque proxyQueque) : base(url, proxyQueque)
        {
        }

        public override async Task<LoginResult> LoginAsync(LoginData loginData)
        {
            var loginResult = new LoginResult();
            UseProxy = true;
            try
            {
               loginResult = await base.LoginAsync(loginData);
            }catch (Exception e) {
                
            }
            return loginResult;
        }
    }
}
