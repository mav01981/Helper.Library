namespace Helper.Web
{
    using System.IO;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    /// <summary>
    /// Json Request.
    /// </summary>
    public class JsonRequest : IHttpRequest
    {
        private readonly IHttpClientFactory httpClientFactory;

        /// <inheritdoc/>
        public RequestFormat Format => RequestFormat.Json;

        public JsonRequest(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// GET Json Object.
        /// </summary>
        /// <typeparam name="T">Object.</typeparam>
        /// <param name="url">Url.</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <param name="header">Header.</param>
        /// <returns>Response.</returns>
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

            return JsonConvert.DeserializeObject<T>(content);
        }

        /// <summary>
        /// Post Object.
        /// </summary>
        /// <typeparam name="T">Object.</typeparam>
        /// <param name="url">Url.</param>
        /// <param name="model">model.</param>
        /// <param name="cancellationToken">cancellationToken.</param>
        /// <param name="header">Header.</param>
        /// <returns>Response.</returns>
        public async Task<T> Post<T>(string url, T model, CancellationToken cancellationToken, Header header = null)
        {
            var client = this.httpClientFactory.CreateClient();
            using var request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Content = new StringContent(JsonConvert.SerializeObject(model));

            if (header != null)
            {
                request.Headers.Add(header.Name, header.Value);
            }

            using var response = await client.SendAsync(request, cancellationToken);
            var stream = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
            {
                return DeserializeJsonFromStream<T>(stream);
            }

            var content = await StreamToStringAsync(stream);

            throw new ApiException
            {
                StatusCode = (int)response.StatusCode,
                Content = content,
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<T> Put<T>(string url, T model, CancellationToken cancellationToken)
        {
            var client = this.httpClientFactory.CreateClient();
            using var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Content = new StringContent(JsonConvert.SerializeObject(model));

            using var response = await client.SendAsync(request, cancellationToken);
            var stream = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
            {
                return DeserializeJsonFromStream<T>(stream);
            }

            var content = await StreamToStringAsync(stream);

            throw new ApiException
            {
                StatusCode = (int)response.StatusCode,
                Content = content,
            };
        }

        public async Task<T> Patch<T>(string url, T model, CancellationToken cancellationToken)
        {
            var client = this.httpClientFactory.CreateClient();
            using var request = new HttpRequestMessage(HttpMethod.Patch, url);
            using var response = await client.PatchAsync(url, new StringContent(JsonConvert.SerializeObject(model)), cancellationToken);
            var stream = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
            {
                return DeserializeJsonFromStream<T>(stream);
            }

            var content = await StreamToStringAsync(stream);

            throw new ApiException
            {
                StatusCode = (int)response.StatusCode,
                Content = content,
            };
        }

        private static T DeserializeJsonFromStream<T>(Stream stream)
        {
            if (stream == null || stream.CanRead == false)
            {
                return default;
            }

            using var sr = new StreamReader(stream);
            using var jtr = new JsonTextReader(sr);
            var js = new JsonSerializer();
            var searchResult = js.Deserialize<T>(jtr);
            return searchResult;
        }

        private static async Task<string> StreamToStringAsync(Stream stream)
        {
            string content = null;

            if (stream != null)
            {
                using var sr = new StreamReader(stream);
                content = await sr.ReadToEndAsync();
            }

            return content;
        }
    }
}
