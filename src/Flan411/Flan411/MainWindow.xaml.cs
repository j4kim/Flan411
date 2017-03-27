using Flan411.ViewModels;
using System.Windows;

namespace Flan411
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UserViewModel userModel = new UserViewModel();

        public MainWindow()
        {
            InitializeComponent();
            base.DataContext = userModel;
        }
    }
}
