using System.Threading.Tasks;

namespace HttpClientLoginBot.Bll.Base
{
    public interface ILoginClient
    {
        Task<LoginResult> Login(LoginCredential loginCredential);
    }
}
