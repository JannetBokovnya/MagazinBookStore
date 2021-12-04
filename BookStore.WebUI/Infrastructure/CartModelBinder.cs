using BookStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.WebUI.Infrastructure
{
    //создаем связыватель модели, который будет получать объект Cart, 
    //содержащийся внутри данных сеанса. 
    //В результате у инфраструктуры MVC Framework появится возможность создавать 
    //объекты Cart и передавать их в виде параметров методам действий класса CartController
    public class CartModelBinder : IModelBinder
    {

        private const string sessionKey = "Cart";
        //этот метод принимает 2 параметра, что бы обеспечить
        //возможность создания объекта модели предметной области

        //параметр ControllerContext обеспечивает доступ ко всей информации
        //которой располагает класс контроллера, включая детали запроса

        //параметр ModelBindingContext предоставляет сведения об объекте модели
        //который требуется создать, а так же набор инструментов для
        //упрощения процесса привязки

        //класс ControllerContext имеет св-во HttpContext, которое, в свою
        //очередь содержит сво-во Session, позволяющее получать и устанавливать данные сеанса

        // Объект Cart получается за счет чтения значения для ключа
        //из данных сеанса



        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Cart cart = null;
            if (controllerContext.HttpContext.Session != null)
            {
                cart = (Cart)controllerContext.HttpContext.Session[sessionKey];
            }

            if (cart == null)
            {
                cart = new Cart();
                if(controllerContext.HttpContext.Session != null)
                {
                    controllerContext.HttpContext.Session[sessionKey] = cart;
                }
            }
            return cart;
            //потом переходим в Global.asax.cs
        }
    }
}