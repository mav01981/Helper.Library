namespace Helper.Web
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Serialization;

    public class XmlRequest : IHttpRequest
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RequestFormat Format => RequestFormat.XML;

        public XmlRequest(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<T> Get<T>(string url, CancellationToken cancellationToken, Header header = null)
        {
            var client = this.httpClientFactory.CreateClient();
            using var request = new HttpRequestMessage(HttpMethod.Get, url);

            if (header != null)
            {
                request.Headers.Add(header.Name, header.Value);
            }

            using var response = await client.SendAsync(request, cancellationToken);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode == false)
            {
                throw new ApiException
                {
                    StatusCode = (int)response.StatusCode,
                    Content = content,
                };
            }

            return DeserializeXMLResponse<T>(content);
        }

        public Task<T> Patch<T>(string url, T model, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<T> Post<T>(string url, T model, CancellationToken cancellationToken, Header header = null)
        {
            throw new NotImplementedException();
        }

        public Task<T> Put<T>(string url, T model, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private static T DeserializeXMLResponse<T>(string soapResponse)
        {
            XmlSerializer xmlSerialize = new XmlSerializer(typeof(T));

            return (T)xmlSerialize.Deserialize(new StringReader(soapResponse));
        }
    }
}
