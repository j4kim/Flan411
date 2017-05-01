using Flan411.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Flan411.Tools
{
    public class T411Service
    {
        static private readonly string HOST_NAME = "http://api.t411.ai";
        static private readonly string AUTHENTICATION_URL = HOST_NAME + "/auth";

        static private readonly string TOKEN_FILENAME = ".token";
        static private string TOKEN = "";

        /// <summary>
        /// Authenticates the user to the T411 API and updates the window's datacontext with the user's information.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>User instance if authentication succeeded, false otherwise.</returns>
        static public async Task<User> AuthenticateUser(string userName, string password)
        {
            using (var httpClient = new HttpClient())
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
                JObject jsonResponseObject = JsonConvert.DeserializeObject(content) as JObject;

                if (jsonResponseObject == null)
                {
                    throw new Exception($"{typeof(T411Service).Name}::{new StackTrace().GetFrame(1).GetMethod().Name} Error while parsing JSON response.");
                }

                // DEBUG
                {
                    string debugString = typeof(T411Service).Name + "::" + new StackTrace().GetFrame(1).GetMethod().Name;
                    Console.WriteLine($"{debugString} Code HTTP: {(int)response.StatusCode}");
                    Console.WriteLine($"{debugString} JSON object code: {jsonResponseObject["code"]}");
                    Console.WriteLine($"{debugString} Response string:\n{content}");
                }

                // If we get the token, we update the datacontext and return the User model
                if (jsonResponseObject["token"] != null)
                {
                    TOKEN = (string)jsonResponseObject["token"];
                    // write the token on the configuration file
                    File.WriteAllText(TOKEN_FILENAME, TOKEN);
                    return new User { UserName = userName, Password = password, Token = TOKEN, Uid = (int)jsonResponseObject["uid"] };
                }
            }

            return null;
        }

        /// <summary>
        /// Makes a search request on the t411 engine
        /// </summary>
        /// <param name="pattern">The query</param>
        /// <returns>A list of torrent objects (10 max by default)</returns>
        public static async Task<List<Torrent>> Search(string pattern)
        {
            using (var httpClient = new HttpClient())
            {
                List<Torrent> torrents = new List<Torrent>();

                // Necessary header for each request, should we have only one instance of HttpClient ?
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", TOKEN);

                string options = "?limit=5000";

                string strResult = await httpClient.GetStringAsync($"{HOST_NAME}/torrents/search/{pattern}{options}");

                JObject result = JsonConvert.DeserializeObject(strResult) as JObject;

                // the error field occurs if the token is invalid
                if (result["error"] != null)
                {
                    // DEBUG: often SQLSTATE[HY000] [2002] Connection refused
                    return null;
                }

                // DEBUG
                {
                    File.WriteAllText("result.json", result.ToString());
                }

                foreach (var tor in result["torrents"])
                {
                    try
                    {
                        Torrent torrent = tor.ToObject<Torrent>();
                        torrents.Add(torrent);
                    }
                    catch (JsonReaderException e)
                    {
                        Console.WriteLine(e.Data);
                    }
                }

                return torrents;
                // DEBUG in case of API unavailability
                //{
                //    return new List<Torrent>
                //    {
                //        new Torrent() {Name="The cat and the dog", Seeders="12", Leechers="6", Size="120000000000" },
                //        new Torrent() {Name="The ground and the sky", Seeders="36", Leechers="16", Size="324000000000" },
                //        new Torrent() {Name="John Doe's gravestone", Seeders="12", Leechers="6", Size="150000000000" },
                //    };
                //}
            }
        }

        /// <summary>
        /// Verify if the configuration file exists and test the validity of the token
        /// </summary>
        /// <returns>true if the token exists and is valid, false otherwise</returns>
        public static bool VerifyToken()
        {
            try
            {
                TOKEN = File.ReadAllText(TOKEN_FILENAME);
            }
            catch (FileNotFoundException)
            {
                return false;
            }
            // check token validity
            // if the token is invalid, the search method will return null
            // maybe we can test with a faster request (get on the user profile, for example)
            return Search("test") != null;
        }
    }
}
