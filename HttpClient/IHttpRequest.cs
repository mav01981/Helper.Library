using System.Threading;
using System.Threading.Tasks;

namespace Web
{
    public interface IHttpRequest
    {
        Task<T> Get<T>(string url, CancellationToken cancellationToken);
        Task<T> Post<T>(string url, T model, CancellationToken cancellationToken);
        Task<T> Put<T>(string url, T model, CancellationToken cancellationToken);
    }
}