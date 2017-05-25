using Flan411.Tools;
using Flan411.ViewModels;
using System;
using System.IO;
using System.Windows;

namespace Flan411
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            base.DataContext = new NavigationViewModel();
            NavigationViewModel navigationViewModel = base.DataContext as NavigationViewModel;
            /*
             * DEBUG: for navigation only. We need to verify if user already authenticated once
             * (check if the credentials are present in a configuration file or a registry ...)
             * if no credentials are found selectedViweModel = LoginViewModel. Else,
             * selectedViweModel = SearchViewModel.
             **/
            if (!T411Service.VerifyToken())
            {
                navigationViewModel.SelectedViewModel = new LoginViewModel(navigationViewModel);
            }
            else
            {
                navigationViewModel.SelectedViewModel = new SearchViewModel();
            }
        }
    }
}
