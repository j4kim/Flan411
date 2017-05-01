using Flan411.Models;
using Flan411.Tools;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Flan411.Views
{
    /// <summary>
    /// Logique d'interaction pour SearchView.xaml
    /// </summary>
    public partial class SearchView : UserControl
    {
        public SearchView()
        {
            InitializeComponent();
            searchTvShowBtn.Click += SearchButton_Click;
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            DisplayInformationMessage("Please enter a search term.");
        }

        private void DisplayInformationMessage(String message)
        {
            torrentListView.Visibility = Visibility.Hidden;
            informationText.Text = message;
            informationText.Visibility = Visibility.Visible;
        }

        private void DisplayTorrentsList()
        {
            informationText.Visibility = Visibility.Hidden;
            torrentListView.Visibility = Visibility.Visible; 
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (searchInput.Text == "")
            {
                return;
            }

            //var torrentsList = await T411Service.Search(searchInput.Text);

            await TvMazeService.Search(searchInput.Text);

            List<Torrent> torrentsList = new List<Torrent>
            {
                new Torrent {Name="The Walking Dead s01e01", Added="12/12/2012", Category="TV Show", CategoryName="Gore", Id="12", Leechers="2450", Seeders="5000", Size="2000000000"},
                new Torrent {Name="The Walking Dead s01e02", Added="12/12/2012", Category="TV Show", CategoryName="Gore", Id="13", Leechers="2450", Seeders="5000", Size="2000000000"},
                new Torrent {Name="The Walking Dead s01e03", Added="12/12/2012", Category="TV Show", CategoryName="Gore", Id="14", Leechers="2450", Seeders="5000", Size="2000000000"}
            };
            // DEBUG
            {
                foreach (var item in torrentsList)
                {
                    Console.WriteLine($"seeders: {item.Seeders}");
                }
            }

            if (torrentsList.Count > 0)
            {
                torrentListView.TorrentList = torrentsList;
                DisplayTorrentsList();
            }
            else
            {
                DisplayInformationMessage("No torrents found.");
            }
        }
    }
}
