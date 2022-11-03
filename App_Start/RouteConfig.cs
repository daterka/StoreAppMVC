using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace StoreApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
                name: "Products",
                url: "products",
                defaults: new { controller = "Product", action = "GetAllProducts", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Remove product",
                url: "products/remove/{id}",
                defaults: new { controller = "Product", action = "RemoveProduct", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
