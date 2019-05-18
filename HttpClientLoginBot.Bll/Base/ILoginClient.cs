using System.Threading.Tasks;

namespace HttpClientLoginBot.Bll.Base
{
    public interface ILoginClient<T>
    {
        string Url { get; set; }
        string MediaType { get; set; }
        Task<T> LoginAsync(LoginData loginCredential);
        ProxyQueque ProxyQueque { get; set; }
    }
}
