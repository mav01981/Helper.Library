using Helper.Socket.Interfaces;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Helper.Socket
{
    public class SocketServer : ISocketServer
    {
        public static ManualResetEvent AllDone = new ManualResetEvent(false);

        public event Action<string> RaiseNotification;

        public event Action<byte[]> RaiseAction;

        private readonly System.Net.Sockets.Socket listener;

        public void Start(int port)
        {
            // Establish the local endpoint for the socket.  
            // The DNS name of the computer  
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

            // Create a TCP/IP socket.  
            System.Net.Sockets.Socket listener = new System.Net.Sockets.Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    // Set the event to nonsignaled state.  
                    AllDone.Reset();

                    // Start an asynchronous socket to listen for connections.  
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);

                    RaiseNotification?.Invoke($"Server Listening on port {port}");
                    // Wait until a connection is made before continuing.  
                    AllDone.WaitOne();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.  
            AllDone.Set();

            // Get the socket that handles the client request.  
            System.Net.Sockets.Socket listener = (System.Net.Sockets.Socket)ar.AsyncState;
            System.Net.Sockets.Socket handler = listener.EndAccept(ar);

            // Create the state object.  
            var state = new StateObject {workSocket = handler};

            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }
        private void ReadCallback(IAsyncResult ar)
        {
            // Retrieve the state object and the handler socket  
            // from the asynchronous state object.  
            var state = (StateObject)ar.AsyncState;
            var handler = state.workSocket;

            // Read data from the client socket.   
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                RaiseAction?.Invoke(state.buffer);

                handler.BeginSend(state.buffer, 0, state.buffer.Length, 0,
                    new AsyncCallback(SendCallback), handler);
            }
        }
        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                System.Net.Sockets.Socket handler = (System.Net.Sockets.Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}