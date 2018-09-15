using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace TodoApp.Models
{
    public class CustomRoleProvider : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        // 指定されたユーザーが所属するロールを配列で返すメソッド
        public override string[] GetRolesForUser(string username)
        {
            if ("administrator".Equals(username))
            {
                return new string[] { "Admnistrator" };
            }
            return new string[] { "Users"};
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        //引数で指定されたuserが同じく引数で指定されたロールに差音在するか確認する
            public override bool IsUserInRole(string username, string roleName)
        {
            if ("administrator".Equals(username) && "Administrator".Equals(roleName))
            {
                return true;
            }
            if ("user".Equals(username) && "User".Equals(roleName))
            {
                return true;
            }
            return false;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}