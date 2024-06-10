using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptedChatApp.MVVM.Model
{
    public class AsymmetricData
    {
        public string Plaintext { get; set; }
        public byte[] Ciphertext { get; set; }
        public AsymmetricData() 
        {
            Plaintext = "";
            Ciphertext = new byte[16];
        }
    }
}
