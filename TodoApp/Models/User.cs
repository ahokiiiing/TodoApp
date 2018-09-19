using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TodoApp.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        //ユニークの設定
        [Index(IsUnique =true)]
        //文字列の長さを設定しないとテーブル作成時にエラーになる
        [StringLength(256)]
        [DisplayName("ユーザー名")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("パスワード")]
        public string Password { get; set; }

        // ユーザーが複数のロールに所属できるようロールのコレクションを持つようにする。
        // ユーザーモデルとロールモデルの関係を表すプロパティ→ナビゲーションプロパティ
        //→virtial修飾子を付ける必要がある
        public virtual ICollection<Role> Roles { get; set; }

        //選択状態を保持するようにプロパティ追加
        //DBに保持しないマイグレーションの処理が走ってもRoleIdsの列を作らないように
        [NotMapped]
        [DisplayName("ロール")]
        public List<int> RoleIds { get; set; }

        //ユーザーに対してTodo複数→UserはTodonoコレクションを持つ
        //ナビゲーションプロパティによってTodoesテーブルにUserID(外部キー)が作成される
        public virtual ICollection<Todo> Todes { get; set; }
    }
}