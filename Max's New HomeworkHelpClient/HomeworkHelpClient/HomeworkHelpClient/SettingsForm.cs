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
    public partial class SettingsForm : Form
    {
        List<ComboBox> classes = new List<ComboBox>();
        List<string> Classes = new List<string>();
        List<string> allClasses = new List<string>();

        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            allClasses.Add("AP Calculus AB");
            allClasses.Add("AP Calculus BC");
            allClasses.Add("Spanish 1");
            allClasses.Add("Spanish 2");
            allClasses.Add("Spanish 3");

            string SchoolStr = "Mountain View High School,Orem High School,Mercer Island High School";
            List<string> schools = SchoolStr.Split(',').ToList();
            schoolCombo.Items.AddRange(schools.ToArray());

            Classes = GetSetting("class").Split(',').ToList();
            for (int i = 0; i < Classes.Count; i++)
            {
                classes.Add(CreateClass(i, allClasses.ToArray()));
                classes[i].SelectedIndex = classes[i].Items.IndexOf(Classes[i]);
            }
        }

        private void ApplyBtn_Click(object sender, EventArgs e)
        {
            List<string> strclasses = new List<string>();
            for (int i = 0; i < classes.Count; i++)
            {
                strclasses.Add(classes[i].Text);
            }
            string ans = string.Join(",", strclasses);
            saveSetting($"class:{ans}");

            saveSetting($"school:{schoolCombo.Text}");
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            List<string> strclasses = new List<string>();
            for (int i = 0; i < classes.Count; i++)
            {
                strclasses.Add(classes[i].Text);
            }
            string ans = string.Join(",", strclasses);
            saveSetting($"class:{ans}");

            saveSetting($"school:{schoolCombo.Text}");

            this.Close();
        }

        private string GetSetting(string setting)
        {
            string settingsStr = File.ReadAllText("settings\\settings.inf");
            List<string> settings = settingsStr.Split('\0').ToList();
            for(int i = 0; i < settings.Count; i++)
            {
                if (settings[i].StartsWith(setting.Split(':')[0]))
                {
                    return settings[i].Split(':')[1];
                }
            }
            return "";
        }

        private void saveSetting(string setting)
        {
            List<string> settings = File.ReadAllText("settings\\settings.inf").Split('\0').ToList();
            for(int i = 0; i< settings.Count; i++)
            {
                if (settings[i].StartsWith(setting.Split(':')[0]))
                {
                    settings[i] = setting;
                    break;
                }
            }
            using(FileStream fs = File.Open("settings\\settings.inf", FileMode.Truncate))
            {
                fs.Write(Encoding.ASCII.GetBytes(string.Join("\0", settings.ToArray())), 0, Encoding.ASCII.GetByteCount(string.Join(",", settings.ToArray())));
            }
        }

        private ComboBox CreateClass(int i, object[] items)
        {
            ComboBox c = new ComboBox();
            c.Parent = classesPage;
            c.Width = 150;
            c.Height = 24;
            c.Location = new Point((this.Width - c.Width) / 2, (i * (c.Height + 15)) + button1.Height + 15);
            c.Items.AddRange(items);
            c.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            c.AutoCompleteSource = AutoCompleteSource.ListItems;
            c.Text = "Select a Class";
            return c;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            classes.Add(CreateClass(classes.Count, allClasses.ToArray()));
        }
    }
}
