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
        static private readonly string AUTHENTICATION_URL = "https://api.t411.ai/auth";
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
                    // Update application's authentication information
                    Application.Current.Properties["UserName"] = userName;
                    Application.Current.Properties["Password"] = password;
                    Application.Current.Properties["Token"] = (string)jsonResponseObject["token"];
                    Application.Current.Properties["Uid"] = (int)jsonResponseObject["uid"];

                    // DEBUG
                    Console.WriteLine($"Token from properties: {Application.Current.Properties["Token"]}");

                    return new User(userName, password, (string)jsonResponseObject["token"], (int)jsonResponseObject["uid"]);
                }
            }

            return null;
        }

        private async Task<User> Login(string userName, string password)
        {
            User user = null;

            try
            {
                user = await AuthenticateUser(userName, password);

                if (user != null)
                {
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
            if (userNameTextBox.Text != null && passwordTextBox.Password != null)
            {
                await Login(userNameTextBox.Text, passwordTextBox.Password);
            }
            else
            {
                MessageBox.Show("Username or password is missing", "Authentication failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
