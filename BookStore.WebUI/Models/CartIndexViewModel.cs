using BookStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.WebUI.Models
{
    //отображение корзины
    //необходимо передать информацию об объекте Cart и URL для отображения
    //когда пользователь щелкает на кнопку "Продолжить покупку"
    //для этого создадим простой класс модели представления
    public class CartIndexViewModel
    {
        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; }
    }
}