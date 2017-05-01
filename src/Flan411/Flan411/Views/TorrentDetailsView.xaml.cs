using Flan411.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Flan411.Views
{
    /// <summary>
    /// Logique d'interaction pour TorrentDetailsView.xaml
    /// </summary>
    public partial class TorrentDetailsView : UserControl
    {
        private Torrent torrent;

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

        private void dlButton_Click(object sender, RoutedEventArgs e)
        {
            // todo
        }
    }
}
