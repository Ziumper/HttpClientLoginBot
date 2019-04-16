using System.Threading.Tasks;

namespace HttpClientLoginBot.Bll.Base
{
    public interface ILoginClient
    {
        string Url { get; set; }
        string RequestBody { get; set; }

        LoginProxy ActiveProxy { get; set; }
        Task<LoginResult> Login(LoginCredential loginCredential);
    }
}
