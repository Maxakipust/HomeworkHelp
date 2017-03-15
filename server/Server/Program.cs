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


        static public nameToSocketIndex socketNames = new nameToSocketIndex();
        static TcpServerContainer cc = new TcpServerContainer(new IPEndPoint(Dns.Resolve(Dns.GetHostName()).AddressList[0],10), receiveData);
        static List<school> enumeratedSchools = new List<school>();

        static public int schoolIndexFinder(string schoolName)
        {
            for (int i = 0; i < enumeratedSchools.Count; i++)
            {
                if (enumeratedSchools[i].name == schoolName) return i;
            }
            return -1;
        }

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
                    enumeratedSchools[schoolIndexFinder(args[0])].classes[enumeratedSchools[schoolIndexFinder(args[0])].whichClassIndex(args[1])].inLobby.Add(args[2]);
                    bool copy = false;
                    for (int i = 0; i < socketNames.names.Count; i++)
                    {
                        if (args[2] == socketNames.names[i]) copy = true;
                    }
                    if (!copy)
                    {
                        socketNames.names.Add(args[2]);
                        socketNames.index.Add(socketNames.howMany());
                    }
                }
                //When user leaves Lobby
                if (args[3] == "0")
                {
                    enumeratedSchools[schoolIndexFinder(args[0])].classes[enumeratedSchools[schoolIndexFinder(args[0])].whichClassIndex(args[1])].inLobby.Remove(args[2]);
                }
                   
            }
            //Lobby message type
            if (type.StartsWith("lobMes"))
            {
                string[] args = data.Split('\0'); 
                for (int i = 0; i < socketNames.names.Count; i++)
                {
                    if(socketNames.names[i] != null) cc.sendString(socketNames.index[socketNames.returnIndex(socketNames.names[i])], data, "cLobMes");
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
