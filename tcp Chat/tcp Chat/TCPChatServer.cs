using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;

namespace tcp_Chat
{
    class TCPChatServer
    {
        public List<TCPServer> ConnectedServers;
        private TcpListener Listener;
        private BackgroundWorker ListenWorker;

        public TCPChatServer(IPEndPoint ep)
        {
            Console.WriteLine("hosting on " + ep.ToString());
            Listener = new TcpListener(ep);
            Listener.Start();
            ConnectedServers = new List<TCPServer>();
            ListenWorker = new BackgroundWorker();
            ListenWorker.DoWork += ListenWorker_DoWork;
            ListenWorker.RunWorkerCompleted += ListenWorker_RunWorkerCompleted;
            ListenWorker.RunWorkerAsync();
        }

        private void ListenWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ListenWorker.RunWorkerAsync();
        }

        private void ListenWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            ConnectedServers.Add(new TCPServer(Listener.AcceptTcpClient(), findNewID(), MsgRec));
        }
        private void MsgRec(string msg, int id)
        {
            updateServers();
            Console.WriteLine(msg);
            for(int i = 0; i< ConnectedServers.Count; i++)
            {
                if (ConnectedServers[i].id != id)
                {
                    ConnectedServers[i].send(Encoding.ASCII.GetBytes(msg), "text");
                }
            }
        }
        public int findNewID()
        {
            List<int> usedIDs = new List<int>();
            for (int i = 0; i < ConnectedServers.Count; i++)
            {
                usedIDs.Add(ConnectedServers[i].id);
            }
            usedIDs.Sort();

            for (int i = 0; i < usedIDs.Count - 1; i++)
            {
                if (usedIDs[i] + 1 != usedIDs[i + 1])
                {
                    return i + 1;
                }
            }
            return usedIDs.Count;
        }
        public void updateServers()
        {
            List<int> remove = new List<int>();
            for(int i = 0; i< ConnectedServers.Count; i++)
            {
                if (!ConnectedServers[i].connected())
                {
                    remove.Add(i);
                }
            }
            for(int i = 0; i< remove.Count; i++)
            {
                ConnectedServers.RemoveAt(remove[i]);
            }
        }
    }
}
