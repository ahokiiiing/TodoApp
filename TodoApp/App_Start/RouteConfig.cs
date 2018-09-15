using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TodoApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //ルーティングの仕方が書いてある＝呼びだすControllerとアクションメソッドを決める。
            routes.MapRoute(
                name: "Default",
                //リクエストされたURL　例：http://localhost:8080/Todoes/Detail/3
                //Controllerとactionが空の場合　http://localhost:8080/　TodoControllerのindexメソッドのリクエストとして処理される
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Todoes", action = "Index", id = UrlParameter.Optional }
                //最初に表示するページ　defaults
            );
        }
    }
}
