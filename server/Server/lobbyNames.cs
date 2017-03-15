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
            for (int i = 0; i < inLobby.Count; i++)
            {
                if (inLobby[i] == name)
                {
                    inLobby[i] = "\0";
                    break;
                }
            }
        }
    }
}
