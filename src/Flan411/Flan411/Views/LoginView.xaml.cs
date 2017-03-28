using Flan411.Models;
using Flan411.Tools;
using Flan411.ViewModels;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Flan411.Views
{
    /// <summary>
    /// Logique d'interaction pour LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {

        public LoginView()
        {
            InitializeComponent();
        }

        private async Task<User> Login(string userName, string password)
        {
            User user = null;

            try
            {
                user = await T411Service.AuthenticateUser(userName, password);

                if (user != null)
                {
                    // Update user's information to share it with other views
                    Application.Current.Properties["UserName"] = user.UserName;
                    Application.Current.Properties["Password"] = user.Password;
                    Application.Current.Properties["Token"] = user.Token;
                    Application.Current.Properties["Uid"] = user.Uid;

                    // DEBUG
                    {
                        string className = this.GetType().Name;
                        string methodeName = new StackTrace().GetFrame(1).GetMethod().Name;
                        Console.WriteLine($"{className}::{methodeName} Token from properties: {Application.Current.Properties["Token"]}");
                    }

                    MessageBox.Show($"Your token: {user.Token}", "Authentication successful", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Username or password might be incorrect", "Authentication failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (HttpRequestException httpError)
            {
                MessageBox.Show($"{httpError.Message}", "Connection to remote server failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return user;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (userNameTextBox.Text.Length > 0 && passwordTextBox.Password.Length > 0)
            {
                NavigationViewModel nav = DataContext as NavigationViewModel;

                if (nav == null)
                {
                    // DEBUG
                    {
                        MessageBox.Show($"Error::no NavigationViewModel accessible");
                        return;
                    }
                }

                User user = await Login(userNameTextBox.Text, passwordTextBox.Password);

                if (user != null)
                {
                    nav.SelectedViewModel = new SearchViewModel();
                }
            }
            else
            {
                MessageBox.Show("Username or password is missing", "Authentication failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
