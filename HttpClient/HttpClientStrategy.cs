namespace Helper.Web
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class HttpClientStrategy : IHttpClientStrategy
    {
        private readonly IHttpRequest[] _requests;

        public HttpClientStrategy(IHttpRequest[] requests)
        {
            this._requests = requests;
        }

        public Task<T> Get<T>(RequestFormat format, string url, Header header = null)
        {
            return this._requests.FirstOrDefault(x => x.Format == format)?.Get<T>(url, CancellationToken.None, header);
        }

        public Task<T> Post<T>(RequestFormat format, string url, T data, Header header = null)
        {
            return this._requests.FirstOrDefault(x => x.Format == format)?.Post<T>(url, data, CancellationToken.None, header);
        }

        public Task<T> Put<T>(RequestFormat format, string url, T data, Header header = null)
        {
            return this._requests.FirstOrDefault(x => x.Format == format)?.Put<T>(url, data, CancellationToken.None);
        }

        public Task<T> Patch<T>(RequestFormat format, string url, T data, Header header = null)
        {
            return this._requests.FirstOrDefault(x => x.Format == format)?.Patch<T>(url, data, CancellationToken.None);
        }
    }
}
