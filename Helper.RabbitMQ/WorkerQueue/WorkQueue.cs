using Helper.FileConvertor;
using Helper.RabbitMQ.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace Helper.RabbitMQ
{
    /// <summary>
    /// Work Queue the Competing Consumer pattern for Rabbit MQ Message broker.
    /// </summary>
    public class WorkQueue : IWorkQueue
    {
        private readonly IConnectionFactory _connectionFactory;

        public event Action<byte[]> RaiseReceieveEvent;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionFactory"></param>
        public WorkQueue(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        public void Send<T>(Send<T> action)
        {
            using (var connection = _connectionFactory.CreateConnection(action.Host))
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: action.QueueName,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = ByteConvertor.ObjectToByteArray<T>(action.Message);

                channel.BasicPublish(exchange: "",
                                     routingKey: action.RouteKey,
                                     basicProperties: null,
                                     body: body);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        public void Recieve<T>(Recieve<T> action)
        {
            using (var connection = _connectionFactory.CreateConnection(action.Host))
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: action.QueueName,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                channel.BasicQos(0, 1, false);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    RaiseReceieveEvent(ea.Body);

                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                };

                channel.BasicConsume(queue: action.QueueName,
                                     autoAck: true,
                                     consumer: consumer);
            }
        }
    }
}