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
    public partial class HHForm : Form
    {
        List<string> settings;
        classContainer cc;
        SettingsForm settingsForm;

        public HHForm()
        {
            InitializeComponent();
        }

        private void HHForm_Load(object sender, EventArgs e)
        {
            settingsForm = new SettingsForm();
            settingsForm.FormClosed += SettingsForm_FormClosed;
            cc = new classContainer(buttonShowClick,actTutorClick,getTutorClick,classChatClick);
            settings = File.ReadAllText("settings\\settings.inf").Split('\0').ToList();
            List<string> classes = GetSetting("class").Split(',').ToList();
            for(int i = 0; i< classes.Count; i++)
            {
                cc.add(classes[i], panel1);
            }
        }

        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            settingsForm = new SettingsForm();
        }

        private string GetSetting(string name)
        {
            for(int i = 0; i < settings.Count; i++)
            {
                if (settings[i].StartsWith(name))
                {
                    return settings[i].Split(':')[1];
                }
            }
            return "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Text = cc.classes[0].getWidth().ToString()+":"+panel1.Width;
        }

        private void buttonShowClick(string name)
        {

        }
        private void actTutorClick(string name)
        {
            label1.Text = "Connecting you with someone in need";
            textBox1.Enabled = true;
            button1.Enabled = true;
        }
        private void getTutorClick(string name)
        {
            label1.Text = "Connecting you with someone to help";
            textBox1.Enabled = true;
            button1.Enabled = true;
        }
        private void classChatClick(string name)
        {
            label1.Text = $"Chating with {name}";
            textBox1.Enabled = true;
            button1.Enabled = true;
        }

        private void editClassesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settingsForm.Show();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog SFD = new SaveFileDialog();
            SFD.FileName = cc.openClass;
            SFD.AddExtension = true;
            SFD.Filter = "Text File|*.txt";
            if(DialogResult.OK== SFD.ShowDialog())
            {
                using (FileStream fs = File.Exists(SFD.FileName) ? File.Open(SFD.FileName,FileMode.Truncate) : File.Create(SFD.FileName))
                {
                    fs.Write(Encoding.ASCII.GetBytes(richTextBox1.Text), 0, Encoding.ASCII.GetByteCount(richTextBox1.Text));
                }
            }
        }
    }
}
