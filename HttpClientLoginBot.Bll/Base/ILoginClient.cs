using System.Threading.Tasks;

namespace HttpClientLoginBot.Bll.Base
{
    public interface ILoginClient
    {
        Task<LoginResult> Login(string url,string body);
    }
}
