﻿using System;
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
        private Action<string, string,int> msgEvent;

        public TcpServerContainer(IPEndPoint ep, Action<string,string,int> msgGot)
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
            Servers.Add(new TcpSocketServer(Listener.AcceptTcpClient(),msgEvent,leave, getIndex));
            //sendString(Servers.Count, Servers[Servers.Count-1].id.ToString(), "ID");
        }
        public int getIndex(TcpSocketServer server)
        {
            return Servers.IndexOf(server);
        }
        public void leave(TcpSocketServer serv)
        {

            int c = Servers.IndexOf(serv);
            Servers[c].client.Client.Close();
            Servers[c].on = false;
            Servers[c] = null;
            //Servers.RemoveAt(c);
        }

        public void updateServers()
        {
            for (int i = 0; i < Servers.Count; i++)
            {
                if (!Servers[i].client.Connected)
                {
                    Servers[i].client.Close();
                    Servers[i].on = false;
                    //Servers.RemoveAt(i);
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
            if(index>-1)
                if(Servers[index]!=null)
                Servers[index].send(Encoding.ASCII.GetBytes(data), type);
        }
    }
}
