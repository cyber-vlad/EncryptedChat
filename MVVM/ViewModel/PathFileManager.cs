using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EncryptedChatApp.MVVM.ViewModel
{
    public class PathFileManager
    {
        private int fileCounter;
        public PathFileManager() 
        { 
            fileCounter = 0;
        }
        public int GetFileCounter()
        {
            return fileCounter;
        }
        public void PlusOneFileCounter()
        {
            fileCounter++;
        }
        public bool VerifyPath(string path) 
        {
            if (!string.IsNullOrWhiteSpace(path) && !Path.GetInvalidPathChars().Any(c => path.Contains(c)))
            {
                string parentDirectory = Path.GetDirectoryName(path)!;
                if (!string.IsNullOrEmpty(parentDirectory) && Directory.Exists(parentDirectory) && Regex.IsMatch(path, @"^[A-Za-z]:\\"))
                {
                    if (File.Exists(path)) return true;
                }
            }
            return false;
        }
        public bool VerifyFileNamePath(string path, string fileName) 
        {
            if (!string.IsNullOrWhiteSpace(path) && !Path.GetInvalidPathChars().Any(c => path.Contains(c)))
            {
                string parentDirectory = Path.GetDirectoryName(path)!;
                if (!string.IsNullOrEmpty(parentDirectory) && Directory.Exists(parentDirectory) && Regex.IsMatch(path, @"^[A-Za-z]:\\"))
                {
                    string defaultFilePath = Path.Combine(parentDirectory, fileName);
                    if (File.Exists(defaultFilePath))
                        return true;
                }
            }
            return false;
        }
        public void CreateFile(string path,string name,string content) 
        {
            if (VerifyPath(path))
            {
                var fileContent = File.ReadAllText(path);
                string fileName = name + $"_({fileCounter})_" + ".txt";
                string filePathCreate = Path.Combine(Path.GetDirectoryName(path)!, fileName);
                File.WriteAllText(filePathCreate, content);
               fileCounter++;
            }
        }
    }
}
