using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TodoApp.Models
{
    public class Todo 
        //クラス名は単数形の名詞→テーブル名はこれが複数形されてテーブルで使われる
        //コードファーストではテーブルが生成される
        //プロパティ名→テーブルの列名
        //IDは自動的に主キーになる

    {
        public int Id { get; set; }
        //TodoクラスのプロパティにDisplayNameを設定
        //一覧のヘッダー部分を日本語に変更
        //ディスプレイアノテーションを設定
        [DisplayName("概要")]
        public　string Summary { get; set; }

        [DisplayName("詳細")]
        public string Detail { get; set; }

        [DisplayName("期限")]
        public DateTime Limit { get; set; }

        [DisplayName("完了")]
        public bool Done { get; set; }

        //Todoを追加したユーザを保持するためプロパティを追加
        //TodoとUserをひもづけ
        public virtual User User { get; set; }
    }

}