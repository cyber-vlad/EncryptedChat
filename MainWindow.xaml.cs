using EncryptedChatApp.MVVM.View;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using static System.Net.Mime.MediaTypeNames;

namespace EncryptedChatApp
{
    public partial class MainWindow : Window
    {
        
        public string? plaintext { get; private set; }
        public static MainWindow? MainInstance { get; set; }
        
        public MainWindow()
        {
            InitializeComponent();
            MainInstance = this;

        }
        //private void MoveToTestViewButton_Click(object sender, RoutedEventArgs e)
        //{
        //    currentView.Children.Clear();
        //    currentView.Children.Add(new TestView());
        //}
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (PlainTextBox.Text != "")
            {
                plaintext = PlainTextBox.Text;
                if (HomeView.HomeInstance != null)
                {
                    HomeView.HomeInstance.SetHomePlainTextValue(plaintext);
                }
                if (SymmetricView.SymmetricInstance != null)
                {
                    SymmetricView.SymmetricInstance.SetSymmetricPlainTextValue(plaintext);
                }
                if (AsymmetricView.AsymmetricInstance != null)
                {
                    AsymmetricView.AsymmetricInstance.SetAsymmetricPlainTextValue(plaintext);
                }
                if(DigSignatureView.DigSignatureInstance != null)
                {
                    DigSignatureView.DigSignatureInstance.SetDigSignaturePlainTextValue(plaintext);
                }
                PlainTextBox.Text = "";
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

                if (lineCount > 3)
                {
                    e.Handled = true;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}