using BookStore.WebUI.Infrastructure.Abstract;
using BookStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.WebUI.Controllers
{
    public class AccountController : Controller
    {
        IAuthProvider authProvider;
       public AccountController(IAuthProvider auth)
        {
            authProvider = auth;
        }

        //В действительности мы создадим две версии метода Login(). 
        //Первая версия будет визуализировать представление, которое 
        //содержит запрос на вход, а вторая - обрабатывать запрос POST, 
        //когда пользователь отправит свои учетные данные.
        public ViewResult Login()
        {
            return View();
        }



        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (authProvider.Authenticate(model.UserName, model.Password))
                {
                    return Redirect(returnUrl ?? Url.Action("Index", "Admin"));
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин или пароль");
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
    }
}