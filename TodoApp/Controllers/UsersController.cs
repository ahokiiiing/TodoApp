using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TodoApp.Models;

namespace TodoApp.Controllers
{
    //Adminだけアクセス可能とする。
    //ユーザー管理画面からロールが選択できるようにしていく。
    //ユーザーコントロールからDBからロールを取得してリストボックスに表示できる形式に加工。
    [Authorize(Roles ="Administrators")]
    public class UsersController : Controller
    {
        private TodoesContext db = new TodoesContext();
        //インスタンスを保持
        readonly private CustomMembershipProvider membershipProvider = new CustomMembershipProvider();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            //空のリスト
            this.SetRoles(new List<Role>());
            return View();
        }

        // POST: Users/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserName,Password,RoleIds")] User user)
        {
            //ロールIDがDBのIDに存在すれば取得してリストとして返す
            var roles = db.Roles.Where(role => user.RoleIds.Contains(role.Id)).ToList();

            if (ModelState.IsValid)
            {
                //取得したロールをセット
                user.Roles = roles;

                user.Password = this.membershipProvider.GeneratePasswordHash(user.UserName, user.Password);

                db.Users.Add(user); //ユーザに追加
                db.SaveChanges();　　　//DBに反映
                return RedirectToAction("Index");
            }
            //False→入力内容に問題がある
            //入力内容を復元のままクリエイト
            this.SetRoles(roles);
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            //ユーザーを選択状態にする
            this.SetRoles(user.Roles);
            return View(user);
        }

        // POST: Users/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserName,Password,RoleIds")] User user)
        {
            var roles = db.Roles.Where(role => user.RoleIds.Contains(role.Id)).ToList();
            if (ModelState.IsValid)
            {
                var dbUser = db.Users.Find(user.Id);
                if (dbUser == null)
                {
                    return HttpNotFound();
                }

                dbUser.UserName = user.UserName;
                
                //パスワード変更が編集画面でパスワードが変更されていないかも
                if (!dbUser.Password.Equals(user.Password))
                {
                    dbUser.Password = this.membershipProvider.GeneratePasswordHash(user.UserName, user.Password);
                }

                //パスワードハッシュ化につき、元の処理削除
                //dbUser.Password = user.Password;
                dbUser.Roles.Clear();
                foreach(var role in roles)
                {
                    dbUser.Roles.Add(role);
                }

                db.Entry(dbUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //入力に不備があった場合
            this.SetRoles(roles);
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        //クリエイト、エディットの両方で使う共通の処理→Private？
        private void SetRoles(ICollection<Role> userRoles)
        {
            //intのID配列のほうが扱いやすい。
            var roles = userRoles.Select(item => item.Id).ToArray();

            var list = db.Roles.Select(item => new SelectListItem()
            {
                Text = item.RoleName,
                Value = item.Id.ToString(),
                //選択状態にするselected
                //Rolesの配列にRoleIDが含まれればTrue
                Selected = roles.Contains(item.Id)
            }).ToList();
            ViewBag.RoleIds = list;
        }
    }
}
