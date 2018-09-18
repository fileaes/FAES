using FAES;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FAES_GUI
{
    static class Program
    {

        private static bool _verbose = false;
        private static bool _debugMenu = false;
        private static bool _purgeTemp = false;
        private static bool _headless = false;
        private static string _directory = null;
        private static string _password;
        private static List<string> _strippedArgs = new List<string>();

        public static FAES_File fileAES;

        [STAThread]
        static void Main(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                args[i].ToLower();

                string strippedArg = args[i];

                if (Directory.Exists(args[i])) _directory = args[i];
                else if (File.Exists(args[i])) _directory = args[i];

                if (args[i][0] == '-') strippedArg = args[i].Replace("-", string.Empty);
                else if (args[i][0] == '/') strippedArg = args[i].Replace("/", string.Empty);
                else if (args[i][0] == '\\') strippedArg = args[i].Replace("\\", string.Empty);

                if (strippedArg == "verbose" || strippedArg == "v") _verbose = true;
                else if (strippedArg == "password" || strippedArg == "p" && !string.IsNullOrEmpty(args[i + 1])) _password = args[i + 1];
                else if (strippedArg == "purgetemp" || strippedArg == "deletetemp") _purgeTemp = true;
                else if (strippedArg == "debug" || strippedArg == "developer") _debugMenu = true;
                else if (strippedArg == "headless" || strippedArg == "h") _headless = true;

                _strippedArgs.Add(strippedArg);
            }

            if (!String.IsNullOrEmpty(_directory) && !String.IsNullOrEmpty(_password) && _headless)
            {
                bool successful = false;

                try
                {
                    fileAES = new FAES_File(_directory, _password, ref successful);

                    if (successful) Console.WriteLine("{0}ion on {1} succeded!", fileAES.getOperation(), fileAES.getFaesType().ToLower());
                    else Console.WriteLine("{0}ion on {1} failed!", fileAES.getOperation(), fileAES.getFaesType().ToLower());

                    if (!successful && fileAES.isFileDecryptable()) Console.WriteLine("Ensure that you entered the correct password!");

                }
                catch (Exception e)
                {
                    Console.WriteLine(FileAES_Utilities.FAES_ExceptionHandling(e));
                }
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                if (!String.IsNullOrEmpty(_directory))
                {
                    fileAES = new FAES_File(_directory);

                    if (fileAES.getOperation() == "Encrypt")
                        Application.Run(new EncryptForm());
                    else
                        Application.Run(new DecryptForm());
                }
                else  
                    Application.Run(new MainForm());
            }
        }

        public static bool setPath(string path)
        {
            if (Directory.Exists(path) || File.Exists(path))
            {
                _directory = path;
                return true;
            }
            return false;
        }

        public static string getPath()
        {
            return _directory;
        }

        public static string getPassword()
        {
            return _password;
        }
    }
}
