using BookStore.Domain.Entities;
using BookStore.WebUI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BookStore.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //сообщаем что она может использовать класс
            //CartModelBinder для создания экземпляров Cart

            ModelBinders.Binders.Add(typeof(Cart), new CartModelBinder());
        }
    }
}
