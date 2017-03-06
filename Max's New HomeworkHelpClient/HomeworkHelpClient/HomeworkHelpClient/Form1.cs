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
        List<privateChat> chats;
        List<string> classes;
        List<string> settings;
        classContainer cc;
        SettingsForm settingsForm;
        Sucure_Socket ss;

        public HHForm()
        {
            InitializeComponent();
        }

        private void HHForm_Load(object sender, EventArgs e)
        {
            ss = new Sucure_Socket(10,newData);
            
            settingsForm = new SettingsForm();
            settingsForm.FormClosed += SettingsForm_FormClosed;
            cc = new classContainer(buttonShowClick,actTutorClick,getTutorClick,classChatClick);
            settings = File.ReadAllText("settings\\settings.inf").Split('\0').ToList();
            classes = GetSetting("class").Split(',').ToList();
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

        private void buttonShowClick(string name)
        {

        }

        private void actTutorClick(string name)
        {
            ss.sendString(settings[2].Split(':')[1] + "\0" + settings[1].Split(':')[1], "offHelp");
        }
        private void getTutorClick(string name)
        {
            ss.sendString(settings[2].Split(':')[1] + "\0" + settings[1].Split(':')[1], "reqHelp");
        }
        private void classChatClick(string name)
        {
            ss.sendString(settings[0].Split(':')[1] + "\0" + name + "\01","lobCon");
        }

        private void editClassesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settingsForm.Show();
        }
        private void newData(byte[] data, string type)
        {
            if (type.StartsWith("cMes"))
            {
                string with = type.Split('\0')[1];
                for(int i = 0; i< chats.Count; i++)
                {
                    if(chats[i].Text == with)
                    {
                        chats[i].onGetMessage(Encoding.ASCII.GetString(data));
                        break;
                    }
                }
                chats.Add(new privateChat(with));
            }
            else if (type.StartsWith("error"))
            {
                throw new Exception(Encoding.ASCII.GetString(data));
            }
            
        }
    }
}
