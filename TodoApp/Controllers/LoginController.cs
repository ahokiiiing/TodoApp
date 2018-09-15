using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TodoApp.Models;

namespace TodoApp.Controllers
{
    //Loginしていなくてもアクセスできる
    [AllowAnonymous]
    public class LoginController : Controller
    {
        //user認証
        //ユーザー認証を行うのでCustomMembershipProviderを保持 
        //サイド代入などなし
        readonly CustomMembershipProvider membershipProvider = new CustomMembershipProvider();
        private object formsAuthentication;

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        //Postのアクションメソッドを追加
        //アノテーションセット
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include ="Username,Password")] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (this.membershipProvider.ValidateUser(model.UserName, model.Password))
                {
                    //認証後、クッキーにusernameを保持→この間は認証状態と判断される
                    FormsAuthentication.SetAuthCookie(model.UserName, false);
                    //認証処理が完了したらTodoesControllerのIndexに処理とリダイレクト
                    return RedirectToAction("Index", "Todoes");
                }
            }
            //認証失敗の場合はログイン画面へ
            ViewBag.Message = "ログイン認証に失敗しました.";
            return View(model);
        }            //Logout
        public ActionResult SignOut()
        {
            //SignOutメソッドを実行するだけで認証クッキーが削除される
            FormsAuthentication.SignOut();
            //ホーム画面に戻る
            return RedirectToAction("Index");
        }
    }
}