using EncryptedChatApp.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncryptedChatApp.MVVM.ViewModel
{
    class AsymmetricViewModel
    {
        private AsymmetricData AsymmetricData;
        private RSACryptoServiceProvider rsa;
        RSAParameters _publicKey;
        RSAParameters _privateKey;

        public AsymmetricViewModel()
        {
            AsymmetricData = new AsymmetricData();
            rsa = new RSACryptoServiceProvider(2048);
            _publicKey = rsa.ExportParameters(false);
            _privateKey = rsa.ExportParameters(true);
        }

        public void SetPlaintext(string message)
        {
            AsymmetricData.Plaintext = message;
        }

        public string GetPlaintext()
        {
            return AsymmetricData.Plaintext;
        }

        public string GetCiphertext()
        {
            return Convert.ToBase64String(AsymmetricData.Ciphertext);
        }

        public void SetCiphertext(string str)
        {
            AsymmetricData.Plaintext = str;
            Encrypt();
        }

        public void Encrypt()
        {
            rsa.ImportParameters(_publicKey);
            var plaintextBytes = Encoding.Unicode.GetBytes(AsymmetricData.Plaintext);

            var encryptedBlocks = new List<byte[]>();
            for (int i = 0; i < plaintextBytes.Length; i += 120)
            {
                int blockSize = Math.Min(120, plaintextBytes.Length - i);
                byte[] blockToEncrypt = new byte[blockSize];
                Array.Copy(plaintextBytes, i, blockToEncrypt, 0, blockSize);

                byte[] encryptedBlock = rsa.Encrypt(blockToEncrypt, false);
                encryptedBlocks.Add(encryptedBlock);
            }
            AsymmetricData.Ciphertext = encryptedBlocks.SelectMany(b => b).ToArray();
        }

        public string Decrypt()
        {
            rsa.ImportParameters(_privateKey);
            var ciphertextBytes = AsymmetricData.Ciphertext;
            var decryptedBlocks = new List<byte[]>();
            
            for (int i = 0; i < ciphertextBytes.Length; i += 256)
            {
                int blockSize = Math.Min(256, ciphertextBytes.Length - i);
                byte[] blockToDecrypt = new byte[blockSize];
                Array.Copy(ciphertextBytes, i, blockToDecrypt, 0, blockSize);
                byte[] decryptedBlock = rsa.Decrypt(blockToDecrypt, false);
                decryptedBlocks.Add(decryptedBlock);
            }
            
            byte[] plaintextBytes = decryptedBlocks.SelectMany(b => b).ToArray();

            return Encoding.Unicode.GetString(plaintextBytes);
        }
    }
}
