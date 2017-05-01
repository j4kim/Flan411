using Flan411.Models;
using Flan411.Tools;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Flan411.Views
{
    /// <summary>
    /// Logique d'interaction pour TorrentDetailsView.xaml
    /// </summary>
    public partial class TorrentDetailsView : UserControl
    {
        private Torrent torrent;
        private static readonly string FILE_EXTENSION = ".torrent";
        private static readonly char[] PATH_INVALID_CHARS = System.IO.Path.GetInvalidFileNameChars();

        public Torrent Torrent {
            get{ return torrent; }
            set {
                Visibility = Visibility.Visible;
                DataContext = torrent = value;
                setContent(value);
            }
        }

        public TorrentDetailsView()
        {
            InitializeComponent();
        }

        private void setContent(Torrent torrent)
        {
            // todo: display torrent details in a Grid (DataGrid ?)
            // todo: load torrent details in the WebBrowser component 
        }

        private async void dlButton_Click(object sender, RoutedEventArgs e)
        {
            
            String normalizeFileName = String.Join("_", torrent.Name.Split(PATH_INVALID_CHARS, StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');
            normalizeFileName += FILE_EXTENSION;
            Console.WriteLine(normalizeFileName);
            Console.WriteLine($"Downloading torrent having id {torrent.Id}");
            try
            {
                String savedFileName = await T411Service.Download(Convert.ToInt64(torrent.Id), normalizeFileName);
                if (savedFileName != String.Empty)
                {
                    System.Diagnostics.Process.Start(savedFileName);
                }
                else
                {
                    MessageBox.Show("An error occured during either the torrent file download or opening.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception error)
            {

                MessageBox.Show($"{error.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); ;
            }
            
        }
    }
}
