using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptedChatApp.MVVM.Model
{
    public class SymmetricData
    {
        public string Plaintext { get; set; } 
        public byte[] Ciphertext { get; set;}
        public byte[] Key {  get; set; } 
        public byte[] InitializationVector { get; set; } 
        // another level of security, that prevents the same plaintext is being encrypted to the same ciphertext

        public SymmetricData() 
        {
            Plaintext = "";
            Ciphertext = new byte[16];
            Key = new byte[16];
            InitializationVector = new byte[16];
        }

    }
}
