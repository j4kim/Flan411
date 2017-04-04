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
        private List<Torrent> torrentList;
        public TorrentListView()
        {
            InitializeComponent();
            torrentList = new List<Torrent>();
        }
    }
}
