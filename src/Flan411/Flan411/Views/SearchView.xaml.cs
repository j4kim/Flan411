using Flan411.Models;
using Flan411.Tools;
using Newtonsoft.Json;
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
            searchButton.Click += SearchButton_Click;
            Loaded += OnLoaded;

            torrentListView.TorrentDetailsView = torrentDetailsView;
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
            DisplayInformationMessage("Waiting for results...");

            List<Torrent> torrentsList;
            try
            {
                torrentsList = await T411Service.Search(searchInput.Text);
            }
            catch (JsonSerializationException error)
            {
                Console.WriteLine(error.Message);
                torrentsList = new List<Torrent>();
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
