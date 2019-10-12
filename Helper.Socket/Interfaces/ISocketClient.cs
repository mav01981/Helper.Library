namespace Helper.Socket.Interfaces
{
    public interface ISocketClient
    {
        System.Net.Sockets.Socket Connect(string host, int port);
        void Send(System.Net.Sockets.Socket client, byte[] byteData);
        void Receive(System.Net.Sockets.Socket client);
    }
}
