using Flan411.Tools;
using System;
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

            var torrentsList = await T411Service.Search(searchInput.Text);
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
