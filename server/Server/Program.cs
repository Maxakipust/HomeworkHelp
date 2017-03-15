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
                enumeratedSchools.Add(new school(fileNames[i]));
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
                if (args[3] == "1")
                {
                    for(int i = 0; i < enumeratedSchools.Count; i++)
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
                                string part1 = enumeratedSchools[schoolIndex].name + "\0" + enumeratedSchools[schoolIndex].classes[classIndex].className + "\0"; 
                                for(int k = 0; k < enumeratedSchools[schoolIndex].classes[j].inLobby.Count; k ++)
                                {
                                    if(args[2] != enumeratedSchools[schoolIndex].classes[j].inLobby[k])
                                    {
                                        string part2 = enumeratedSchools[schoolIndex].classes[classIndex].inLobby[k] + "\0" + args[3];
                                        cc.sendString(socketNames.returnIndex(enumeratedSchools[schoolIndex].classes[classIndex].inLobby[k]), part1 + part2, "cLobMes");
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
