using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace HomeworkHelpClient
{
    public partial class Form1 : Form
    {
        static SecureSocket SS;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SS = new SecureSocket("fhuiafsdv;c" ,420666);

        }
    }
}
