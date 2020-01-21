namespace Helper.RabbitMQ.PubSub.Models
{
    public class Send<T>
    {
        public string Host { get; set; }
        public string ExhangeName { get; set; }
        public string RouteKey { get; set; }
        public T Message { get; set; }
    }
}