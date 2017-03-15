﻿using System;
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
        static public List<school> schools = new List<school>();


        static nameToSocketIndex socketNames = new nameToSocketIndex();

        //static TcpServerContainer cc = new TcpServerContainer(new IPEndPoint(Dns.Resolve(Dns.GetHostName()).AddressList[0],10), receiveData);
        static TcpServerContainer cc = new TcpServerContainer(new IPEndPoint(IPAddress.Parse("172.20.10.3"), 10), receiveData);
        static List<school> enumeratedSchools = new List<school>();
        static void Main(string[] args)
        {
            Console.Title = cc.hostIP;
            string[] fileNames = Directory.GetFiles("Schools");
            for (int i = 0; i < fileNames.Length; i ++)
            {
                string fname = Path.GetFileName(fileNames[i]);
                enumeratedSchools.Add(new school(fname.Substring(0,fname.LastIndexOf("."))));
            }
            Console.ReadLine();
        }
        static void receiveData(string data, string type, int servIndex)
        {
            Console.WriteLine(data + ":" + type);
            if (type.StartsWith("lobCon"))
            {
                string[] args = data.Split('\0');
                string fileData = System.IO.File.ReadAllText("lobList.inf");
                if (args[3] == "1")
                {
                    socketNames.index.Add(args[2], servIndex);
                    for (int i = 0; i < enumeratedSchools.Count; i++)
                    {
                        if (enumeratedSchools[i].name == args[0])
                        {
                            int schoolIndex = i;
                            for(int j = 0; j < enumeratedSchools[schoolIndex].classes.Count; j++)
                            {
                                if (enumeratedSchools[schoolIndex].classes[j].className == args[1])
                                {
                                    int classIndex = j;
                                    enumeratedSchools[schoolIndex].classes[classIndex].addToLobby(args[2]);
                                    for (int m = 0; m < enumeratedSchools[schoolIndex].classes[classIndex].inLobby.Count; m++)
                                    {
                                        cc.sendString(enumeratedSchools[schoolIndex].classes[classIndex].inLobby[m])
                                    }
                                }
                            }
                        }
                    }
                }
                if (args[3] == "0")
                {
                    for (int i = 0; i < enumeratedSchools.Count; i++)
                    {
                        if (enumeratedSchools[i].name == args[0])
                        {
                            int schoolIndex = i;
                            for (int j = 0; j < enumeratedSchools[schoolIndex].classes.Count; j++)
                            {
                                if (enumeratedSchools[schoolIndex].classes[j].className == args[1])
                                {
                                    int classIndex = j;
                                    
                                    enumeratedSchools[schoolIndex].classes[classIndex].removeFromLobby(args[2]);
                                    socketNames.index.Remove(args[2]);
                                }
                            }
                        }
                    }
                }
            }
            if (type.StartsWith("lobMes"))
            {
                string[] args = data.Split('\0');
                string fileData = System.IO.File.ReadAllText("lobList.inf");
                for (int i = 0; i < enumeratedSchools.Count; i++)
                {
                    if (enumeratedSchools[i].name == args[0])
                    {
                        int schoolIndex = i;
                        for (int j = 0; j < enumeratedSchools[schoolIndex].classes.Count; j++)
                        {
                            if (enumeratedSchools[schoolIndex].classes[j].className == args[1])
                            {
                                int classIndex = j;
                                string[] lobbyNames = enumeratedSchools[schoolIndex].classes[classIndex].inLobby.ToArray();
                                for (int k = 0; k < enumeratedSchools[schoolIndex].classes[j].inLobby.Count; k++)
                                {
                                    if (enumeratedSchools[schoolIndex].classes[classIndex].inLobby[k] != null)
                                    { 
                                        cc.sendString(socketNames.index[lobbyNames[k]], data, "cLobMes");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (type.StartsWith("pMes"))
            {

            }
            if (type.StartsWith("offHelp"))
            {

            }
            if (type.StartsWith("reqHelp"))
            {

            }
            if (type.StartsWith("nameEdit"))
            {

            }
            if (type.StartsWith("errMes"))
            {

            }
        }
    }
}
