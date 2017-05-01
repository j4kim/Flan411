using Flan411.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Flan411.Tools
{
    public class TvMazeService
    {
        static private readonly string HOST_NAME = "http://api.tvmaze.com";
        static private readonly string SEARCH_SHOW_URL = HOST_NAME + "/singlesearch/shows";

        public static async Task<TvMazeTvShowInfo> Search(string title)
        {
            string url = string.Format(
            SEARCH_SHOW_URL + "?q={0}&embed={1}",
            Uri.EscapeDataString(title),
            Uri.EscapeDataString("episodes"));

            TvMazeTvShowInfo tvShowInfo;

            using (var httpClient = new HttpClient())
            {
                string strResult = await httpClient.GetStringAsync(url);
                JObject result = JsonConvert.DeserializeObject(strResult) as JObject;
                if (result == null)
                {
                    throw new Exception("No tv show found");
                }
                tvShowInfo = result.ToObject<TvMazeTvShowInfo>();
                tvShowInfo.PosterUrl = result["image"]["medium"].ToString();
                tvShowInfo.SetEpisodes(result["_embedded"]["episodes"].ToObject<List<TvMazeTvShowInfo.TvMazeEpisode>>().ToArray());
            }

            // Debug
            {
                Console.WriteLine("TV SHOW INFOS:");
                Console.WriteLine($"Time: {tvShowInfo.Schedule.Time}, Day: {tvShowInfo.Schedule.Days.Length}");
                
                Console.WriteLine("Episodes:");
                foreach (var row in tvShowInfo.Episodes)
                {
                    foreach (var col in row)
                    {
                        Console.WriteLine($"name: {col.Name}");
                        Console.WriteLine($"summary: {col.Summary}");
                    }
                }
            }
            
            return tvShowInfo;
        }

    }
}
