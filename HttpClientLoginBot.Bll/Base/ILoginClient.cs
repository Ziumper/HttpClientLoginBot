using System.Threading.Tasks;

namespace HttpClientLoginBot.Bll.Base
{
    public interface ILoginClient<T>
    {
        string Url { get; set; }
        string MediaType { get; set; }
        LoginProxy ActiveProxy { get; set; }
        Task<T> Login(LoginData loginCredential);
    }
}
