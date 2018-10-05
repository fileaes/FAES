using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FAES;

namespace FileAES_CLI
{
    class Program
    {
        private static bool _verbose = false;
        private static bool _purgeTemp = false;
        private static bool _help = false;
        private static bool _getHint = false;
        private static bool _getEncryptTimestamp = false;
        private static string _directory = null;
        private static string _passwordHint = null;
        private static string _password;
        private static List<string> _strippedArgs = new List<string>();

        static void Main(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                string strippedArg = args[i].ToLower();

                if (Directory.Exists(args[i])) _directory = args[i];
                else if (File.Exists(args[i])) _directory = args[i];

                if (strippedArg[0] == '-') strippedArg = strippedArg.Replace("-", string.Empty);
                else if (strippedArg[0] == '/') strippedArg = strippedArg.Replace("/", string.Empty);
                else if (strippedArg[0] == '\\') strippedArg = strippedArg.Replace("\\", string.Empty);

                if (strippedArg == "verbose" || strippedArg == "v") _verbose = true;
                else if (String.IsNullOrEmpty(_password) && (strippedArg == "password" || strippedArg == "p") && !string.IsNullOrEmpty(args[i + 1])) _password = args[i + 1];
                else if (strippedArg == "purgetemp" || strippedArg == "deletetemp") _purgeTemp = true;
                else if (strippedArg == "help") _help = true;
                else if (String.IsNullOrEmpty(_passwordHint) && (strippedArg == "hint" || strippedArg == "passwordhint" || strippedArg == "h") && !string.IsNullOrEmpty(args[i + 1])) _passwordHint = args[i + 1];
                else if (strippedArg == "gethint" || strippedArg == "getpasswordhint") _getHint = true;
                else if (strippedArg == "gettimestamp" || strippedArg == "timestamp" || strippedArg == "encryptiondate") _getEncryptTimestamp = true;

                _strippedArgs.Add(strippedArg);
            }

            if (_help)
            {
                Console.WriteLine("A FAES-based tool for encrypting and decrypting files using the command-line.\n\nPossible Launch Parameters:\n'--verbose' or '-v': Show more debugging information in the console (WIP)." +
                    "\n'--purgeTemp' or '-p': Purge the FileAES Temp folder to resolve possible errors.\n'--password <password>' or '-p <password>': Set the password that will be used to encrypt/decrypt the file/folder." +
                    "\n'--hint <Password Hint>' or '-h <Password Hint>': Sets a password hint.\n'--getHint': Gets the password hint for the encrypted file.\n'--getTimestamp': Gets the encryption timestamp of the encrypted file.\n\n" +
                    "File/Folder names can be entered as a launch parameter to select what to encrypt/decrypt (also allows for dragging/dropping a file/folder on the .exe).\n\n" +
                    "Example: 'FileAES-CLI.exe File.txt -p password123'");
                return;
            }

            if (_purgeTemp)
            {
                FileAES_Utilities.PurgeTempFolder();
            }

            if (_getHint)
            {
                if (File.Exists(_directory) && FileAES_Utilities.isFileDecryptable(_directory))
                {
                    string passHint = FileAES_Utilities.GetPasswordHint(_directory);

                    if (passHint != "No Password Hint Set")
                        Console.WriteLine("The hint for '{0}' is: {1}", Path.GetFileName(_directory), passHint);
                    else
                        Console.WriteLine("'{0}' does not contain a password hint!", Path.GetFileName(_directory));

                    if (String.IsNullOrEmpty(_password) && !_getEncryptTimestamp) return;
                }
                else
                {
                    Console.WriteLine("You have not specified a valid encrypted file!");
                    return;
                }
            }

            if(_getEncryptTimestamp)
            {
                if (File.Exists(_directory) && FileAES_Utilities.isFileDecryptable(_directory))
                {
                    int timestamp = FileAES_Utilities.GetEncrpytionTimeStamp(_directory);

                    if (timestamp >= 0)
                    {
                        DateTime dateTime = FileAES_Utilities.UnixTimeStampToDateTime((double)timestamp);
                        Console.WriteLine("'{0}' was encrypted on {1} at {2}.", Path.GetFileName(_directory), dateTime.ToString("dd/MM/yyyy"), dateTime.ToString("HH:mm:ss tt"));
                    }
                    else
                    {
                        Console.WriteLine("This file does not contain a encryption date. This is likely due to this file being encrypted using an older FAES version.");
                    }

                    if (String.IsNullOrEmpty(_password)) return;
                }
                else
                {
                    Console.WriteLine("You have not specified a valid encrypted file!");
                    return;
                }
            }

            if (String.IsNullOrEmpty(_directory))
            {
                while (true)
                {
                    Console.Write("File/Folder: ");
                    _directory = Console.ReadLine();

                    if (File.Exists(_directory) || Directory.Exists(_directory))
                    {
                        break;
                    }
                    Console.WriteLine("You have not specified a valid file or folder!");
                }
            }
            if (String.IsNullOrEmpty(_password))
            {
                while (true)
                {
                    Console.Write("Password: ");
                    string password = passwordInput();

                    if (new FAES_File(_directory).isFileEncryptable())
                    {
                        Console.Write("\nConf. Password: ");
                        string passwordConf = passwordInput();

                        Console.Write(Environment.NewLine);

                        if (password == passwordConf)
                        {
                            _password = password;
                            break;
                        }
                        Console.WriteLine("Passwords do not match!");
                    }
                    else
                    {
                        Console.Write(Environment.NewLine);
                        _password = password;
                        break;
                    }
                }
            }

            if (!File.Exists(_directory) && !Directory.Exists(_directory))
            {
                Console.WriteLine("You have not specified a valid file or folder!");
            }
            else if (String.IsNullOrEmpty(_password))
            {
                Console.WriteLine("Please specify a password!");
            }
            else
            {
                bool successful = false;
                FAES_File fileAES;

                try
                {
                    fileAES = new FAES_File(_directory, _password, ref successful, _passwordHint);

                    if (successful) Console.WriteLine("{0}ion on {1} succeded!", fileAES.getOperation(), fileAES.getFaesType().ToLower());
                    else Console.WriteLine("{0}ion on {1} failed!", fileAES.getOperation(), fileAES.getFaesType().ToLower());

                    if (!successful && fileAES.isFileDecryptable())
                    {
                        Console.WriteLine("Ensure that you entered the correct password!");
                        Console.WriteLine("Password Hint: {0}", fileAES.getPasswordHint());
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
        }

        public static string passwordInput()
        {
            ConsoleKeyInfo inf;
            StringBuilder input = new StringBuilder();
            inf = Console.ReadKey(true);
            while (inf.Key != ConsoleKey.Enter)
            {
                if (inf.Key == ConsoleKey.Backspace && input.Length > 0) input.Remove(input.Length - 1, 1);
                else input.Append(inf.KeyChar);
                inf = Console.ReadKey(true);
            }

            return input.ToString();
        }
    }
}
