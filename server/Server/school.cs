using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class school
    {
        public string name;
        public List<string> classes = new List<string>();

        public school(string classLists, string _name)
        {
            name = _name;
            classes = classLists.Split('\0').ToList();
        }
    }
}
