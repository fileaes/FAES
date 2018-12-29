using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleSettingsManager;

namespace FAES_GUI
{
    public class SettingsManager
    {

        protected SSM ssm;

        public SettingsManager()
        {
            //ssm = new SSM()
        }
    }

    public class ProgramManager
    {
        protected string _appDataPath;

        protected string[] _currentSubDirsAppData = new string[] { "config" };

        public ProgramManager()
        {
            cacheVariables();
        }

        private void cacheVariables()
        {
            
        }

        private void createAppDataDirectory(bool useFullInstall)
        {
            if (useFullInstall)
            {
                foreach (string subDir in _currentSubDirsAppData)
                {
                    if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "mullak99", "FileAES", subDir)))
                        Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "mullak99", "FileAES", subDir));
                }
            }
            
            else
                _appDataPath = Path.Combine("");
        }

        public string getAppDataPath()
        {
            return "/";
        }
    }
}
