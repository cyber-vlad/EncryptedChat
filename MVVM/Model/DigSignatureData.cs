using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptedChatApp.MVVM.Model
{
    public class DigSignatureData
    {
        public string Plaintext;
        public byte[] Hash;
        public byte[] Signature;
        public DigSignatureData() 
        {
            Plaintext = "";
            Hash = new byte[16];
            Signature = new byte[16];
        }
    }
}
