using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mavplus.RovioDriver
{
    public enum UserGroups
    {
        /// <summary>
        /// 未知。
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// 管理员账号
        /// </summary>
        Administrator,
        /// <summary>
        /// 普通用户
        /// </summary>
        User,
    }
    public class UserInformation
    {
        public UserGroups Group { get; private set; }
        public string UserName { get; private set; }

        public UserInformation(UserGroups group, string username)
        {
            this.Group = group;
            this.UserName = username;
        }
    }
}
