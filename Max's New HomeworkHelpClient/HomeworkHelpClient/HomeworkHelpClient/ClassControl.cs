using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace HomeworkHelpClient
{
    class ClassControl
    {
        public string name = "";
        public bool isOpen = false;

        private Action<string> _showClick;
        private Action<string> _actTutorClick;
        private Action<string> _getTutorClick;
        private Action<string> _classChatClick;
        private Action<string> _format;


        public Button showButton = new Button();

        public Panel panel = new Panel();
            public Button actAsTutor = new Button();
            public Button getTutor = new Button();
            public Button classChat = new Button();

        public ClassControl(string className, int x, int y, Control parent, Action<string> showClick, Action<string> actAsTutorClick, Action<string> getTutorClick, Action<string> classChatClick, Action<string> format)
        {
            name = className;

            _showClick = showClick;
            _actTutorClick = actAsTutorClick;
            _classChatClick = classChatClick;
            _getTutorClick = getTutorClick;
            _format = format;

            //set parent
            showButton.Parent = parent;
            panel.Parent = parent;
            actAsTutor.Parent = panel;
            getTutor.Parent = panel;
            classChat.Parent = panel;
            
            //set text
            showButton.Text = className;
            actAsTutor.Text = "Help Someone";
            getTutor.Text = "Get Help";
            classChat.Text = "Class Chat";

            //set AutoSize
            showButton.AutoSize = true;
            panel.AutoSize = true;
            actAsTutor.AutoSize = true;
            getTutor.AutoSize = true;
            classChat.AutoSize = true;

            //set location
            showButton.Location = new Point(x, y);
            panel.Location = new Point(x + (showButton.Width / 2), y + showButton.Height + 5);
            classChat.Location = new Point(0, 0);
            getTutor.Location = new Point(0, classChat.Height + 5);
            actAsTutor.Location = new Point(0, classChat.Height + getTutor.Height + 10);

            //setTriggers
            showButton.Click += ShowButton_Click;
            classChat.Click += ClassChat_Click;
            getTutor.Click += GetTutor_Click;
            actAsTutor.Click += ActAsTutor_Click;

            //set visible
            panel.Visible = false;

            //set height
            panel.Height = classChat.Height + getTutor.Height + actAsTutor.Height + 15;
            panel.Width = actAsTutor.Width;
        }


        private void ActAsTutor_Click(object sender, EventArgs e)
        {
            _actTutorClick(name);
        }

        private void GetTutor_Click(object sender, EventArgs e)
        {
            _getTutorClick(name);
        }

        private void ClassChat_Click(object sender, EventArgs e)
        {
            _classChatClick(name);
        }

        private void ShowButton_Click(object sender, EventArgs e)
        {
            isOpen = !isOpen;
            panel.Visible = isOpen;
            _showClick(name);
        }
        public void Close()
        {
            isOpen = false;
            panel.Visible = false;
            _format(name);
        }

        public int getHeight()
        {
            if(panel.Visible)
                return showButton.Height + 5 + panel.Height;
            return showButton.Height;
        }
        
        public int getWidth()
        {
            if (panel.Visible)
                return showButton.Width / 2 + panel.Width;
            return showButton.Width;
        }

        public void move(int x, int y)
        {
            //set location
            showButton.Location = new Point(x, y);
            panel.Location = new Point(x + (showButton.Width / 2), y + showButton.Height + 5);
            classChat.Location = new Point(0, 0);
            getTutor.Location = new Point(0, classChat.Height + 5);
            actAsTutor.Location = new Point(0, classChat.Height + getTutor.Height + 10);
        }
    }
}
