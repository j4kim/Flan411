using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Flan411.Models
{
    public class TvMazeTvShowInfo
    {
        public int Id;
        public string Name;
        public string[] Genres;
        public string Status;
        public string PosterUrl;
        public string Summary
        {
            get { return summary; }
            set { summary = Regex.Replace(value, "<.*?>", String.Empty); }
        }
        public ShowSchedule Schedule;
        public TvMazeEpisode[][] Episodes { get { return episodes; } }
        public int Seasons { get { return seasons; } }
        public Dictionary<int, int> EpisodesBySeaons {
            get { return episodesBySeason; }
            set { episodesBySeason = value; }
        }

        private TvMazeEpisode[][] episodes;
        private int seasons;
        private string summary;
        private Dictionary<int, int> episodesBySeason;

        internal void SetEpisodes(TvMazeEpisode[] tvMazeEpisode)
        {
            episodesBySeason = CountEpisodesBySeason(tvMazeEpisode);

            episodes = new TvMazeEpisode[episodesBySeason.Count][];

            foreach (var item in episodesBySeason)
            {
                episodes[item.Key - 1] = new TvMazeEpisode[item.Value];
            }

            foreach (var item in tvMazeEpisode)
            {
                episodes[item.Season - 1][item.Number - 1] = new TvMazeEpisode { Id = item.Id, Name = item.Name, Season = item.Season, Number = item.Number, Summary = item.Summary, Runtime = item.Runtime };
            }

            seasons = episodes.Length;
        }

        private static Dictionary<int, int> CountEpisodesBySeason(TvMazeEpisode[] tvMazeEpisode)
        {
            Dictionary<int, int> episodeCounter = new Dictionary<int, int>();
            foreach (var item in tvMazeEpisode)
            {
                if (episodeCounter.ContainsKey(item.Season))
                {
                    episodeCounter[item.Season] += 1;
                }
                else
                {
                    episodeCounter[item.Season] = 1;
                }
            }
            return episodeCounter;
        }

        public class ShowSchedule
        {
            public string Time;
            public string[] Days;
        }

        public class TvMazeEpisode
        {
            public int Id;
            public string Name;
            public int Season;
            public int Number;
            public int Runtime;
            public string Summary
            {
                get { return summary; }
                set { summary = Regex.Replace(value, "<.*?>", String.Empty); }
            }

            private string summary;
        }
    }
}
