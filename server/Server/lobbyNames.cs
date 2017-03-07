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
        List<string> inLobby = new List<string>();
        public string className;

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
            if (inLobby.Contains(name)) inLobby.RemoveAt(inLobby.IndexOf(name));
        }
    }
}
