using Flan411.Models;
using Flan411.ViewModels;
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

        internal void SetUserName(string userName)
        {
            userModel.UserName = userName;
        }
    }
}
