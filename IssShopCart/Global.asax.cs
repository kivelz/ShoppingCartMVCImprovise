﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using IssShopCart.Models;

namespace IssShopCart
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AuthenticateRequest()
        {
            if(User == null) { return;}

            string username = Context.User.Identity.Name;

            using (ShopModelContainer db = new ShopModelContainer())
            {
                User dto = db.Users.FirstOrDefault(x => x.UserName == username);
            }
            IIdentity userIdentity = new GenericIdentity(username);
         
        }
    }
}
