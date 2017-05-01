using Flan411.Models;
using System.Collections.Generic;
using System.Windows.Controls;

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
    }
}
