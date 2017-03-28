using System;
using System.Threading.Tasks;
using Flan411.Models;
using Flan411.Tools;
using System.Windows;
using System.Net.Http;
using System.Diagnostics;

namespace Flan411.ViewModels
{
    class LoginViewModel : ObservableObject
    {
        public NavigationViewModel NavigationViewModel { get; set; }

        public LoginViewModel(NavigationViewModel navigationViewModel)
        {
            NavigationViewModel = navigationViewModel;
        }

        /// <summary>
        /// Login to T411 API. Sets the credentials in the current application's properties.
        /// A User object is returned if the authentication was successful.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<User> Login(string userName, string password)
        {
            User user = null;

            if (NavigationViewModel == null)
            {
                // DEBUG
                {
                    throw new Exception($"Error::no NavigationViewModel accessible");
                }
            }


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
                        Console.WriteLine($"Your token: {user.Token}", "Authentication successful");
                    }

                    // Switch to next view if login is successful (might be DANGEROUS to put it here)
                    NavigationViewModel.SelectedViewModel = new SearchViewModel();
                }
                else
                {
                    // DEBUG
                    {
                        Console.WriteLine("Username or password might be incorrect", "Authentication failed");
                    }
                }
            }
            catch (HttpRequestException httpError)
            {
                // DEBUG
                {
                    Console.WriteLine($"{httpError.Message}", "Connection to remote server failed");
                }
            }

            return user;
        }
    }
}
