using Flan411.Models;
using Flan411.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace Flan411.Views
{
    /// <summary>
    /// Logique d'interaction pour LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        #region Attributes
        static private readonly string AUTHENTICATION_URL = "https://api.t411.li/auth";
        #endregion
        #region Properties
        #endregion

        public LoginView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Authenticates the user to the T411 API and updates the window's datacontext with the user's information.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>User instance if authentication succeeded, false otherwise.</returns>
        public async Task<User> AuthenticateUser(string userName, string password)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Retrieve the parents datacontext to update it
                UserViewModel flan411Container = DataContext as UserViewModel;

                if (flan411Container == null) // If container cannot be casted
                {
                    Console.WriteLine($"Error in {this.GetType().Name}::AuthenticateUser", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                // Prepare POST parameters
                FormUrlEncodedContent formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", userName),
                    new KeyValuePair<string, string>("password", password)
                });

                // Send HTTP POST request and retrieve content as a string
                HttpResponseMessage response = await httpClient.PostAsync(AUTHENTICATION_URL, formContent);
                string content = await response.Content.ReadAsStringAsync();

                // Cast content as a JSON object to make information processing easier
                JObject jsonResponseObject = (JObject)JsonConvert.DeserializeObject(content);

                // DEBUG
                Console.WriteLine($"Code HTTP: {(int)response.StatusCode}");
                Console.WriteLine($"JSON object code: {jsonResponseObject["code"]}");
                Console.WriteLine("Response string:");
                Console.WriteLine(content);

                // If we get the token, we update the datacontext and return the User model
                if (jsonResponseObject["token"] != null)
                {
                    flan411Container.UserName = userName;
                    flan411Container.Password = password;
                    flan411Container.Token = (string)jsonResponseObject["token"];
                    flan411Container.Uid = (int)jsonResponseObject["uid"];
                    //flan411Container.User = new User(userName, password, (string)jsonResponseObject["token"], (int)jsonResponseObject["uid"]);
                    return flan411Container.User;
                }
            }

            return null;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (userNameTextBox.Text != null && passwordTextBox.Password != null)
            {
                User user = await AuthenticateUser(userNameTextBox.Text, passwordTextBox.Password);
                if (user != null)
                {
                    MessageBox.Show($"Your token: {user.Token}", "Authentication successful", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Username or password might be incorrect", "Authentication failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Username or password is missing", "Authentication failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
