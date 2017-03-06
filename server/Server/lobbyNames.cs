using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class lobbyNames
    {
        List<string> inLobby = new List<string>();

        public lobbyNames()
        {

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
