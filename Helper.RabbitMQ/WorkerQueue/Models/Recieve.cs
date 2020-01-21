namespace Helper.RabbitMQ.Model
{
    public class Recieve<T>
    {
        public string Host { get; set; }
        public string QueueName { get; set; }
    }
}
