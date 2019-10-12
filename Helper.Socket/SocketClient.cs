using Helper.Socket.Interfaces;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Helper.Socket
{
    // State object for receiving data from remote device.  
    public class StateObject
    {
        // Client socket.  
        public System.Net.Sockets.Socket workSocket = null;

        // Size of receive buffer.  
        public static int BufferSize = 512000;

        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];

        // Received data string.  
        public StringBuilder sb = new StringBuilder();
    }

    public class SocketClient : ISocketClient
    {
        // ManualResetEvent instances signal completion.  
        private static readonly ManualResetEvent connectDone = new ManualResetEvent(false);

        private static readonly ManualResetEvent sendDone = new ManualResetEvent(false);

        private static readonly ManualResetEvent receiveDone = new ManualResetEvent(false);

        // The response from the remote device.  
        private static string _response = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public System.Net.Sockets.Socket Connect(string host, int port)
        {
            // Establish the remote endpoint for the socket.  
            // The name of the   
            // remote device is "host.contoso.com".  
            IPHostEntry ipHostInfo = Dns.GetHostEntry(host);
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

            // Create a TCP/IP socket.  
            var client = new System.Net.Sockets.Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Connect to the remote endpoint.  
            client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
            connectDone.WaitOne();

            return client;
        }
        public void Send(System.Net.Sockets.Socket client, byte[] byteData)
        {
            // Begin sending the data to the remote device.  
            client.BeginSend(byteData, 0, byteData.Length, SocketFlags.None,
                new AsyncCallback(SendCallback), client);
        }
        public void Receive(System.Net.Sockets.Socket client)
        {
            try
            {
                // Create the state object.  
                var state = new StateObject
                {
                    workSocket = client
                };

                // Begin receiving the data from the remote device.  
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                var client = (System.Net.Sockets.Socket)ar.AsyncState;

                // Complete the connection.  
                client.EndConnect(ar);

                Console.WriteLine("Socket connected to {0}",
                    client.RemoteEndPoint.ToString());

                // Signal that the connection has been made.  
                connectDone.Set();
            }
            catch (Exception)
            {
                throw;
            }
        }
        private static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket   
                // from the asynchronous state object.  
                StateObject state = (StateObject)ar.AsyncState;
                System.Net.Sockets.Socket client = state.workSocket;

                // Read data from the remote device.  
                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.  
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                    // Get the rest of the data.  
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    // All the data has arrived; put it in response.  
                    if (state.sb.Length > 1)
                    {
                        _response = state.sb.ToString();
                    }

                    // Signal that all bytes have been received.  
                    receiveDone.Set();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                System.Net.Sockets.Socket client = (System.Net.Sockets.Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                var bytesSent = client.EndSend(ar);
                //Console.WriteLine("Sent {0} bytes to server.", bytesSent);
                //To do raise event.

                // Signal that all bytes have been sent.  
                sendDone.Set();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
