using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MCrypt
{
    static class Program
    {

        public static string fileName = "";
        public static bool doEncryptFile = false;
        public static bool doEncryptFolder = false;
        public static bool doDecrypt = false;

        private static bool isValidFiletype(string path)
        {
            if (Path.GetExtension(path) == ".encrypted" || Path.GetExtension(path) == ".aes" || Path.GetExtension(path) == ".secureaes" || Path.GetExtension(path) == ".mcrypt")
                return true;
            else
                return false;
        }

        [STAThread]
        static void Main(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (File.Exists(args[i]) && isValidFiletype(Path.GetExtension(args[i])) && !doEncryptFile && !doEncryptFolder)
                {
                    doDecrypt = true;
                    fileName = args[i];
                }
                else if (Directory.Exists(args[i]) && !doDecrypt && !doEncryptFile)
                {
                    doEncryptFolder = true;
                    fileName = args[i];
                }
                else if (File.Exists(args[i]) && !doDecrypt && !doEncryptFolder)
                {
                    doEncryptFile = true;
                    fileName = args[i];
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (doEncryptFile || doEncryptFolder) Application.Run(new MCrypt_Encrypt());
            else if (doDecrypt) Application.Run(new MCrypt_Decrypt());
            else Application.Run(new WarnPlaceholder()); //Placeholder
            //else Application.Run(new MCrypt());

        }
    }
}
