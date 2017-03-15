using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class nameToSocketIndex
    {
        List<string> names = new List<string>();
        public nameToSocketIndex()
        {

        }
        public void addName(string newName)
        {
            names.Add(newName);
        }
        public int returnIndex(string nameSearch)
        {
            for(int i = 0; i < names.Count; i++)
            {
                if (names[i] == nameSearch) return i;
            }
            return 0;
        }
    }
}
