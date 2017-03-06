using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HomeworkHelpClient
{
    public partial class privateChat : Form
    {
        
        public privateChat(string with)
        {
            InitializeComponent();
            this.Text = with;
        }
        public void onGetMessage(string data)
        {
            richTextBox1.Text += data;
        }
    }
}
