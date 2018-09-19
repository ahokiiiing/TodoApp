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
            //if ("administrator".Equals(username))
            //{
            //    return new string[] { "Admnistrator" };
            //}
            using (var db = new TodoesContext())
            {
                var user = db.Users
                    .Where(u => u.UserName == username)
                    .FirstOrDefault();

                if (user != null)
                {
                    //UserNameが存在したときに、ユーザーが所属するRoleを配列で返す
                    return user.Roles.Select(role => role.RoleName).ToArray();
                }
            }
            //当てはまるユーザーがいなかったときは空の配列を返す
            return new string[] { };
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        //引数で指定されたuserが同じく引数で指定されたロールに差音在するか確認する
        public override bool IsUserInRole(string username, string roleName)
        {
            //if ("administrator".Equals(username) && "Administrator".Equals(roleName))
            //{
            //    return true;
            //}
            //if ("user".Equals(username) && "User".Equals(roleName))
            //{
            //    return true;
            //}

            //ユーザーが所属するRoleNameが存在するかチェック
            //rolesに入力したユーザーのRoleとして返ってきた値が存在するかチェック
            string[] roles = this.GetRolesForUser(username);
            return roles.Contains(roleName);
            //return false;
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