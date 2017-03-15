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
        static public List<school> schools = new List<school>();


        static nameToSocketIndex socketNames = new nameToSocketIndex();
        static TcpServerContainer cc = new TcpServerContainer(new IPEndPoint(Dns.Resolve(Dns.GetHostName()).AddressList[0],10), receiveData);
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
                                    socketNames.index.Add(args[2], servIndex);
                                    enumeratedSchools[schoolIndex].classes[classIndex].addToLobby(args[2]);
                                    cc.sendString(servIndex, enumeratedSchools[schoolIndex].classes[classIndex].createBLogData(), "bLog");
                                    for (int m = 0; m < enumeratedSchools[schoolIndex].classes[classIndex].inLobby.Count; m++)
                                    {
                                        cc.sendString(socketNames.index[enumeratedSchools[schoolIndex].classes[classIndex].inLobby[m]], args[0] + "\0" + args[1] + "\0" + "Server" + "\0" + args[2]+" has entered the lobby.", "cLobMes");
                                    }
                                    enumeratedSchools[schoolIndex].classes[classIndex].addToLog("Server: " + args[2] + " has entered the lobby.");
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
                                    
                                    for (int n = 0; n < enumeratedSchools[schoolIndex].classes[classIndex].inLobby.Count; n++)
                                    {
                                        if (enumeratedSchools[schoolIndex].classes[classIndex].inLobby.Count > 0) cc.sendString(socketNames.index[enumeratedSchools[schoolIndex].classes[classIndex].inLobby[n]], args[0] + "\0" + args[1] + "\0" + "Server" + "\0" + args[2] + " has exited the lobby.", "cLobMes");
                                    }
                                    enumeratedSchools[schoolIndex].classes[classIndex].inLobby.Remove(args[2]);
                                    socketNames.index.Remove(args[2]);
                                    enumeratedSchools[schoolIndex].classes[classIndex].addToLog("Server: " + args[2] + " has exited the lobby.");
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
                                enumeratedSchools[schoolIndex].classes[classIndex].addToLog(args[2] + ": "+ args[3]);
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
