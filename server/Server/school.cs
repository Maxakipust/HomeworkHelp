using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Server
{
    class school
    {
        public string name;
        public List<lobbyNames> classes = new List<lobbyNames>();

        public school(string _name)
        {
            name = _name;
            foreach (string line in File.ReadAllLines("Schools//" + name + ".inf"))
            {
                string newName = line;
                classes.Add(new lobbyNames(newName));
            }
        }
        public int whichClassIndex(string classNameQuery)
        {
            for (int i = 0; i < classes.Count; i++)
            {
                if (classes[i].className == classNameQuery) return i;
            }
            return -1;
        }
    }
}
