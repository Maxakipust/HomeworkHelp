using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Server
{
    class lobbyNames
    {
        public List<string> inLobby = new List<string>();
        public string className;
        public string backLog;

        public lobbyNames(string newName)
        {
            className = newName;
        }
        public void addToLobby(string name)
        {
            inLobby.Add(name);
        }
        public void removeFromLobby(string name)
        {
            if (inLobby.Contains(name))
                inLobby[inLobby.IndexOf(name)]=null;
        }
        public void addToLog(string message)
        {
            backLog += Environment.NewLine + message;
        }
        public string createBLogData()
        {
            return className + "\0" + backLog;
        }
    }
}
