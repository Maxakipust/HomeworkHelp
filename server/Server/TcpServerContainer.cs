using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.ComponentModel;
using System.IO;

namespace Server
{
    class TcpServerContainer
    {
        public List<TcpSocketServer>Servers;
        private TcpListener Listener;
        private BackgroundWorker AcceptWorker;
        public string hostIP;
        private Action<string, string> msgEvent;

        public TcpServerContainer(IPEndPoint ep, Action<string,string> msgGot)
        {
            msgEvent = msgGot;
            

            Listener = new TcpListener(ep);
            Listener.Start();
            Servers = new List<TcpSocketServer>();
            hostIP = Listener.LocalEndpoint.ToString();
            AcceptWorker = new BackgroundWorker();
            AcceptWorker.DoWork += AcceptWorker_DoWork;
            AcceptWorker.RunWorkerCompleted += AcceptWorker_RunWorkerCompleted;
            AcceptWorker.RunWorkerAsync();
        }

        public List<string> getIPs()
        {
            updateServers();
            List<string> ret = new List<string>();
            for(int i = 0; i< Servers.Count; i++)
            {
                ret.Add(Servers[i].client.Client.RemoteEndPoint.ToString());
            }
            return ret;
        }

        private void AcceptWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            AcceptWorker.RunWorkerAsync();
        }
        public int IDfinder(int id)
        {
            for (int i = 0; i < Servers.Count; i++){
                if (Servers[i].id == id)
                {
                    return i;
                }
            }
            return -1;
        }
        void AcceptWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Servers.Add(new TcpSocketServer(Listener.AcceptTcpClient(),msgEvent));
            sendString(Servers.Count, Servers[Servers.Count-1].id.ToString(), "ID");
        }

        public void updateServers()
        {
            for (int i = 0; i < Servers.Count; i++)
            {
                if (!Servers[i].client.Connected)
                {
                    Servers[i].client.Close();
                    Servers.RemoveAt(i);
                }
            }
        }

        public void send(int index,byte[] data,string type)
        {
            Servers[index].send(data,type);
        }

        public void sendFile(int index, string path)
        {
            Servers[index].send(File.ReadAllBytes(path),"file*"+path+"*");
        }

        public void sendString(int index, string data, string type)
        {
            if(index>0)
                Servers[index-1].send(Encoding.ASCII.GetBytes(data), type);
        }
    }
}
