namespace Helper.Web
{
    public interface IHttpClientStrategyFactory
    {
        IHttpRequest[] Create(RequestFormat format);
    }
}