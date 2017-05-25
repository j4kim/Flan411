using Flan411.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Flan411.Tools
{
    public class T411Service
    {
        static private readonly string APP_FOLDER = getAppFolder();

        static private readonly string DEFAULT_HOST_NAME = "https://api.t411.al";
        static private readonly string HOST_NAME_FILENAME = Path.Combine(APP_FOLDER, "api_hostname.txt");
        static private readonly string HOST_NAME = getHostName();

        static private readonly string AUTHENTICATION_URL = HOST_NAME + "/auth";
        static private readonly string DOWNLOAD_URL = HOST_NAME + "/torrents/download";
        
        // Categories IDs, for search filtering
        static public int CID_SERIES = 433;
        static public int CID_MOVIES = 631;
        static public int CID_ANIMATION = 455;
        static public int CID_SERIES_ANIM = 637;

        static private readonly string TOKEN_FILENAME = Path.Combine(APP_FOLDER, ".token");
        static private string TOKEN = "";

        private static string getAppFolder()
        {
            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            appdata = Path.Combine(appdata, "Flan411");
            if (!Directory.Exists(appdata))
                Directory.CreateDirectory(appdata);

            string torrentsPath = Path.Combine(appdata, "torrents");
            if (!Directory.Exists(torrentsPath))
                Directory.CreateDirectory(torrentsPath);

            return appdata;
        }

        private static string getHostName()
        {
            try
            {
                return File.ReadAllText(HOST_NAME_FILENAME);
            }
            catch (FileNotFoundException)
            {
                File.WriteAllText(HOST_NAME_FILENAME, DEFAULT_HOST_NAME);
                return DEFAULT_HOST_NAME;
            }
        }

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
                    throw new Exception("Error while parsing JSON response.");
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

        public static async Task<string> Download(long torrentId, string fileName)
        {
            byte[] result;
            byte[] buffer = new byte[4096];

            WebRequest wr = WebRequest.Create($"{DOWNLOAD_URL}/{torrentId}");
            wr.Headers.Add("Authorization", TOKEN);
            wr.ContentType = "application/x-bittorrent";

            using (var response = await wr.GetResponseAsync())
            {
                bool gzip = response.Headers["Content-Encoding"] == "gzip";
                var responseStream = gzip
                                        ? new GZipStream(response.GetResponseStream(), CompressionMode.Decompress)
                                        : response.GetResponseStream();

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = responseStream.Read(buffer, 0, buffer.Length);
                        memoryStream.Write(buffer, 0, count);
                    } while (count != 0);

                    result = memoryStream.ToArray();

                    fileName = Path.Combine(APP_FOLDER, "torrents", fileName);
                    using (BinaryWriter writer = new BinaryWriter(new FileStream(fileName, FileMode.Create)))
                    {
                        writer.Write(result);
                    }
                }
            }
            return fileName;
        }

        /// <summary>
        /// Makes a search request on the t411 engine
        /// </summary>
        /// <param name="pattern">The query</param>
        /// <param name="limit">The max amount of resluts</param>
        /// <param name="cid">The category ID, -1 for all categories</param>
        /// <returns>A list of Torrent objects</returns>
        public static async Task<List<Torrent>> Search(string pattern, int limit=5000, int cid=-1)
        {
            using (var httpClient = new HttpClient())
            {
                List<Torrent> torrents = new List<Torrent>();

                // Necessary header for each request, should we have only one instance of HttpClient ?
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", TOKEN);

                string options = $"?limit={limit}";
                if(cid != -1)
                    // category id : Série TV -> 433, Film -> 631, Animation -> 455, Série animée -> 637
                    options += $"&cid={cid}";
                
                var strResult = await httpClient.GetStringAsync($"{HOST_NAME}/torrents/search/{pattern}{options}");

                JObject result = JsonConvert.DeserializeObject(strResult) as JObject;

                // DEBUG
                //File.WriteAllText(Path.Combine(APP_FOLDER, "result.json"), result.ToString());

                // the error field occurs if the token is invalid
                if (result["error"] != null)
                {
                    // DEBUG: often SQLSTATE[HY000] [2002] Connection refused
                    throw new Exception(result["error"].ToString());
                    //return null;
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
                        Console.WriteLine(e);
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
        /// Get the detail of a particular torrent.
        /// </summary>
        /// <param name="id">The id of the torrent</param>
        /// <returns>A TorrentDetail object containing an HTML formatted description</returns>
        public static async Task<TorrentDetail> Details(int id)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", TOKEN);
                string strResult = await httpClient.GetStringAsync($"{HOST_NAME}/torrents/details/{id}");

                JObject result = JsonConvert.DeserializeObject(strResult) as JObject;
                TorrentDetail torrentDetail = result.ToObject<TorrentDetail>();

                return torrentDetail;
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

            return TestAPI();
        }

        /// <summary>
        /// Check token validity by performing a simple synchronous search request
        /// </summary>
        /// <returns>false if there is a "error" field in the json response from the api, true otherwise</returns>
        public static bool TestAPI()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", TOKEN);
                var strResult = httpClient.GetStringAsync($"{HOST_NAME}/torrents/search/query with no results").Result;

                JObject result = JsonConvert.DeserializeObject(strResult) as JObject;

                if (result["error"] != null)
                {
                    Console.WriteLine(result["error"]);
                    return false;
                }

                 return true;
            }
        }
    }
}

