namespace Helper.Web
{
    using System;
    using System.Net.Http;

    public class HttpClientStrategyFactory : IHttpClientStrategyFactory
    {
        private readonly IHttpClientFactory httpClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientStrategyFactory"/> class.
        /// </summary>
        /// <param name="httpClientFactory">httpClientFactory.</param>
        public HttpClientStrategyFactory(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        /// <inheritdoc/>
        public IHttpRequest[] Create(RequestFormat format)
        {
            switch (format)
            {
                case RequestFormat.Json:
                    return new IHttpRequest[] { new JsonRequest(this.httpClientFactory) };
                case RequestFormat.XML:
                    return new IHttpRequest[] { new XmlRequest(this.httpClientFactory) };
                default:
                    throw new ArgumentException("Http Request ");
            }
        }
    }
}
