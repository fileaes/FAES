using FAES;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace FileAES
{
    static class Program
    {

        public static string fileName = "";
        public static string tempPathInstance = "";
        public static bool doEncryptFile = false;
        public static bool doEncryptFolder = false;
        public static bool doDecrypt = false;
        private static bool _skipUpdate = false;
        private static bool _fullInstall = false;
        private static bool _cleanUpdates = false;
        private static bool _purgeTemp = false;
        private static string branch = "";
        private static string _launchTimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffffff");
        private static string _autoPassword = null;

        [STAThread]
        static void Main(string[] args)
        {
            tempPathInstance = Path.Combine(Path.GetTempPath(), "FileAES");
            List<string> arguments = new List<string>();
            arguments.AddRange(args);
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"mullak99\FileAES\config\launchParams.cfg"))) arguments.AddRange(readLaunchParams());
            if (File.Exists(@"Config\launchParams.cfg")) arguments.AddRange(readLaunchParams(true));

            string[] param = arguments.ToArray();

            for (int i = 0; i < param.Length; i++)
            {
                param[i].ToLower();

                if (File.Exists(param[i]) && Core.isValidFiletype(Path.GetExtension(param[i])) && !doEncryptFile && !doEncryptFolder)
                {
                    doDecrypt = true;
                    fileName = param[i];
                }
                else if (Directory.Exists(param[i]) && !doDecrypt && !doEncryptFile)
                {
                    doEncryptFolder = true;
                    fileName = param[i];
                }
                else if (File.Exists(param[i]) && !doDecrypt && !doEncryptFolder)
                {
                    doEncryptFile = true;
                    fileName = param[i];
                }

                if (param[i].Equals("-fullinstall") || param[i].Equals("--fullinstall") || param[i].Equals("-f") || param[i].Equals("--f")) _fullInstall = true;
                else if (param[i] == "--dev") branch = "dev";
                else if (param[i] == "--stable") branch = "stable";
                else if (param[i] == "--skipupdate" || param[i] == "-skipupdate") _skipUpdate = true;
                else if (param[i].Equals("-cleanupdates") || param[i].Equals("--cleanupdates") || param[i].Equals("-c") || param[i].Equals("--c")) _cleanUpdates = true;
                else if (param[i].Equals("-update") || param[i].Equals("--update") || param[i].Equals("-u") || param[i].Equals("--u")) FileAES_Update.selfUpdate(_cleanUpdates);
                else if (param[i].Equals("-password") || param[i].Equals("--password") || param[i].Equals("-p") || param[i].Equals("--p") && !String.IsNullOrEmpty(param[i + 1])) _autoPassword = param[i + 1];
                else if (param[i].Equals("-purgetemp") || param[i].Equals("--purgetemp") || param[i].Equals("-deletetemp") || param[i].Equals("--deletetemp")) _purgeTemp = true;
            }
            if (String.IsNullOrEmpty(branch)) branch = "stable";

            if (_purgeTemp) FileAES_Utilities.PurgeTempFolder();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (doEncryptFile || doEncryptFolder) Application.Run(new FileAES_Encrypt(fileName, _autoPassword));
            else if (doDecrypt) Application.Run(new FileAES_Decrypt(fileName, _autoPassword));
            else Application.Run(new FileAES_Main());

        }

        public static string[] readLaunchParams(bool local = false)
        {
            string dir;
            if (local)
                dir = Path.Combine(Directory.GetCurrentDirectory(), @"config\launchParams.cfg");
            else
                dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"mullak99\FileAES\config\launchParams.cfg");

            return File.ReadAllLines(dir);
        }

        public static string getBranch()
        {
            return branch;
        }

        public static bool getCleanUpdates()
        {
            return _cleanUpdates;
        }

        public static bool getFullInstall()
        {
            return _fullInstall;
        }

        public static bool getSkipUpdate()
        {
            return _skipUpdate;
        }
    }
}
