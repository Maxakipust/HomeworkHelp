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
        Sucure_Socket _socket;
        public privateChat(string with, Sucure_Socket socket)
        {
            InitializeComponent();
            this.Text = with;
            _socket = socket;
        }
    }
}
