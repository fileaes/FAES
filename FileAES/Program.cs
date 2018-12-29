using FAES;
using FAES.Packaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
        private static bool _getFaesVersion = false;
        private static bool _getVersion = false;
        private static string _directory = null;
        private static string _password;
        private static string _passwordHint = null;
        private static string _compressionMethod = null;
        private static int _compressionLevel = 7;
        private static List<string> _strippedArgs = new List<string>();

        public static FAES_File faesFile;

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
                else if (String.IsNullOrEmpty(_passwordHint) && (strippedArg == "hint" || strippedArg == "passwordhint" || strippedArg == "h") && !string.IsNullOrEmpty(args[i + 1])) _passwordHint = args[i + 1];
                else if (strippedArg == "purgetemp" || strippedArg == "deletetemp") _purgeTemp = true;
                else if (strippedArg == "debug" || strippedArg == "debugmode" || strippedArg == "developer" || strippedArg == "devmode" || strippedArg == "dev") _debugMenu = true;
                else if (strippedArg == "headless" || strippedArg == "cli" || strippedArg == "commandline") _headless = true;
                else if (strippedArg == "faesversion" || strippedArg == "faes" || strippedArg == "faesver") _getFaesVersion = true;
                else if (strippedArg == "faesguiversion" || strippedArg == "faesguiver" || strippedArg == "faesgui" || strippedArg == "guiver" || strippedArg == "ver")
                {
                    _getVersion = true;
                    _getFaesVersion = true;
                }

                else if (String.IsNullOrEmpty(_compressionMethod) && (strippedArg == "compression" || strippedArg == "compressionmethod" || strippedArg == "c") && !string.IsNullOrEmpty(args[i + 1])) _compressionMethod = args[i + 1].ToUpper();
                else if ((strippedArg == "level" || strippedArg == "compressionlevel" || strippedArg == "l") && !string.IsNullOrEmpty(args[i + 1]))
                {
                    try
                    {
                        _compressionLevel = Convert.ToInt32(args[i + 1]);
                    }
                    catch
                    { }
                }

                _strippedArgs.Add(strippedArg);
            }

            if (_purgeTemp)
            {
                FileAES_Utilities.PurgeTempFolder();
            }

            if (_getVersion)
            {
                Console.WriteLine("Current FileAES Version: {0}", GetVersion());
            }

            if (_getFaesVersion)
            {
                Console.WriteLine("Current FAES Version: {0}", FileAES_Utilities.GetVersion());
            }

            if (!String.IsNullOrEmpty(_directory) && !String.IsNullOrEmpty(_password) && _headless)
            {
                faesFile = new FAES_File(_directory);

                Console.WriteLine("FileAES-GUI (FileAES) only has basic command-line support. Use FileAES-CLI for improved command-line functionality.");

                try
                {
                    if (_compressionLevel < 0 || _compressionLevel > 9)
                    {
                        Console.WriteLine("You have not specified a valid compression level! Please choose a value between 0 and 9.");
                        return;
                    }
                    else
                    {
                        if (faesFile.isFileEncryptable())
                        {
                            FileAES_Encrypt encrypt = new FileAES_Encrypt(faesFile, _password, _passwordHint);

                            if (!String.IsNullOrEmpty(_compressionMethod))
                            {
                                switch (_compressionMethod)
                                {
                                    case "ZIP":
                                        {
                                            encrypt.SetCompressionMode(CompressionMode.ZIP, _compressionLevel);
                                            break;
                                        }
                                    case "TAR":
                                        {
                                            encrypt.SetCompressionMode(CompressionMode.TAR, _compressionLevel);
                                            break;
                                        }
                                    case "LZMA":
                                        {
                                            encrypt.SetCompressionMode(CompressionMode.LZMA, _compressionLevel);
                                            break;
                                        }
                                    case "LGYZIP":
                                    case "LEGACYZIP":
                                    case "LEGACY":
                                        {
                                            encrypt.SetCompressionMode(CompressionMode.LGYZIP, _compressionLevel);
                                            break;
                                        }
                                    default:
                                        Console.WriteLine("Unknown Compression Method: {0}", _compressionMethod);
                                        return;
                                }
                            }

                            if (encrypt.encryptFile())
                            {
                                Console.WriteLine("Encryption on {0} succeeded!", faesFile.getFaesType().ToLower());
                            }
                            else
                            {
                                Console.WriteLine("Encryption on {0} failed!", faesFile.getFaesType().ToLower());
                            }
                        }
                        else
                        {
                            FileAES_Decrypt decrypt = new FileAES_Decrypt(faesFile, _password);

                            if (decrypt.decryptFile())
                            {
                                Console.WriteLine("Decryption on {0} succeeded!", faesFile.getFaesType().ToLower());
                            }
                            else
                            {
                                Console.WriteLine("Decryption on {0} failed!", faesFile.getFaesType().ToLower());
                                Console.WriteLine("Ensure that you entered the correct password!");
                                Console.WriteLine("Password Hint: {0}", faesFile.getPasswordHint());
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    if (!_verbose)
                        Console.WriteLine(FileAES_Utilities.FAES_ExceptionHandling(e));
                    else
                    {
                        Console.WriteLine("Verbose Mode: Showing Full Exception...");
                        Console.WriteLine(e.ToString());
                    }
                }
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                if (!String.IsNullOrEmpty(_directory))
                {
                    faesFile = new FAES_File(_directory);

                    if (faesFile.isFileEncryptable())
                        Application.Run(new EncryptForm(faesFile));
                    else if(faesFile.isFileDecryptable())
                        Application.Run(new DecryptForm(faesFile));
                }
                else  
                    Application.Run(new MainForm());
            }
        }

        public static bool SetPath(string path)
        {
            if (Directory.Exists(path) || File.Exists(path))
            {
                _directory = path;
                return true;
            }
            return false;
        }

        public static string GetPath()
        {
            return _directory;
        }

        public static string GetPassword()
        {
            return _password;
        }

        public static string GetVersion()
        {
            string[] ver = (typeof(FAES_GUI.Program).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version).Split('.');
            return "v" + ver[0] + "." + ver[1] + "." + ver[2];
        }

        public static bool GetDebugMode()
        {
            return _debugMenu;
        }
    }
}
