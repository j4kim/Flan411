using Flan411.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
                tvShowInfo = result.ToObject<TvMazeTvShowInfo>();
                tvShowInfo.SetEpisodes(result["_embedded"]["episodes"].ToObject<List<TvMazeTvShowInfo.TvMazeEpisode>>().ToArray());
            }

            // Debug
            {
                foreach (var row in tvShowInfo.Episodes)
                {
                    foreach (var col in row)
                    {
                        Console.WriteLine(col.Name);
                    }
                }
            }
            
            return tvShowInfo;
        }

    }
}
