using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using CoreChecksums;
using SecureAES;

namespace FAES
{
    public class FAES_File
    {
        protected enum Operation
        {
            NULL,
            ENCRYPT,
            DECRYPT
        };

        protected enum FAES_Type
        {
            NULL,
            FILE,
            FOLDER
        };

        protected string _filePath, _password, _fileName, _fullPath;
        protected Operation _op = Operation.NULL;
        protected FAES_Type _type = FAES_Type.NULL;
        protected string _sha1Hash;

        public FAES_File(string filePath)
        {
            if (File.Exists(filePath) || Directory.Exists(filePath))
            {
                _filePath = filePath;
                Initialise();
            }
            else throw new FileNotFoundException();
        }

        public FAES_File(string filePath, string password, ref bool success)
        {
            if (File.Exists(filePath) || Directory.Exists(filePath))
            {
                _filePath = filePath;
                _password = password;
                Initialise();
            }
            else throw new FileNotFoundException();

            Run(ref success);
        }

        private void Initialise()
        {
            isFolder();
            isFileDecryptable();
            getSHA1();
            getFileName();
            getPath();
        }

        public void Run(ref bool success)
        {
            if (!String.IsNullOrEmpty(_password))
            {
                if (isFileEncryptable())
                {
                    Console.WriteLine("Encrypting '{0}'...", _filePath);
                    FileAES_Encrypt encrypt = new FileAES_Encrypt(new FAES_File(_filePath), _password);
                    success = encrypt.encryptFile();
                }
                else if (isFileDecryptable())
                {
                    Console.WriteLine("Decrypting '{0}'...", _filePath);
                    FileAES_Decrypt decrypt = new FileAES_Decrypt(new FAES_File(_filePath), _password);
                    success = decrypt.decryptFile();
                }
                else
                {
                    throw new Exception("The file/folder specified is not valid!");
                }
            }
            else throw new Exception("A password has not been set!");
        }

        public void setPassword(string password)
        {
            _password = password;
        }

        public bool isFileEncryptable()
        {
            if (_op == Operation.NULL) isFileDecryptable();

            return (_op == Operation.ENCRYPT);
        }

        public bool isFileDecryptable()
        {
            if (_op == Operation.NULL)
            {
                if (Path.GetExtension(getPath()) == ".mcrypt")
                    Path.ChangeExtension(getPath(), ".faes");

                if (Path.GetExtension(getPath()) == ".faes")
                    _op = Operation.DECRYPT;
                else
                    _op = Operation.ENCRYPT;
            }

            return (_op == Operation.DECRYPT);
        }

        public string getSHA1()
        {
            if (isFile())
            {
                if (_sha1Hash == null) _sha1Hash = Checksums.convertHashToString(Checksums.getSHA1(getPath()));

                return _sha1Hash;
            }
            else return null;
        }

        public string getFileName()
        {
            if (_fileName == null) _fileName = Path.GetFileName(getPath());

            return _fileName;
        }

        public bool isFolder()
        {
            if (_type == FAES_Type.NULL)
            {
                FileAttributes attr = File.GetAttributes(getPath());

                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                    _type = FAES_Type.FOLDER;
                else
                    _type = FAES_Type.FILE;
            }

            return (_type == FAES_Type.FOLDER);
            
        }

        public bool isFile()
        {
            return !isFolder();
        }

        public string getPath()
        {
            if (_fullPath == null) _fullPath = Path.GetFullPath(_filePath);

            return _fullPath;
        }

        public override string ToString()
        {
            return getPath();
        }

        public string getFaesType()
        {
            if (_type == FAES_Type.FILE) return "File";
            else return "Folder";
        }

        public string getOperation()
        {
            if (_op == Operation.ENCRYPT) return "Encrypt";
            else return "Decrypt";
        }
    }

    public class FileAES_Encrypt
    {
        protected string tempPath = FileAES_Utilities.getDynamicTempFolder("Encrypt");
        protected FAES_File _file;
        protected string _password;
        protected SecureAES.SecureAES aes = new SecureAES.SecureAES();

        public FileAES_Encrypt(FAES_File file, string password)
        {
            if (file.isFileEncryptable())
            {
                _file = file;
                _password = password;
            }
            else throw new Exception("This filetype cannot be encrypted!");
        }

