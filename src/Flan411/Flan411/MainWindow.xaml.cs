using Flan411.ViewModels;
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
            ((NavigationViewModel)DataContext).SelectedViewModel = new LoginViewModel();
        }
    }
}
