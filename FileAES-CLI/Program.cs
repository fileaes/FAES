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
        private static string _directory = null;
        private static string _password;
        private static List<string> _strippedArgs = new List<string>();

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
                else if (strippedArg == "help" || strippedArg == "h") _help = true;

                _strippedArgs.Add(strippedArg);
            }

            if (_help)
            {
                Console.WriteLine("A FAES-based tool for encrypting and decrypting files using the command-line.\n\nPossible Launch Parameters:\n'--verbose' or '-v': Show more debugging information in the console (WIP)." +
                    "\n'--purgeTemp' or '-p': Purge the FileAES Temp folder to resolve possible errors.\n'--password <password>' or '-p <password>': Set the password that will be used to encrypt/decrypt the file/folder.\n\n" +
                    "File/Folder names can be entered as a launch parameter to select what to encrypt/decrypt (also allows for dragging/dropping a file/folder on the .exe).\n\n" +
                    "Example: 'FileAES-CLI.exe File.txt -p password123'");

                return;
            }

            if (_purgeTemp)
            {
                if (Directory.Exists(Path.Combine(Path.GetTempPath(), "FileAES"))) Directory.Delete(Path.Combine(Path.GetTempPath(), "FileAES"), true);
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
                    fileAES = new FAES_File(_directory, _password, ref successful);

                    if (successful) Console.WriteLine("{0}ion on {1} succeded!", fileAES.getOperation(), fileAES.getFaesType().ToLower());
                    else Console.WriteLine("{0}ion on {1} failed!", fileAES.getOperation(), fileAES.getFaesType().ToLower());

                    if (!successful && fileAES.isFileDecryptable()) Console.WriteLine("Ensure that you entered the correct password!");

                }
                catch (IOException e)
                {
                    if (e.ToString().Contains("Error occured in creating the FAESZIP file."))
                        Console.WriteLine("ERROR: The chosen file(s) could not be compressed as a compressed version already exists in the Temp files! Consider using '--purgeTemp' if you are not using another instance of FileAES and this error persists.");
                    else if (e.ToString().Contains("Error occured in encrypting the FAESZIP file."))
                        Console.WriteLine("ERROR: The compressed file could not be encrypted. Please close any other instances of FileAES and try again. Consider using '--purgeTemp' if you are not using another instance of FileAES and this error persists.");
                    else if (e.ToString().Contains("Error occured in deleting the FAESZIP file."))
                        Console.WriteLine("ERROR: The compressed file could not be deleted. Please close any other instances of FileAES and try again. Consider using '--purgeTemp' if you are not using another instance of FileAES and this error persists.");
                    else if (e.ToString().Contains("Error occured in moving the FAES file after encryption."))
                        Console.WriteLine("ERROR: The encrypted file could not be moved to the original destination! Please ensure that a file with the same name does not already exist.");
                    else if (e.ToString().Contains("Error occured in decrypting the FAES file."))
                        Console.WriteLine("ERROR: The encrypted file could not be decrypted. Please try again.");
                    else if (e.ToString().Contains("Error occured in extracting the FAESZIP file."))
                        Console.WriteLine("ERROR: The compressed file could not be extracted! Consider using '--purgeTemp' if you are not using another instance of FileAES and this error persists.");
                    else
                        Console.WriteLine(e.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
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
