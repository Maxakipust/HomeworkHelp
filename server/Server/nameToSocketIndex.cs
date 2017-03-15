using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class nameToSocketIndex
    {
        public List<string> names = new List<string>();
        public List<int> index = new List<int>();
        public nameToSocketIndex()
        {

        }
        public void addName(string newName)
        {
            names.Add(newName);
            index.Add(howMany());
        }
        public void removeName(string name)
        {
            for(int i = 0; i < names.Count; i++)
            {
                if (names[i] == name) names[i] = "\0";
            }
        }
        public int returnIndex(string nameSearch)
        {
            for(int i = 0; i < names.Count; i++)
            {
                if (names[i] == nameSearch) return index[i];
            }
            return -1;
        }
        public int howMany()
        {
            if (index.Count == 0) return 0;
            return index.Count;
        }
    }
}
