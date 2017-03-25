using Flan411.Models;
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
        private UserModel user;
        static private readonly string AUTHENTICATION_URL = "https://api.t411.li/auth";
        #endregion
        #region Properties
        public UserModel User { get { return user; } set { user = value; } }
        #endregion

        public LoginView()
        {
            this.user = null;
            InitializeComponent();
        }

        public async Task<UserModel> AuthenticateUser(string userName, string password)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                FormUrlEncodedContent formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", userName),
                    new KeyValuePair<string, string>("password", password)
                });

                HttpResponseMessage response = await httpClient.PostAsync(AUTHENTICATION_URL, formContent);
                string content = await response.Content.ReadAsStringAsync();
                JObject jsonResponseObject = (JObject) JsonConvert.DeserializeObject(content);

                Console.WriteLine($"Code HTTP: {(int) response.StatusCode}");
                //Console.WriteLine($"JSON object code: {jsonResponseObject["code"]}");
                Console.WriteLine(content);

                if (jsonResponseObject["token"] != null)
                {
                    return new UserModel(userName, password, (string)jsonResponseObject["token"], (int)jsonResponseObject["uid"]);
                }
            }

            return null;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (userNameTextBox.Text != "" && passwordTextBox.Password != "")
            {
                user = await AuthenticateUser(userNameTextBox.Text, passwordTextBox.Password);
                if (user != null)
                {
                    MessageBox.Show($"Your token: {user.Token}", "Authentication successful", MessageBoxButton.OK, MessageBoxImage.Error);
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
