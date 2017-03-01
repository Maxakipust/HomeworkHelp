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
        Form3 addClass = new Form3();
        Form4 removeClass = new Form4();
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Opens About box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Random r = new Random();
            string firstSpace = randspace(r, 7);
            string secondSpace = randspace(r, 70);
            string thridSpace = randspace(r, 70);
            string fourthSpace = randspace(r, 70);
            string fifthSpace = randspace(r, 70);
            MessageBox.Show(firstSpace+ "Homework Helpler 2017\n"+secondSpace+"Created by:\n"+thridSpace+"Daniel Woodal\n"+fourthSpace+"Max Kipust\n"+fifthSpace+"Nicholas Hoffman", "Creators");
        }
        /// <summary>
        /// Induces OCD Seizures
        /// </summary>
        /// <param name="rand">opens object rand</param>
        /// <param name="max">Max number of spaces before each line of Text</param>
        /// <returns></returns>
        private string randspace(Random rand, int max)
        {
            string ans = "";
            int n = rand.Next(max);
            for (int i = 0; i < n; i++)
            {
                ans = ans + " ";
            }
            return ans;
        }

        private void profileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settingsForm.Show();
            settingsForm.FormClosed += SettingsForm_FormClosed;
        }

        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            settingsForm = new Form2();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addClass.Show();
            addClass.FormClosed += AddClass_FormClosed;
        }

        private void AddClass_FormClosed(object sender, FormClosedEventArgs e)
        {
            addClass = new Form3();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            removeClass.Show();
            removeClass.FormClosed += RemoveClass_FormClosed;
        }

        private void RemoveClass_FormClosed(object sender, FormClosedEventArgs e)
        {
            removeClass = new Form4();
        }
    }
}
