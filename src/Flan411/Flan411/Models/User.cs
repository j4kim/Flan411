﻿namespace Flan411.Models
{
    public class User
    {
        #region Properties
        public string UserName;
        public string Password;
        public string Token;
        public int Uid;
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
