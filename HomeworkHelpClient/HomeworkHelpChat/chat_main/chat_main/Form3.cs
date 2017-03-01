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
    public partial class Form3 : Form
    {
        int numOfBoxes = 1;
        int y = 30;
        List<ComboBox> cmb;
        string[] fakeClasses = { "Anatomy", "Biology", "Biotech", "Calculus", "Environmental Science", "Health", "Lab", "Orchestra", "Physical Education", "Quilting" };
        public Form3()
        {
            
            InitializeComponent();
            cmb = new List<ComboBox>();
            cmb.Add(new ComboBox());
            cmb[0].Parent = panel1;
            cmb[0].Items.AddRange(fakeClasses);
            cmb[0].Text = "Choose a Class";
            cmb[0].Size = new Size(180, 00);
            cmb[0].Location = new Point( 9, 0);
            cmb[0].AutoCompleteMode = AutoCompleteMode.Append;
            cmb[0].AutoCompleteSource = AutoCompleteSource.ListItems;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            cmb.Add(new ComboBox());
            cmb[numOfBoxes].Parent = panel1;
            cmb[numOfBoxes].Items.AddRange(fakeClasses);
            cmb[numOfBoxes].Text = "Choose a Class";
            cmb[numOfBoxes].Size = new Size(180, 00);
            cmb[numOfBoxes].Location = new Point(9, y);
            cmb[numOfBoxes].AutoCompleteMode = AutoCompleteMode.Append;
            cmb[numOfBoxes].AutoCompleteSource = AutoCompleteSource.ListItems;
            numOfBoxes++;
            y += 30;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
