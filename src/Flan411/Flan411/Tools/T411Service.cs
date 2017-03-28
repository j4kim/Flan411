using Flan411.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Flan411.Tools
{
    public class T411Service
    {
        static private readonly string HOST_NAME = "https://api.t411.ai";
        static private readonly string AUTHENTICATION_URL = HOST_NAME + "/auth";

        static private string TOKEN = "";

        /// <summary>
        /// Authenticates the user to the T411 API and updates the window's datacontext with the user's information.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>User instance if authentication succeeded, false otherwise.</returns>
        static public async Task<User> AuthenticateUser(string userName, string password)
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
                    return new User { UserName = userName, Password = password, Token = TOKEN, Uid = (int)jsonResponseObject["uid"] };
                }
            }

            return null;
        }

        public static List<Torrent> Search(string pattern)
        {
            using (HttpClient http = new HttpClient())
            {
                // Necessary header for each request, should we have only one instance of HttpClient ?
                http.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", TOKEN);

                /*
                 * limites et pagination : /torrents/search/{query}?offset=10&limit=5
                 */
                Task<string> task = http.GetStringAsync($"{HOST_NAME}/torrents/search/{pattern}");
                Console.WriteLine("je cherche couz");
                task.Wait();
                JObject result = JsonConvert.DeserializeObject(task.Result) as JObject;
                Console.WriteLine(result);
                
                List<Torrent> torrents = new List<Torrent>();
                foreach (var tor in result["torrents"])
                {
                    Torrent torrent = JsonConvert.DeserializeObject<Torrent>(tor.ToString());
                    torrents.Add(torrent);
                }
                return torrents;
            }
        }


    }
}
