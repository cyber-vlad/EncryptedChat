using System;
using System.Windows.Controls;


namespace EncryptedChatApp.MVVM.View
{

    public partial class HomeView : UserControl
    {
        public string? plaintext {  get; set; }
        public static HomeView? HomeInstance { get; private set; }
        public HomeView()
        {
            InitializeComponent();
            HomeInstance = this;           
        }
        public void SetHomePlainTextValue(string message)
        {
            plaintext = message;
            DefaultMsg.Content = "Algorithm!!!";
        }
    }
}
