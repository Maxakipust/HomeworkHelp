using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chat_main
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(schoolIn.ToString() != "") rewrite("school:" + schoolIn.Text);
            if(nameIn.ToString() != "") rewrite("name:" + nameIn.Text);
        }
        void rewrite(string insert)
        {
            string data = System.IO.File.ReadAllText("..\\..\\..\\..\\..\\..\\settings\\settings.inf");
            string[] sets = data.Split('\0');
            for(int i = 0; i < sets.Length; i++)
            {
                if (sets[i].StartsWith(insert.Split(':')[0]))
                {
                    sets[i] = insert;
                    break;
                }
            }
            data = string.Join("\0", sets);
            using (System.IO.FileStream fs = System.IO.File.Open("..\\..\\..\\..\\..\\..\\settings\\settings.inf", System.IO.FileMode.Truncate))
            {
                fs.Write(Encoding.ASCII.GetBytes(data), 0, Encoding.ASCII.GetByteCount(data));
            }
        }
    }
}
