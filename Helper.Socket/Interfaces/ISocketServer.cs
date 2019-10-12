using System;

namespace Helper.Socket.Interfaces
{
    public interface ISocketServer
    {
        event Action<string> RaiseNotification;
        event Action<byte[]> RaiseAction;
        void Start(int port);
    }
}
