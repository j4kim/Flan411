using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flan411.Models
{
    public class User
    {
        #region Properties
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public int Uid { get; set; }
        #endregion

        #region Constructors
        public User() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName">T411 user's name</param>
        /// <param name="password">T411 user's password</param>
        /// <param name="token">T411 API's authentication token</param>
        /// <param name="uid">User id</param>
        public User(string userName, string password, string token, int uid)
        {
            UserName = userName;
            Password = password;
            Token = token;
            Uid = uid;
        }
        #endregion
    }
}
