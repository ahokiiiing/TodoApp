using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;

namespace TodoApp.Models
{
    public class CustomMembershipProvider : MembershipProvider
    {
        public override bool EnablePasswordRetrieval => throw new NotImplementedException();

        public override bool EnablePasswordReset => throw new NotImplementedException();

        public override bool RequiresQuestionAndAnswer => throw new NotImplementedException();

        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override int MaxInvalidPasswordAttempts => throw new NotImplementedException();

        public override int PasswordAttemptWindow => throw new NotImplementedException();

        public override bool RequiresUniqueEmail => throw new NotImplementedException();

        public override MembershipPasswordFormat PasswordFormat => throw new NotImplementedException();

        public override int MinRequiredPasswordLength => throw new NotImplementedException();

        public override int MinRequiredNonAlphanumericCharacters => throw new NotImplementedException();

        public override string PasswordStrengthRegularExpression => throw new NotImplementedException();

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }


        //今回はこれを使う
        //固定のusernameとpasswordを使うように実装する
        //認証機能の動作確認から
        public override bool ValidateUser(string username, string password)
        {
            //if ("administrator".Equals(username) && "password".Equals(password))
            //{
            //    return true;
            //}
            //if ("user".Equals(username) && "password".Equals(password))
            //{
            //    return true;
            //using構文について
            //ブロック内でだけ変数が有効になる。
            //終わりにコンテキストのディスポーズメソッドが呼ばれ、
            //終了処理が自動的に実行されるのでクローズ処理を忘れなくなる。
            using (var db = new TodoesContext())
            {
                //ハッシュ値を取得する
                string hash = this.GeneratePasswordHash(username, password);

                //FirstOrDefault→複数検索結果があるとき一番上のを使う。
                //検索結果がないとき、nullを返す(今回はこっちの用途)
                var user = db.Users
                    .Where(u => u.UserName == username && u.Password == hash)
                    .FirstOrDefault();
                if (user != null)
                {
                    return true;
                }
            }
            ////パスワードハッシュ化によってだれもログインできなくなるのを防ぐ。あとで消す。
            //if ("admin".Equals(username) && "password".Equals(password))
            //{
            //    return true;
            //}
            return false;
        }
        //ユーザー名とパスワードを引数にハッシュ化されたパスワードを返すメソッド
        public string GeneratePasswordHash(string username, string password)
        {
            string rawSalt = $"secret_{username}";
            var sha256 = new SHA256CryptoServiceProvider();
            //ComputeHashの引数に肩に合うようバイトの配列に変換
            var salt = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(rawSalt));
            //passwordをハッシュ化
            //インスタンス化　ストレッチング10000回
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            var hash = pbkdf2.GetBytes(32);

            return Convert.ToBase64String(hash);
        }
    }
}