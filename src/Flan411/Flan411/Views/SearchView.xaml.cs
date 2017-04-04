using Flan411.Tools;
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
    /// Logique d'interaction pour SearchView.xaml
    /// </summary>
    public partial class SearchView : UserControl
    {
        public SearchView()
        {
            InitializeComponent();
            searchButton.Click += SearchButton_Click;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (searchInput.Text == "")
            {
                return;
            }
            var torrentsList = T411Service.Search(searchInput.Text);
            // DEBUG
            {
                foreach (var item in torrentsList)
                {
                    Console.WriteLine($"seeders: {item.Seeders}");
                }
            }
            torrentListView.TorrentList = torrentsList;
        }
    }
}
