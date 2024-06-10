using EncryptedChatApp.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
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
using System.IO;
namespace EncryptedChatApp.MVVM.View
{
  
    public partial class AsymmetricView : UserControl
    {
        public string? plaintext { get; set; }
        public static AsymmetricView? AsymmetricInstance { get; private set; }
        private AsymmetricViewModel asymmetricViewModel;
        private PathFileManager pathFile;
        public AsymmetricView()
        {
            InitializeComponent();
            AsymmetricInstance = this;
            asymmetricViewModel = new AsymmetricViewModel();
            pathFile = new PathFileManager();
        }
        public void SetAsymmetricPlainTextValue(string message)
        {
            plaintext = message;
            AsymmetricPlainText.Text = plaintext;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            AsymmetricPlainText.Text = "";
            AsymmetricEncryptedText.Text = "";
            AsymmetricDecryptedText.Text = "";
            TextBoxPath.Text = "";
            InfoPath.Text = "Searching file for encryption/decryption";
        }
        public void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (AsymmetricPlainText.Text != "")
            {
                plaintext = AsymmetricPlainText.Text;
                asymmetricViewModel.SetPlaintext(plaintext);
                asymmetricViewModel.Encrypt();
                AsymmetricEncryptedText.Text = asymmetricViewModel.GetCiphertext().ToString();

            }
        }
        public void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (AsymmetricEncryptedText.Text != "")
            {
                asymmetricViewModel.SetCiphertext(AsymmetricPlainText.Text);
                string simpletext = asymmetricViewModel.Decrypt();
                AsymmetricDecryptedText.Text = simpletext;
            }
        }

        private void VerifyButtonPath_Click(object sender, RoutedEventArgs e)
        {
            InfoPath.Text = "Searching file for encryption/decryption";
            if (pathFile.VerifyPath(TextBoxPath.Text))
            {
                InfoPath.Text += ": Correct path";
            }
            else InfoPath.Text += ": Wrong path";
        }

        private void EDCreateButtonPath_Click(object sender, RoutedEventArgs e)
        {
            InfoPath.Text = "Searching file for encryption/decryption";
            string path = TextBoxPath.Text;
            if (pathFile.VerifyPath(path))
            {
                var fileContent = File.ReadAllText(path);
                asymmetricViewModel.SetPlaintext(fileContent);
                asymmetricViewModel.Encrypt();
                string plaintext = asymmetricViewModel.Decrypt();
                pathFile.CreateFile(path, "RSA_Algorithm", "<--EncryptedText-->\n" + asymmetricViewModel.GetCiphertext().ToString() + "\n\n<--DecryptedText-->\n" + plaintext);

            }

            TextBoxPath.Text = "";
        }
    }
}
