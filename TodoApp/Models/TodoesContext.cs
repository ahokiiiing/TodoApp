using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TodoApp.Models
{
    public class TodoesContext : DbContext
    //コンテキスト：TodoクラスとDBをつなぐ
    //データの取得・更新に使う
    //コンテキストクラスはDBコンテキストクラスを継承する必要がある。
    //using System.Data.Entity;追加
    {
        // DBから取得したTodoを格納するDBセットを用意
        //このプロパティからデータセットの取得・更新を行う。
        public DbSet<Todo> Todoes { get; set; }

    }
}