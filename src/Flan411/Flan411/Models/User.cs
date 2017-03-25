using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flan411.Models
{
    public class User
    {
        #region Attributes
        private string userName;
        private string password;
        private string token;
        private int uid;
        #endregion

        #region Properties

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public string Token
        {
            get { return token; }
            set { token = value; }
        }
        public int Uid
        {
            get { return uid; }
            set { uid = value; }
        }
        #endregion

        #region Constructors
        public User()
        {
            userName = password = token = "Undefined";
            uid = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="token">T411 API's authentication token</param>
        /// <param name="uid">User id</param>
        public User(string userName, string password, string token, int uid)
        {
            this.userName = userName;
            this.password = password;
            this.token = token;
            this.uid = uid;
        }  
        #endregion
    }
}
