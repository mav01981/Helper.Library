using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Helper.Socket
{
    public class FileSocketServer : IFileSocketServer
    {
        public event Action<byte[]> RaiseAction;
        public event Action<string> RaiseNotification;
        public void Start(int port, string savePath)
        {
            var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            var ipAddress = ipHostInfo.AddressList[0];

            var listener = new TcpListener(IPAddress.Any, port);

            listener.Start();

            RaiseNotification?.Invoke($"File Socket Server listening on Port:{port}");

            while (true)
            {
                using (var client = listener.AcceptTcpClient())
                using (var stream = client.GetStream())
                {
                    RaiseAction?.Invoke(ReadAllBytes(stream));
                }
            }
        }
        private static byte[] ReadAllBytes(Stream stream)
        {
            if (stream is MemoryStream stream1)
                return stream1.ToArray();

            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }

    public interface IFileSocketServer
    {
        void Start(int port, string savePath);
        event Action<byte[]> RaiseAction;
        event Action<string> RaiseNotification;
    }
}