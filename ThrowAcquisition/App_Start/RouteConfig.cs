using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ThrowAcquisition
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // remove default route
            //routes.IgnoreRoute("");

            routes.MapRoute(
                name: "BR",
                url: "br/{id}",
                defaults: new { controller = "Home", action = "BR", id = UrlParameter.Optional }
                //defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Test",
                url: "Test",
                defaults: new { controller = "Home", action = "test", id = UrlParameter.Optional }
                //defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Disattivazione",
                url: "Disattivazione",
                defaults: new { controller = "Home", action = "Disattivazione", id = UrlParameter.Optional }
                //defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "DBR",
                url: "DBR",
                defaults: new { controller = "Home", action = "DBR", id = UrlParameter.Optional }
                //defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "OTPBR",
                url: "OTPBR",
                defaults: new { controller = "Home", action = "OTPBR", id = UrlParameter.Optional }
                //defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "tuttomobile",
                url: "giochi",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                //defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "tuttomobile_disattivazione",
                url: "giochi/disattivazione",
                defaults: new { controller = "Home", action = "Disattivazione", id = UrlParameter.Optional }
                //defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                //defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
