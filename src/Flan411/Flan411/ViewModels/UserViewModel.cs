using Flan411.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flan411.ViewModels
{
    class UserViewModel : INotifyPropertyChanged
    {
        #region Attributes
        User user;
        #endregion

        #region Properties
        public User User { get; set; }     
        #endregion

        #region Constructors
        public UserViewModel()
        {
            user = new User();
        }

        public UserViewModel(User user)
        {
            this.user = user;
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        // Create the OnPropertyChanged method to raise the event
        protected void RaisePropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
