namespace Helper.RabbitMQ.Messaging.PubSub.Models
{
    public class Recieve<T>
    {
        public string Host { get; set; }
        public string ExchangeName { get; set; }
    }
}
