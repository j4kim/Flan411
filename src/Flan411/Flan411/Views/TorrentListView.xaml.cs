using Flan411.Models;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;

namespace Flan411.Views
{
    /// <summary>
    /// Logique d'interaction pour TorrentListView.xaml
    /// </summary>
    public partial class TorrentListView : UserControl
    {
        public List<Torrent> TorrentList
        {
            get
            {
                return torrentList;
            }
            set
            {
                torrentList = value;
                torrentResultList.ItemsSource = torrentList;
            }
        }

        public TorrentDetailsView TorrentDetailsView { get; set; }
        public string Order { get; set; }

        private List<Torrent> torrentList;

        public TorrentListView()
        {
            InitializeComponent();
            torrentList = new List<Torrent>();
        }

        private void torrentResultList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Torrent selected = (Torrent) torrentResultList.SelectedItem;
            if (selected != null)
                TorrentDetailsView.Torrent = selected;
        }

        private void RadioButton_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            var rb = (RadioButton)sender;
            Order = rb.Content.ToString();
            if (TorrentList == null) return;
            switch (Order)
            {
                case "Smallest size":
                    TorrentList = TorrentList.OrderBy(t => t.SizeMB).ToList();
                    break;
                case "Most recent":
                    TorrentList = TorrentList.OrderByDescending(t => t.Added).ToList();
                    break;
                case "Most seeded":
                    TorrentList = TorrentList.OrderByDescending(t => t.Seeders).ToList();
                    break;
            }
        }
    }
}
