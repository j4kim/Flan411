using Flan411.Models;
using Flan411.Tools;

namespace Flan411.ViewModels
{
    class UserViewModel : ObservableObject
    {
        #region Properties

        public User User;     
        
        #endregion

        #region Constructors

        public UserViewModel()
        {
            User = new User();
        }

        public UserViewModel(User user)
        {
            User = user;
        }

        #endregion
    }
}
