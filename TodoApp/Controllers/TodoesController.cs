using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TodoApp.Models;

//スキャフォールティング→CRDU定型的な高度の骨組みを自動的に生成

namespace TodoApp.Controllers
{
    //TodoesControllerにはログインした状態でのみアクセスできるようにAuthorizeアノテーションを設定。
    //各アクションメソッドにはサインインしているときにだけアクセスできる。
    //アクセスしようとすると、ログイン画面に戻される。
    [Authorize]
    public class TodoesController : Controller
        //コントローラークラスの名前はControllerで終わる必要がある。
        //コントローラクラスを継承する必要がある。
        //コントローラクラスを継承した独自の基底クラスを作成してもOK←？
    {
        private TodoesContext db = new TodoesContext();

        // GET: Todoes
        //BrowserからTodoesへのアクセスがあった場合に呼ばれる。(アクションメソッド)
        //クライアントから要求されたURLを基に呼び出されるコントローラ、アクションメソッド、アクションメソッドに渡されるパラメータがきまる
        //リクエストに応じて処理の受け渡し先を決定すること→ルーティング→ルートコンフィグが案内
        public ActionResult Index()
        {
            //DBから現在ログインしているユーザのオブジェクトを取得。
            var user = db.Users.Where(item => item.UserName == User.Identity.Name).FirstOrDefault();

            if (user != null)
            {
                return View(user.Todes);
            }
            //userがnullだったら空のリストを返す
            return View(new LinkedList<Todo>());
            
        }

        // GET: Todoes/Details/5
        public ActionResult Details(int? id)
            //int？→nullアプル→nullOK
        {
            if (id == null)
            {
                //IDがnullButRequestを返して終了(400)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                
            }
            Todo todo = db.Todoes.Find(id);
            if (todo == null)
            {
                //DBにデータがない場合はNotFoundを返す
                return HttpNotFound();
            }
            //一致するデータがあればViewにデータをセットしてかえす
            return View(todo);
        }

        // GET: Todoes/Create
        public ActionResult Create()
        {
            //呼ばれただけだと、画面を表示するだけ
            return View();
        }

        // POST: Todoes/Create
        //入力画面でPOSTされる
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Bind→PostされたデータをTodoモデルに紐づける
        public ActionResult Create([Bind(Include = "Id,Summary,Detail,Limit,Done")] Todo todo)
        {
            //入力内容が適切かどうか返す
            if (ModelState.IsValid)
            {
                var user = db.Users.Where(item => item.UserName == User.Identity.Name).FirstOrDefault();

                if (user != null)
                {
                    todo.User = user;
                    //OK→登録処理
                    //DBセットに登録
                    db.Todoes.Add(todo);
                    //DBセットの内容をDBへ反映
                    db.SaveChanges();
                    //index(アクションメソッド)に処理を転送
                    //ここでは処理が完了すると一覧画面に戻る。
                    return RedirectToAction("Index");
                }
            }
            //NG→入力内容をCreate.schtmlに返す
            return View(todo);
        }

        // GET: Todoes/Edit/5
        public ActionResult Edit(int? id)
        {
            //指定されたIDのTodoを取得して返す
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Todo todo = db.Todoes.Find(id);
            if (todo == null)
            {
                return HttpNotFound();
            }

            return View(todo);
        }

        // POST: Todoes/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Summary,Detail,Limit,Done")] Todo todo)
        {
            if (ModelState.IsValid)
            {
                //該当のTodoを更新
                db.Entry(todo).State = EntityState.Modified;
                //DBに反映
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(todo);
        }

        // GET: Todoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Todo todo = db.Todoes.Find(id);
            if (todo == null)
            {
                return HttpNotFound();
            }
            return View(todo);
        }

        // POST: Todoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Todo todo = db.Todoes.Find(id);
            db.Todoes.Remove(todo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //終了の処理
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //処理しているコンテキストを開放
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
