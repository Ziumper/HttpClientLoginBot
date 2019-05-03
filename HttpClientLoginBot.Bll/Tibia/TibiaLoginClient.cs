using HttpClientLoginBot.Bll.Base;
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
            
        }

        public async new Task<TibiaLoginResult> Login(LoginData loginCredential)
        {
            var baseResult = await base.Login(loginCredential);

            var result = new TibiaLoginResult(baseResult);

            await result.Validate();
            
            return result;
        }


    }
}
