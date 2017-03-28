using Flan411.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Flan411.Views
{
    public partial class LoginView : UserControl
    {
        private LoginViewModel LoginViewModel;
        public bool LoginIsPending { get; private set; }

        public LoginView()
        {
            InitializeComponent();
            LoginIsPending = false;
            this.Loaded += updateDataContext; // to be sure DataContext exists whene retrieved
        }

        #region Event Handlers
        private void updateDataContext(object sender, RoutedEventArgs e)
        {
            // Dangerous because there is no guarantee that the cast succeeds.
            LoginViewModel = new LoginViewModel(DataContext as NavigationViewModel);
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (LoginIsPending)
            {
                return;
            }
            else if (userNameTextBox.Text.Length > 0 && passwordTextBox.Password.Length > 0 && !LoginIsPending)
            {
                LoginIsPending = true;

                await LoginViewModel.Login(userNameTextBox.Text, passwordTextBox.Password);

                LoginIsPending = false;
            }
            else
            {
                MessageBox.Show("Username or password is missing", "Authentication failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
    }
}
