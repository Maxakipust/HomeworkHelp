using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HomeworkHelpClient
{
    public class Settings
    {
        private List<string> S = new List<string>();
        
        public Settings()
        {
            while (S.Count==0)
            {
                List<string> settings = File.ReadAllText("settings\\settings.inf").Split('\0').ToList();
                if (settings.Count != 3)
                {
                    SettingsForm sf = new SettingsForm();
                    sf.Show();
                }
                else
                {
                    S = settings;
                }
            }
        }
        public string GetSetting(string name)
        {
            for (int i = 0; i < S.Count; i++)
            {
                if (S[i].StartsWith(name))
                {
                    return S[i].Split(':')[1];
                }
            }
            return "";
        }
    }
}
