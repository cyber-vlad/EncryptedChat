using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EncryptedChatApp.MVVM.Model;
using EncryptedChatApp.MVVM.View;
using System.Windows;
using System.Windows.Controls;
using System.Security.Cryptography;
using System.Windows.Input;
using System.IO;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Collections;

namespace EncryptedChatApp.MVVM.ViewModel
{
    class SymmetricViewModel
    {
        private SymmetricData SymmetricData;
        public SymmetricViewModel()
        {
            SymmetricData = new SymmetricData();
            
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(SymmetricData.Key);
                rng.GetBytes(SymmetricData.InitializationVector);
            }
        }
        public void SetPlaintext(string message)
        {
            SymmetricData.Plaintext = message;
        }
        public string GetPlaintext()
        {
            return SymmetricData.Plaintext;
        }
        public string GetCiphertext()
        {
            return Convert.ToBase64String(SymmetricData.Ciphertext);
        }
        public void SetCiphertext(string str)
        {
            SymmetricData.Plaintext = str;
            Encrypt();

        }
        public void Encrypt()
        {
            using (Aes aes = Aes.Create())
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(SymmetricData.Key, SymmetricData.InitializationVector);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(SymmetricData.Plaintext);
                        }

                        SymmetricData.Ciphertext = memoryStream.ToArray();
                    }
                }
            }
        }
        public string Decrypt() 
        {
            string simpletext = String.Empty;
            using (Aes aes = Aes.Create())
            {
                ICryptoTransform decryptor = aes.CreateDecryptor(SymmetricData.Key, SymmetricData.InitializationVector);
                using (MemoryStream memoryStream = new MemoryStream(SymmetricData.Ciphertext))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            simpletext = streamReader.ReadToEnd();
                        }
                    }
                }
            }
            return simpletext;
        }
    }
}
