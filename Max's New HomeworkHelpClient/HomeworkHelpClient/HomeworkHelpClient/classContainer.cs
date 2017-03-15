using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HomeworkHelpClient
{
    class classContainer
    {
        public List<ClassControl> classes = new List<ClassControl>();
        public string openClass = "";
        Action<string> _showClick;
        Action<string> _actTutorClick;
        Action<string> _getTutorClick;
        Action<string> _classChatClick;

        public void del()
        {
            for(int i =0;i< classes.Count; i++)
            {
                classes[i].panel.Visible = false;
                classes[i].showButton.Visible = false;
            }
        }

        public classContainer(Action<string> showClick, Action<string> actTutorClick, Action<string> getTutorClick, Action<string> classChatClick)
        {
            _showClick = showClick;
            _actTutorClick = actTutorClick;
            _getTutorClick = getTutorClick;
            _classChatClick = classChatClick;
        }
        public void showClick(string name)
        {
            
            int curHeight = 15;
            for(int i = 0; i < classes.Count; i++)
            {
                int x = classes[i].getWidth();
                if (classes[i].isOpen)
                {
                    if(classes[i].name != name)
                    {
                        classes[i].Close();
                    }
                    else
                    {
                        openClass = name;
                    }
                }
                classes[i].move(15, curHeight);
                curHeight += classes[i].getHeight()+10;
            }
        }
        public void format(string name)
        {
            int curHeight = 15;
            for (int i = 0; i < classes.Count; i++)
            {
                classes[i].move(15, curHeight);
                curHeight += classes[i].getHeight() + 10;
            }
        }
        public void add(string name,Control parent)
        {
            classes.Add(new ClassControl(name, 0, 0, parent, showClick, _actTutorClick, _getTutorClick, _classChatClick,format));
            showClick(name);
        }
    }
}
