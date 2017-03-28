using Flan411.Tools;
using System.Windows.Input;

namespace Flan411.ViewModels
{
    /// <summary>
    /// ViewModel used to allow View switching in the MainWindow.
    /// </summary>
    class NavigationViewModel : ObservableObject
    {
        #region Properties
        /**
         * View switching commands
         **/
        public ICommand LoginCommand { get; set; }

        public ICommand SearchCommand { get; set; }
        
        public object SelectedViewModel
        {
            get { return selectedViewModel; }
            set { selectedViewModel = value; RaisePropertyChanged("SelectedViewModel"); }
        }

        #endregion

        private object selectedViewModel;

        public NavigationViewModel()
        {
            LoginCommand = new BaseCommand(OpenLogin);
            SearchCommand = new BaseCommand(OpenSearch);
        }

        #region View switching methods
        private void OpenLogin(object obj)
        {
            SelectedViewModel = new LoginViewModel(this);
        }

        private void OpenSearch(object obj)
        {
            SelectedViewModel = new SearchViewModel();
        }
        #endregion
    }
}
