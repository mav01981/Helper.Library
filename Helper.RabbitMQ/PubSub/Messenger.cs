using Helper.FileConvertor;
using Helper.RabbitMQ.Messaging.PubSub.Models;
using Helper.RabbitMQ.PubSub.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace Helper.RabbitMQ.PubSub
{
    /// <summary>
    /// Publish Subscriber helpers for Rabbit MQ Message broker.
    /// </summary>
    public class Messenger: IMessenger
    {
        private readonly IConnectionFactory _connectionFactory;

        public event Action<byte[]> RaiseReceieveEvent;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionFactory"></param>
        public Messenger(IConnectionFactory connectionFactory)
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
                channel.ExchangeDeclare(exchange: action.ExhangeName, type: ExchangeType.Fanout);

                var body = ByteConvertor.ObjectToByteArray<T>(action.Message);

                channel.BasicPublish(exchange: action.ExhangeName,
                                     routingKey: "",
                                     basicProperties: null,
                                     body: body);
            }
        }
        public void Recieve<T>(Recieve<T> action)
        {
            using (var connection = _connectionFactory.CreateConnection(action.Host))
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: action.ExchangeName, type: ExchangeType.Fanout);

                var queueName = channel.QueueDeclare().QueueName;

                channel.QueueBind(queue: queueName,
                                  exchange: action.ExchangeName,
                                  routingKey: "");

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    RaiseReceieveEvent(ea.Body);
                };

                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);
            }
        }
    }
}
