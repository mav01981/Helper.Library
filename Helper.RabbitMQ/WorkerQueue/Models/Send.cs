namespace Helper.RabbitMQ.Model
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Send<T>
    {
        public string Host { get; set; }
        public string QueueName { get; set; }
        public string RouteKey { get; set; }
        public T Message { get; set; }
    }
}
