using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPChatClient
{
    class Program
    {
        static ChatClient client;
        static void Main(string[] args)
        {
            Console.Write("Name: ");
            Console.Title = "Chat";
            client = new ChatClient(40, rec, Console.ReadLine());
            Console.Title += " " + client.N;
            Console.WriteLine("connected "+client.client.Client.RemoteEndPoint.ToString());
            client.send(Encoding.ASCII.GetBytes(client.N), "name");
            while (true)
            {
                string msg = Console.ReadLine();
                client.send(Encoding.ASCII.GetBytes(client.N + ": " + msg),"text");
            }
        }
        static void rec(string data)
        {
            Console.WriteLine(data);
        }
    }
}