        public bool encryptFile()
        {
            string fileToDelete = Path.Combine(tempPath, _file.getFileName() + ".faeszip");
            string tempFolderName = "";
            bool success;
            try
            {
                try
                {
                    if (_file.isFile())
                    {
                        if (!Directory.Exists(Path.Combine(tempPath))) Directory.CreateDirectory(Path.Combine(tempPath));

                        using (ZipArchive zip = ZipFile.Open(Path.Combine(tempPath, _file.getFileName()) + ".faeszip", ZipArchiveMode.Create))
                        {
                            zip.CreateEntryFromFile(_file.getPath(), _file.getFileName());
                            zip.Dispose();
                        }
                    }
                    else
                    {
                        tempFolderName = FileAES_Utilities.genRandomTempFolder(_file.getFileName().Substring(0, _file.getFileName().Length - Path.GetExtension(_file.getFileName()).Length));
                        if (Directory.Exists(Path.Combine(tempPath, tempFolderName))) Directory.Delete(Path.Combine(tempPath, tempFolderName), true);
                        FileAES_Utilities.DirectoryCopy(_file.getPath(), Path.Combine(tempPath, tempFolderName, _file.getFileName()), true);
                        ZipFile.CreateFromDirectory(Path.Combine(tempPath, tempFolderName), Path.Combine(tempPath, _file.getFileName()) + ".faeszip");
                    }
                }
                catch
                {
                    throw new IOException("Error occured in creating the FAESZIP file.");
                }

                try
                {
                    success = aes.AES_Encrypt(Path.Combine(tempPath, _file.getFileName()) + ".faeszip", _password);
                }
                catch
                {
                    throw new IOException("Error occured in encrypting the FAESZIP file.");
                }
                
                try
                {
                    if (File.Exists(fileToDelete))
                        File.Delete(fileToDelete);
                }
                catch
                {
                    throw new IOException("Error occured in deleting the FAESZIP file.");
                }

                try
                {
                    if (_file.isFile()) File.Move(Path.Combine(tempPath, _file.getFileName().Substring(0, _file.getFileName().Length - Path.GetExtension(_file.getFileName()).Length) + ".faes"), Path.Combine(Directory.GetParent(_file.getPath()).FullName, _file.getFileName().Substring(0, _file.getFileName().Length - Path.GetExtension(_file.getFileName()).Length) + ".faes"));
                    if (Directory.Exists(Path.Combine(tempPath, tempFolderName))) Directory.Delete(Path.Combine(tempPath, tempFolderName), true);
                    if (_file.isFolder() && Directory.Exists(Path.Combine(tempPath, tempFolderName))) Directory.Delete(Path.Combine(tempPath, tempFolderName), true);
                    if (_file.isFile()) File.Delete(_file.getPath());
                    else Directory.Delete(_file.getPath(), true);
                    if (_file.isFolder()) File.Move(Path.Combine(tempPath, _file.getFileName().Substring(0, _file.getFileName().Length - Path.GetExtension(_file.getFileName()).Length) + ".faes"), Path.Combine(Directory.GetParent(_file.getPath()).FullName, _file.getFileName().Substring(0, _file.getFileName().Length - Path.GetExtension(_file.getFileName()).Length) + ".faes"));
                    File.SetAttributes(_file.getFileName().Substring(0, _file.getFileName().Length - Path.GetExtension(_file.getFileName()).Length) + ".faes", FileAttributes.Encrypted);
                }
                catch
                {
                    throw new IOException("Error occured in moving the FAES file after encryption.");
                }

            }
            finally
            {
                if (File.Exists(fileToDelete))
                    File.Delete(fileToDelete);

                if (Directory.Exists(tempPath)) Directory.Delete(tempPath, true);
            }
            return success;
        }
    }

    public class FileAES_Decrypt
    {
        protected string tempPath = FileAES_Utilities.getDynamicTempFolder("Decrypt");
        protected FAES_File _file;
        protected string _password;
        protected SecureAES.SecureAES aes = new SecureAES.SecureAES();

        public FileAES_Decrypt(FAES_File file, string password)
        {
            if (file.isFileDecryptable())
            {
                _file = file;
                _password = password;
            }
            else throw new Exception("This filetype cannot be decrypted!");
        }

