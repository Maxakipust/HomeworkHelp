using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeworkHelpClient
{
    class SecureSocket
    {
        public Socket socket;
        public SecureSocket(string ip, int port)
        {
            socket = new Socket(ip, port);
        }
        public void SendString(string message)
        {
            socket.Send(Encoding.ASCII.GetBytes(message), "message");
        }
    }
}
