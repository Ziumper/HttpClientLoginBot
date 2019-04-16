using System.Threading.Tasks;

namespace HttpClientLoginBot.Bll.Base
{
    public interface ILoginClient
    {
        LoginProxy ActiveProxy { get; set; }
        Task<LoginResult> Login(LoginCredential loginCredential);
    }
}
