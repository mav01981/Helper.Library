using Helper.Socket;
using Helper.Socket.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Helper.Socket.Bytes;
using Xunit;
using System.Diagnostics;

namespace Tests.Socket
{
    public class SocketTests
    {
        private static IServiceProvider _serviceProvider;
        private readonly string _filepath;

        public SocketTests()
        {
            _filepath = $@"{Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)}";

            Process.Start(@"C:\Users\Jonathan\source\repos\Thread\SocketServer\bin\Release\netcoreapp2.1\publish\SocketServer.exe");

            var collection = new ServiceCollection();
            collection.AddScoped<ISocketServer, SocketServer>();
            collection.AddScoped<ISocketClient, SocketClient>();

            _serviceProvider = collection.BuildServiceProvider();
        }

        [Fact]
        public void SocketClient_Connect_ToServer()
        {
            //Arrange
            var socketClient = _serviceProvider.GetService<ISocketClient>();
            //Act
            var client = socketClient.Connect("", 1201);
            //Assert
            Assert.True(client.Connected);

            client.Shutdown(SocketShutdown.Both);
        }

        [Fact]
        public void SocketClient_SendFileWithFileType_ToServer()
        {
            //Arrange
            var socketClient = _serviceProvider.GetService<ISocketClient>();
            //Act
            var client = socketClient.Connect("", 1201);

            var bytes = File.ReadAllBytes($@"{_filepath}\Files\Untitled Document.pdf");

            var finalBytes = Encoding.ASCII.GetBytes($@".pdf");

            Bytes.AppendSpecifiedBytes(ref finalBytes, bytes);

            socketClient.Send(client, finalBytes);

            //Assert
            Assert.True(client.Connected);

            client.Shutdown(SocketShutdown.Both);
        }
    }
}
