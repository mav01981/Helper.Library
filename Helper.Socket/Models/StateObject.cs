using System.Text;

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
}
