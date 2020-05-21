namespace Helper.Web
{
    using System.Threading.Tasks;

    public interface IHttpClientStrategy
    {
        Task<T> Get<T>(RequestFormat format, string url, Header header = null);

        Task<T> Post<T>(RequestFormat format, string url, T data, Header header = null);

        Task<T> Put<T>(RequestFormat format, string url, T data, Header header = null);

        Task<T> Patch<T>(RequestFormat format, string url, T data, Header header = null);
    }
}