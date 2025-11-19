using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;

namespace WebUsuarios
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var settings = new FriendlyUrlSettings();
            //settings.AutoRedirectMode= RedirectMode.Off;
            routes.Ignore("{resource}.axd/{*pathInfo}");
            routes.EnableFriendlyUrls(settings);
        }
    }
}
