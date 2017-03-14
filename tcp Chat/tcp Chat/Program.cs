using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace tcp_Chat
{
    class Program
    {
        static TCPChatServer server;
        static void Main(string[] args)
        {
            server = new TCPChatServer(new IPEndPoint(Dns.Resolve(Dns.GetHostName()).AddressList[2], 40));
            Console.WriteLine("Press ENTER to continue");
            Console.ReadLine();
        }
        
    }
}
