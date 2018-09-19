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

            //����ɂł������ (�p�b�P�[�W�}�l�W���[�R���\�[����DB�\�z)
            AutomaticMigrationsEnabled = true;
            //�f�[�^��������ύX(��Ƃ�)�������I�ɔ��f���Ă������H
            //����͎����}�C�O���[�V�����������̂�true
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "TodoApp.Models.TodoesContext";
        }

        //�}�C�O���[�V�������s��Ɏ����Ŏ��s����鏈��
        protected override void Seed(TodoApp.Models.TodoesContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            //�����f�[�^�o�^����
            User admin = new User()
            {
                Id = 1 ,
                UserName = "admin",
                Password="password",
                Roles= new List<Role>()
            };
            //���ł����̂ł�������Ȃ�
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

            //admin�̃p�X���[�h�n�b�V����
            var membershipProvider = new CustomMembershipProvider();
            admin.Password = membershipProvider.GeneratePasswordHash(admin.UserName, admin.Password);

            admin.Roles.Add(administrators);
            administrators.Users.Add(admin);
            //alu.Roles.Add(users);
            //users.Users.Add(alu);

            //���[�U�ƃ��[����DB�ɔ��f
            //Seed�N���X�͈����ɃR���e�L�X�g�N���X�������Ă���
            //�R���e�L�X�g�Ƀ��[�U�ƃ��[���𔽉f�B
            //ddOrUpdate��ID�Ȃ���Βǉ��A����΍X�V�B
            //context.Users.AddOrUpdate(user => user.Id , new User[] { admin, alu });
            context.Users.AddOrUpdate(user => user.Id, new User[] { admin });
            context.Roles.AddOrUpdate(role => role.Id , new Role[] { administrators, users });

        }
    }
}
