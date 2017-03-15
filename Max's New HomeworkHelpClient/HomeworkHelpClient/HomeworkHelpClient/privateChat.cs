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
        
        ChatClient Client;
        Settings setting;

        public privateChat(string with, ChatClient cc,Settings s)
        {
            setting = s;
            Client = cc;
            InitializeComponent();
            this.Text = with;
        }
        public void onGetMessage(string data)
        {
            richTextBox1.Text += data.Split('\0')[2]+": "+data.Split('\0')[3]+Environment.NewLine;
            richTextBox1.SelectionLength = 0;
            richTextBox1.ScrollToCaret();
        }

        private void privateChat_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
            Client.sendString(setting.GetSetting("school") + "\0" + this.Text + "\0" + setting.GetSetting("name") + "\01", "lobCon");
        }

        private void privateChat_FormClosing(object sender, FormClosingEventArgs e)
        {
            Client.sendString(setting.GetSetting("school") + "\0" + this.Text + "\0" + setting.GetSetting("name") + "\00", "lobCon");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Client.sendString(setting.GetSetting("school") + "\0" + this.Text + "\0" + setting.GetSetting("name") +"\0"+ textBox1.Text, "lobMes");
            textBox1.Text = "";
            textBox1.Focus();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                Client.sendString(setting.GetSetting("school") + "\0" + this.Text + "\0" + setting.GetSetting("name") + "\0" + textBox1.Text, "lobMes");
                textBox1.Text = "";
            }
        }
    }
}