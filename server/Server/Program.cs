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


        static public nameToSocketIndex socketNames = new nameToSocketIndex();
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
        static void receiveData(string data, string type)
        {
            Console.WriteLine(data + ":" + type);
            if (type.StartsWith("lobCon"))
            {
                string[] args = data.Split('\0');
                string fileData = System.IO.File.ReadAllText("lobList.inf");
                //When user Enters Lobby
                if (args[3] == "1")
                {
                    socketNames.addName(args[2]);
                    for (int i = 0; i < enumeratedSchools.Count; i++)
                    {
                        if (enumeratedSchools[i].name == args[0])
                        {
                            int schoolIndex = i;
                            for(int j = 0; j < enumeratedSchools[schoolIndex].classes.Count; j++)
                            {
                                if (enumeratedSchools[schoolIndex].classes[j].className == args[1])
                                {
                                    for (int l = 0; l < enumeratedSchools[schoolIndex].classes[j].inLobby.Count; l++)
                                    {
                                        cc.sendString(socketNames.returnIndex(enumeratedSchools[schoolIndex].classes[j].inLobby[l]), args[0] + "\0" + args[1] + "\0" + "Server" + "\0" + args[2] + " has entered the Lobby.", "cLobMes");
                                    }
                                    int classIndex = j;
                                    enumeratedSchools[schoolIndex].classes[classIndex].addToLobby(args[2]);
                                }
                            }
                        }
                    }
                }
                //When user leaves Lobby
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
                                    for(int l = 0; l < enumeratedSchools[schoolIndex].classes[j].inLobby.Count; l++)
                                    {
                                        cc.sendString(socketNames.returnIndex(enumeratedSchools[schoolIndex].classes[classIndex].inLobby[l]), args[0]+"\0"+args[1]+ "\0" + "Server" + "\0" + args[2]+" has left the Lobby.", "cLobMes");
                                    }
                                    enumeratedSchools[schoolIndex].classes[classIndex].removeFromLobby(args[2]);
                                }
                            }
                        }
                    }
                }
            }
            //Lobby message type
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
                                string part1 = enumeratedSchools[schoolIndex].name + "\0" + enumeratedSchools[schoolIndex].classes[classIndex].className + "\0";
                                for (int k = 0; k < enumeratedSchools[schoolIndex].classes[j].inLobby.Count; k++)
                                {
                                    if (enumeratedSchools[schoolIndex].classes[classIndex].inLobby[k] != null)
                                    {
                                        string part2 = args[2] + "\0" + args[3];
                                        if(enumeratedSchools[schoolIndex].classes[classIndex].inLobby[k] != "\0")
                                        {
                                            cc.sendString(socketNames.returnIndex(enumeratedSchools[schoolIndex].classes[classIndex].inLobby[k]), part1 + part2, "cLobMes");
                                        }
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
