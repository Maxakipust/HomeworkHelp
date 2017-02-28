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
    public partial class Form1 : Form
    {
        Form2 settingsForm = new Form2();
        public Form1()
        {
            InitializeComponent();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Homework Helpler 2017\nCreated by:\nDaniel Woodal\nMax Kipust\nNicholas Hoffman\n");
        }

        private void profileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settingsForm.Show();
        }
    }
}
