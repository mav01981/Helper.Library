using Helper.RabbitMQ.Model;
using System;

namespace Helper.RabbitMQ
{
    public interface IWorkQueue
    {
        event Action<byte[]> RaiseReceieveEvent;
        void Send<T>(Send<T> message);
    }
}