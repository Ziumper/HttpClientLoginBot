using HttpClientLoginBot.Bll.Base;
using System.Threading.Tasks;

namespace HttpClientLoginBot.Bll.Tibia
{
    public class TibiaLoginClient : LoginClient
    {
        public TibiaLoginClient(string url, string requestBody) : base(url, requestBody)
        {

        }

        public override Task<LoginResult> Login(LoginCredential loginCredential)
        {
            var result = base.Login(loginCredential);
            return result;
        }
    }
}
