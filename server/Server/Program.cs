using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace Server
{
    class Program
    {
        TcpClient client;
        NetworkStream netStream;
        static void Main(string[] args)
        {
            string[] fileNames = Directory.GetFiles("Schools");
            school MVHS = new Server.school(fileNames[0]);

        }
    }
}
