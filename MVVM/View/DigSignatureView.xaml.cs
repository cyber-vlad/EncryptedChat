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

    public partial class DigSignatureView : UserControl
    {
        public string? plaintext { get; set; }
        public static DigSignatureView? DigSignatureInstance { get; private set; }
        private DigSignatureViewModel digSignatureViewModel;
        private PathFileManager pathFile;
        public DigSignatureView()
        {
            InitializeComponent();
            DigSignatureInstance = this;
            digSignatureViewModel = new DigSignatureViewModel();
            pathFile = new PathFileManager();
        }
        public void SetDigSignaturePlainTextValue(string message)
        {
            plaintext = message;
            DigSignaturePlainText.Text = plaintext;
        }
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            DigSignaturePlainText.Text = "";
            DigitalSignatureText.Text = "";
            ModifyTextBox.Text = "";
            InfoDigitalSignature.Text = "";
            TextBoxPath.Text = "";
            InfoPath.Text = "Searching file for generating digital signature";
        }

        private void GenerateDigitalSignature_Click(object sender, RoutedEventArgs e)
        {
            if (DigSignaturePlainText.Text != "")
            {
                plaintext = DigSignaturePlainText.Text;
                digSignatureViewModel.SetPlaintext(plaintext);
                digSignatureViewModel.ComputeHash(plaintext);
                digSignatureViewModel.GenerateDigitalSignature();
                DigitalSignatureText.Text = digSignatureViewModel.GetDigitalSignature();
            }
        }
        private void ModifyButton_Click(object sender, RoutedEventArgs e)
        {
            if (ModifyTextBox.Text != "")
            {
                plaintext = ModifyTextBox.Text;
                digSignatureViewModel.SetPlaintext(plaintext);
            }
        }

        private void VerifyDigitalSignature_Click(object sender, RoutedEventArgs e)
        {
            if (DigitalSignatureText.Text != "")
            {
                digSignatureViewModel.ComputeHash(digSignatureViewModel.GetPlaintext());
                if (digSignatureViewModel.VerifyDigitalSignature())
                {
                    InfoDigitalSignature.Foreground = Brushes.Green;
                    InfoDigitalSignature.Text = "Digital Signature is valid";
                }
                else
                {
                    InfoDigitalSignature.Foreground = Brushes.Red;
                    InfoDigitalSignature.Text = "Digital Signature is NOT valid";
                }
            }
        }
        private void VerifyButtonPath_Click(object sender, RoutedEventArgs e)
        {
            InfoPath.Text = "Searching file for generating digital signature";
            if (pathFile.VerifyPath(TextBoxPath.Text))
            {
                InfoPath.Text += ": Correct path";
            }
            else InfoPath.Text += ": Wrong path";
        }

        private void EDCreateButtonPath_Click(object sender, RoutedEventArgs e)
        {
            InfoPath.Text = "Searching file for generating digital signature";
            string path = TextBoxPath.Text;
            if (pathFile.VerifyPath(path))
            {
                var fileContent = File.ReadAllText(path);
                digSignatureViewModel.SetPlaintext(fileContent);
                digSignatureViewModel.ComputeHash(fileContent);
                digSignatureViewModel.GenerateDigitalSignature();
                pathFile.CreateFile(path, "DigitalSignature", digSignatureViewModel.GetDigitalSignature());

            }

            
        }

        private void CheckTextarea(object sender, KeyEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (e.Key == Key.Enter)
            {

                var lineIndex = textBox.GetLineIndexFromCharacterIndex(textBox.CaretIndex);
                var lineCount = textBox.LineCount;

                if (textBox.CaretIndex < textBox.Text.Length)
                    lineCount--;

                if (lineCount > 2)
                {
                    e.Handled = true;
                }
            }
        }

        private void IsValidDigitalSignature_Click(object sender, RoutedEventArgs e)
        {
            string path = TextBoxPath.Text;
            if (pathFile.VerifyPath(path))
            {
                var oldSignature = digSignatureViewModel.GetDigitalSignature();
                var fileContent = File.ReadAllText(path);
                digSignatureViewModel.ComputeHash(fileContent);
                digSignatureViewModel.GenerateDigitalSignature();
                if (oldSignature == digSignatureViewModel.GetDigitalSignature())
                {
                    File.WriteAllText(path, fileContent+"\n\nDigitalSignature is valid");
                }
                else File.WriteAllText(path, fileContent+"\n\nDigitalSignature is NOT valid");

            }
        }
    }
}