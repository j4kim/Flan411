using Flan411.Tools;
using System.Windows.Input;

namespace Flan411.ViewModels
{
    class NavigationViewModel : ObservableObject
    {
        public ICommand LoginCommand { get; set; }

        public ICommand SearchCommand { get; set; }

        private object selectedViewModel;

        public object SelectedViewModel
        {
            get { return selectedViewModel; }
            set { selectedViewModel = value; RaisePropertyChanged("SelectedViewModel"); }
        }

        public NavigationViewModel()
        {
            LoginCommand = new BaseCommand(OpenLogin);
            SearchCommand = new BaseCommand(OpenSearch);
        }

        private void OpenLogin(object obj)
        {
            SelectedViewModel = new LoginViewModel();
        }

        private void OpenSearch(object obj)
        {
            SelectedViewModel = new SearchViewModel();
        }
    }
}
