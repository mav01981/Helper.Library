using Helper.Socket.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;

namespace SocketServer
{
    class Program
    {
        private static IServiceProvider _serviceProvider;
        private static string _filePath;
        static void Main(string[] args)
        {
            _filePath = $@"{Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)}";

            var collection = new ServiceCollection();
            collection.AddScoped<ISocketServer, Helper.Socket.SocketServer>();

            _serviceProvider = collection.BuildServiceProvider();

            var socketServer = _serviceProvider.GetService<ISocketServer>();

            socketServer.RaiseNotification += SocketServerOnRaiseNotification;
            socketServer.RaiseAction += SocketServerOnRaiseWork;

            socketServer.Start(1201);
        }

        private static void SocketServerOnRaiseWork(byte[] data)
        {
            try
            {
                byte[] fileType = data.Take(4).ToArray();
                var fileExtension = System.Text.Encoding.UTF8.GetString(fileType);

                File.WriteAllBytes($@"{_filePath}\files\test{fileExtension}", data.Skip(4).Take(data.Length - 4).ToArray());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void SocketServerOnRaiseNotification(string message)
        {
            Console.WriteLine(message);
        }
    }
}
