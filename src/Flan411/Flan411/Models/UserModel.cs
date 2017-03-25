using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flan411.Models
{
    public class UserModel
    {
        #region Attributes
        private string userName;
        private string password;
        private string token;
        private int uid;
        #endregion

        #region Properties
        public string Token { get { return token;  } }
        #endregion

        public UserModel(string userName, string password, string token, int uid)
        {
            this.userName = userName;
            this.password = password;
            this.token = token;
            this.uid = uid;
        }
    }
}