        public bool decryptFile()
        {
            bool success = false;

            success = aes.AES_Decrypt(_file.getPath(), _password);

            File.SetAttributes(Path.Combine(Directory.GetParent(_file.getPath()).FullName, _file.getFileName().Substring(0, _file.getFileName().Length - Path.GetExtension(_file.getFileName()).Length) + ".faeszip"), FileAttributes.Hidden);

            if (success)
            {
                try
                {
                    ZipFile.ExtractToDirectory(Path.Combine(Directory.GetParent(_file.getPath()).FullName, _file.getFileName().Substring(0, _file.getFileName().Length - Path.GetExtension(_file.getFileName()).Length) + ".faeszip"), Directory.GetParent(_file.getPath()).FullName);
                }
                catch
                {
                    throw new IOException("Error occured in extracting the FAESZIP file.");
                }

                try
                {
                    File.SetAttributes(Directory.GetParent(_file.getPath()).FullName, FileAttributes.Hidden);
                    File.Delete(Path.Combine(Directory.GetParent(_file.getPath()).FullName, _file.getFileName().Substring(0, _file.getFileName().Length - Path.GetExtension(_file.getFileName()).Length) + ".faeszip"));
                    File.Delete(_file.getPath());
                    File.SetAttributes(Directory.GetParent(_file.getPath()).FullName, FileAttributes.Normal);
                }
                catch
                {
                    throw new IOException("Error occured in deleting the FAESZIP file.");
                }
            }

            if (Directory.Exists(tempPath)) Directory.Delete(tempPath, true);
            if (File.Exists(Path.ChangeExtension(_file.getPath(), "faeszip"))) File.Delete(Path.ChangeExtension(_file.getPath(), "faeszip"));

            return success;
        }
    }

    public class FileAES_Utilities
    {
        public static string getDynamicTempFolder(string label)
        {
            return Path.Combine(Path.GetTempPath(), "FileAES", genRandomTempFolder(label));
        }

        public static string genRandomTempFolder(string sourceFolder)
        {
            DateTime now = DateTime.Now;
            return sourceFolder + "_" + (now.Day).ToString("00") + (now.Month).ToString("00") + (now.Year % 100).ToString() + (now.Hour).ToString("00") + (now.Minute).ToString("00") + (now.Second).ToString("00");
        }

        public static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFiles(path).Any();
        }

        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists) return;

            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!Directory.Exists(destDirName)) Directory.CreateDirectory(destDirName);

            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files) file.CopyTo(Path.Combine(destDirName, file.Name), false);
            if (copySubDirs) foreach (DirectoryInfo subdir in dirs) DirectoryCopy(subdir.FullName, Path.Combine(destDirName, subdir.Name), copySubDirs);
        }

        public static string FAES_ExceptionHandling(Exception e)
        {
            if (e.ToString().Contains("Error occured in creating the FAESZIP file."))
                return "ERROR: The chosen file(s) could not be compressed as a compressed version already exists in the Temp files! Consider using '--purgeTemp' if you are not using another instance of FileAES and this error persists.";
            else if (e.ToString().Contains("Error occured in encrypting the FAESZIP file."))
                return "ERROR: The compressed file could not be encrypted. Please close any other instances of FileAES and try again. Consider using '--purgeTemp' if you are not using another instance of FileAES and this error persists.";
            else if (e.ToString().Contains("Error occured in deleting the FAESZIP file."))
                return "ERROR: The compressed file could not be deleted. Please close any other instances of FileAES and try again. Consider using '--purgeTemp' if you are not using another instance of FileAES and this error persists.";
            else if (e.ToString().Contains("Error occured in moving the FAES file after encryption."))
                return  "ERROR: The encrypted file could not be moved to the original destination! Please ensure that a file with the same name does not already exist.";
            else if (e.ToString().Contains("Error occured in decrypting the FAES file."))
                return  "ERROR: The encrypted file could not be decrypted. Please try again.";
            else if (e.ToString().Contains("Error occured in extracting the FAESZIP file."))
                return "ERROR: The compressed file could not be extracted! Consider using '--purgeTemp' if you are not using another instance of FileAES and this error persists.";
            else
                return e.ToString();
        }
    }
}
