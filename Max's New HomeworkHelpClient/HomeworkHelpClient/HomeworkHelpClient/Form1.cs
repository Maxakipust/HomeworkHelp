﻿using System;
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
        Settings settings;
        classContainer cc;
        SettingsForm settingsForm;
        ChatClient ss;
        List<privateChat> ch = new List<privateChat>();
        delegate void gmsg(string data);
        delegate void glog(string msg);

        public HHForm()
        {
            InitializeComponent();
        }

        private void HHForm_Load(object sender, EventArgs e)
        {
            settings = new Settings();
            settingsForm = new SettingsForm();
            settingsForm.FormClosed += SettingsForm_FormClosed;
            cc = new classContainer(buttonShowClick,actTutorClick,getTutorClick,classChatClick);
            for(int i = 0; i< settings.GetSetting("class").Split(',').Length; i++)
            {
                cc.add(settings.GetSetting("class").Split(',')[i], panel1);
            }
            ss = new ChatClient(10, newData,settings.GetSetting("name"));
        }

        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            settingsForm = new SettingsForm();
            settings = new Settings();
            cc.del();
            cc = new classContainer(buttonShowClick, actTutorClick, getTutorClick, classChatClick);
            for (int i = 0; i < settings.GetSetting("class").Split(',').Length; i++)
            {
                cc.add(settings.GetSetting("class").Split(',')[i], panel1);
            }
        }

        

        private void buttonShowClick(string name)
        {

        }

        private void actTutorClick(string name)
        {
            //ss.sendString(settings.GetSetting("school") + "\0" +name, "offHelp");
        }
        private void getTutorClick(string name)
        {
            //ss.sendString(settings.GetSetting("school")+"\0" + name, "reqHelp");
        }
        private void classChatClick(string name)
        {
            //ss.sendString(settings.GetSetting("school") + "\0" + name + "\01","lobCon");
            ch.Add(new privateChat(name, ss, settings));
            ch[ch.Count - 1].Show();
        }

        private void editClassesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settingsForm.Show();
        }
        private void newData(string data, string type)
        {
            if (type.StartsWith("cLobMes"))
            {
                string with = data.Split('\0')[1];
                for (int i = 0; i < ch.Count; i++)
                {
                    if (ch[i].Text == with)
                    {
                        gmsg d = ch[i].onGetMessage;
                        Invoke(d,new object[] { data });
                        break;
                    }
                }
            }
            else if (type.StartsWith("bLog"))
            {
                string with = data.Split('\0')[0];
                for (int i = 0; i < ch.Count; i++)
                {
                    if (ch[i].Text == with)
                    {
                        glog d = ch[i].onGetlog;
                        Invoke(d, new object[] { data });
                        break;
                    }
                }
            }
            else if (type.StartsWith("crClass"))
            {

            }
            else if (type.StartsWith("error"))
            {
                throw new Exception(data);
            }
        }
    }
}
