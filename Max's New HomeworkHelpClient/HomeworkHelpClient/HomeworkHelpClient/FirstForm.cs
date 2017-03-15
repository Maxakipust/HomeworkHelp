using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HomeworkHelpClient
{
    public partial class FirstForm : Form
    {
        public FirstForm()
        {
            InitializeComponent();
        }

        Label label1;
        ComboBox schoolCombo;
        const int LISTSCHOOLS = 0;
        const int LISTCLASSES = 1;
        const int ADDUSER = 2;
        List<ComboBox> Combos = new List<ComboBox>();
        Button addButton;
        string selectedSchool = "";
        List<string> Classes;
        Button nextButton;
        List<string> ClassesTaken = new List<string>();
        TextBox nameBox;
        Button finButton;
        

        private void FirstForm_Load(object sender, EventArgs e)
        {
            /*SS = new SecureSocket("SERVER" ,891);
            SS.SendString("<Command>GetSchools</Command>");
            byte[] bytes = new byte[0];
            string type = "";
            int bytesRead = SS.socket.Listen(ref bytes, ref type);
            string schoolsStr = Encoding.ASCII.GetString(bytes);
            */
            label1 = new Label();
            schoolCombo = new ComboBox();
            label1.Text = "Select Your School";
            schoolCombo.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            CreateCenteredComboAndText(ref schoolCombo, ref label1);
            string schoolsStr = "Mountain View High School";
            List<string> schools = schoolsStr.Split(',').ToList<string>();
            schoolCombo.Items.AddRange(schools.ToArray());
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label1.Text = "Add Your Classes";
            label1.Location = new Point((this.Width - label1.Width) / 2, 5);
            if (schoolCombo.SelectedIndex > -1)
            {
                selectedSchool = schoolCombo.SelectedItem.ToString();
            }
            schoolCombo.Visible = false;
            //SS.SendString($"<Command>{LISTCLASSES}:{selectedSchool}</Command>");

            //byte[] bytes = new byte[0];
            //string type = "";
            //int bytesRead = SS.socket.Listen(ref bytes, ref type);
            //string classesStr = Encoding.ASCII.GetString(bytes);
            List<string> classStr = new List<string>();
            foreach (string line in File.ReadAllLines("settings//" + "Mountain View High School" + ".inf"))
            {
                string newName = line;
                classStr.Add(newName);
            }
            classStr.Sort();
            Classes = classStr.ToList<string>();
            Combos.Add(CreateCombo(Combos.Count + 1, Classes.ToArray()));
            addButton = CreateAddButton(Combos.Count);
            addButton.Location = new Point(addButton.Location.X, addButton.Location.Y + 24);
            nextButton = createButton("Next", this.Width - 175, this.Height - 75, 150, 24);
            nextButton.Click += NextButton_Click;
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            bool c = true;

            for (int i = 0; i < Combos.Count; i++)
            {
                if (!Classes.Contains(Combos[i].Text))
                {
                    Combos[i].Text = "";
                    c = false;
                }
            }
            if (c)
            {
                for (int i = 0; i < Combos.Count; i++)
                {
                    ClassesTaken.Add(Combos[i].Text);
                    Combos[i].Visible = false;
                }
                nextButton.Visible = false;
                addButton.Visible = false;
                label1.Text = "Username:";
                label1.Location = new Point((this.Width - 150) / 2, (this.Height - 24 - label1.Height) / 2);
                nameBox = createTextBox((this.Width - 150) / 2, (this.Height - 24 + label1.Height) / 2, 150, 24);
                finButton = createButton("Finish", this.Width - 175, this.Height - 75, 150, 24);
                finButton.Click += FinButton_Click;
            }
        }

        private void FinButton_Click(object sender, EventArgs e)
        {
            string userName = nameBox.Text;
            using (FileStream fs = File.Exists("settings\\settings.inf")? File.Open("settings\\settings.inf", FileMode.Truncate): File.Create("settings\\settings.inf"))
            {
                string x = $"school:{selectedSchool}\0class:{String.Join(",", ClassesTaken.Distinct().ToArray())}\0name:{userName}";
                fs.Write(Encoding.ASCII.GetBytes(x), 0, Encoding.ASCII.GetByteCount(x));
            }
            this.Close();
        }

        private Button CreateAddButton(int i)
        {
            Button b = new Button();
            b.Parent = panel1;
            b.Width = 150;
            b.Height = 24;
            b.Location = new Point((this.Width - b.Width) / 2, 15);
            b.Text = "Add a Class";
            b.Click += B_Click;
            return b;
        }

        private void B_Click(object sender, EventArgs e)
        {
            Combos.Add(CreateCombo(Combos.Count + 1, Classes.ToArray()));
        }

        private ComboBox CreateCombo(int i, object[] items)
        {
            ComboBox c = new ComboBox();
            c.Parent = panel1;
            c.Width = 150;
            c.Height = 24;
            c.Location = new Point((this.Width - c.Width) / 2, (i * (c.Height + 15)) + label1.Height + 15);
            c.Items.AddRange(items);
            c.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            c.AutoCompleteSource = AutoCompleteSource.ListItems;
            c.Text = "Select a Class";
            return c;
        }
        private void CreateCenteredComboAndText(ref ComboBox cb, ref Label l)
        {
            cb.Parent = panel1;
            cb.Width = 150;
            cb.Height = 24;
            cb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cb.AutoCompleteSource = AutoCompleteSource.ListItems;
            l.Height = 24;
            l.Parent = panel1;
            cb.Location = new Point((this.Width - cb.Width) / 2, (this.Height - (cb.Height + l.Height)) / 2);
            l.Location = new Point((this.Width - cb.Width) / 2, (this.Height - (cb.Height - l.Height)) / 2);
        }

        private Button createButton(string text, int x, int y, int width, int height)
        {
            Button b = new Button();
            b.Parent = this;
            b.Text = text;
            b.Location = new Point(x, y);
            b.Width = width;
            b.Height = height;
            return b;
        }
        private TextBox createTextBox(int x, int y, int width, int height)
        {
            TextBox tb = new TextBox();
            tb.Parent = panel1;
            tb.Location = new Point(x, y);
            tb.Width = width;
            tb.Height = height;
            return tb;
        }
    }
}
