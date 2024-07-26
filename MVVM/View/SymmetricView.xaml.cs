using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EncryptedChatApp.MVVM.ViewModel;
using System.IO;
namespace EncryptedChatApp.MVVM.View
{
    public partial class SymmetricView : UserControl
    {
        public string? plaintext { get; set; }
        public static SymmetricView? SymmetricInstance { get; private set; }
        private SymmetricViewModel symmetricViewModel;
        private PathFileManager pathFile;

        public SymmetricView()
        {
            InitializeComponent();
            SymmetricInstance = this;
            symmetricViewModel = new SymmetricViewModel();
            pathFile = new PathFileManager();
        }
        public void SetSymmetricPlainTextValue(string message)
        {
            plaintext = message;
            SymmetricPlainText.Text = plaintext;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            SymmetricPlainText.Text = "";
            SymmetricEncryptedText.Text = "";
            SymmetricDecryptedText.Text = "";
            TextBoxPath.Text = "";
            InfoPath.Text = "Searching file for encryption/decryption";

        }
        public void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (SymmetricPlainText.Text != "")
            {
                plaintext = SymmetricPlainText.Text;
                symmetricViewModel.SetPlaintext(plaintext);
                symmetricViewModel.Encrypt();
                SymmetricEncryptedText.Text = symmetricViewModel.GetCiphertext().ToString();
                
            }
        }
        public void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (SymmetricEncryptedText.Text != "")
            {
                symmetricViewModel.SetCiphertext(SymmetricPlainText.Text);
                string simpletext = symmetricViewModel.Decrypt();
                SymmetricDecryptedText.Text = simpletext;
            }
        }

        private void VerifyButtonPath_Click(object sender, RoutedEventArgs e)
        {
            InfoPath.Text = "Searching file for encryption/decryption";
            if(pathFile.VerifyPath(TextBoxPath.Text))
            {
                InfoPath.Text += ": Correct path";
            }else InfoPath.Text += ": Wrong path";
        }

        private void EDCreateButtonPath_Click(object sender, RoutedEventArgs e)
        {
            InfoPath.Text = "Searching file for encryption/decryption";
            string path = TextBoxPath.Text;
            if (pathFile.VerifyPath(path))
            {
                var fileContent = File.ReadAllText(path);
                symmetricViewModel.SetPlaintext(fileContent);
                symmetricViewModel.Encrypt();
                string plaintext = symmetricViewModel.Decrypt();
                pathFile.CreateFile(path, "AES_Algorithm", "<--EncryptedText-->\n"+symmetricViewModel.GetCiphertext().ToString()+"\n\n<--DecryptedText-->\n"+plaintext);

            }
            
            TextBoxPath.Text = "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
