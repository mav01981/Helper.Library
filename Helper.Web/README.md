# Helper.Web

**Implementation in Startup**

```c#
        services.AddHttpClient();
        services.AddScoped<IHttpRequest, XmlRequest>();
        services.AddScoped<IHttpRequest, JsonRequest>();
        services.AddScoped<IHttpClientStrategyFactory, HttpClientStrategyFactory>();
        services.AddScoped<IHttpRequest[]>(provider =>
        {
           var factory=(IHttpClientStrategyFactory)provider.GetService(typeof(IHttpRequest));
           return factory.Create(RequestFormat.Json);
        });
```
**Implement in service/controller**

    public class HttpService : IHttpService
    {
        private readonly IHttpClientStrategy _httpClientStrategy;
    
        public HttpService (IHttpClientStrategy httpClientStrategy)
        {
            _httpClientStrategy = httpClientStrategy;
        }
    }