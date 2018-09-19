namespace TodoApp.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TodoApp.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<TodoApp.Models.TodoesContext>
    {
        public Configuration()
        {

            //勝手にできたやつ (パッケージマネジャーコンソールでDB構築)
            AutomaticMigrationsEnabled = true;
            //データが失われる変更(列とか)を自動的に反映していいか？
            //今回は自動マイグレーションしたいのでtrue
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "TodoApp.Models.TodoesContext";
        }

        //マイグレーション実行後に自動で実行される処理
        protected override void Seed(TodoApp.Models.TodoesContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            //初期データ登録処理
            User admin = new User()
            {
                Id = 1 ,
                UserName = "admin",
                Password="password",
                Roles= new List<Role>()
            };
            //一回できたのでもういらない
            //User alu = new User()
            //{
            //    Id = 2 ,
            //    UserName = "alu",
            //    Password = "password",
            //    Roles = new List<Role>()
            //};
            Role administrators = new Role()
            {
                Id = 1 ,
                RoleName = "Administrators",
                Users = new List<User>()
            };
            Role users = new Role()
            {
                Id = 2,
                RoleName = "users",
                Users = new List<User>()
            };

            //adminのパスワードハッシュ化
            var membershipProvider = new CustomMembershipProvider();
            admin.Password = membershipProvider.GeneratePasswordHash(admin.UserName, admin.Password);

            admin.Roles.Add(administrators);
            administrators.Users.Add(admin);
            //alu.Roles.Add(users);
            //users.Users.Add(alu);

            //ユーザとロールをDBに反映
            //Seedクラスは引数にコンテキストクラスを持っている
            //コンテキストにユーザとロールを反映。
            //ddOrUpdate→IDなければ追加、あれば更新。
            //context.Users.AddOrUpdate(user => user.Id , new User[] { admin, alu });
            context.Users.AddOrUpdate(user => user.Id, new User[] { admin });
            context.Roles.AddOrUpdate(role => role.Id , new Role[] { administrators, users });

        }
    }
}
