using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EncryptedChatApp.Core;
namespace EncryptedChatApp.MVVM.ViewModel
{
    class MainViewModel : ObservableObject 
    {
        //public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand SymmetricViewCommand { get; set; }
        public RelayCommand AsymmetricViewCommand { get; set; }
        public RelayCommand DigSignatureCommand { get; set; }

        public HomeViewModel HomeVM { get; set; }
        public SymmetricViewModel SymmetricVM {  get; set; }
        public AsymmetricViewModel AsymmetricVM { get; set; }
        public DigSignatureViewModel DigSignatureVM { get; set; }

        private object? _currentView;
        public object CurrentView 
        { 
            get { return _currentView!; } 
            set { _currentView = value; OnPropertyChanged(); }
        }
        public MainViewModel()
        { 
            HomeVM = new HomeViewModel();
            SymmetricVM = new SymmetricViewModel();
            AsymmetricVM = new AsymmetricViewModel();
            DigSignatureVM = new DigSignatureViewModel();

            CurrentView = HomeVM;

            SymmetricViewCommand = new RelayCommand(o =>
            {
                CurrentView = SymmetricVM;
            });
            AsymmetricViewCommand = new RelayCommand(o =>
            {
                CurrentView = AsymmetricVM;
            });
            DigSignatureCommand = new RelayCommand(o =>
            {
                CurrentView = DigSignatureVM;
            });
        }
    }
}
