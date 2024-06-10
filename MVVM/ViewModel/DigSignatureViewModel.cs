using EncryptedChatApp.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace EncryptedChatApp.MVVM.ViewModel
{
    class DigSignatureViewModel
    {
        private DigSignatureData digSignatureData;
        RSAParameters _publicKey;
        RSAParameters _privateKey;

        public DigSignatureViewModel()
        {
            digSignatureData = new DigSignatureData();
            using (RSA rsa = RSA.Create())
            {
                _publicKey = rsa.ExportParameters(false);
                _privateKey = rsa.ExportParameters(true);
            }
        }
        public void SetPlaintext(string message)
        {
            digSignatureData.Plaintext = message;
        }

        public string GetPlaintext()
        {
            return digSignatureData.Plaintext;
        }
        public string GetDigitalSignature()
        {
            return Convert.ToBase64String(digSignatureData.Signature);
        }
        public void ComputeHash(string text)
        {
            byte[] data = Encoding.ASCII.GetBytes(text);
            using (SHA256 alg = SHA256.Create())
            {
                digSignatureData.Hash = alg.ComputeHash(data);
            }
        }
        public void GenerateDigitalSignature()
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportParameters(_privateKey);

                RSAPKCS1SignatureFormatter rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
                rsaFormatter.SetHashAlgorithm("SHA256");

                digSignatureData.Signature = rsaFormatter.CreateSignature(digSignatureData.Hash);

            }
        }
        
        public bool VerifyDigitalSignature()
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportParameters(_publicKey);

                RSAPKCS1SignatureDeformatter rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
                rsaDeformatter.SetHashAlgorithm("SHA256");

                return rsaDeformatter.VerifySignature(digSignatureData.Hash, digSignatureData.Signature);
            }
        }

    }
}
