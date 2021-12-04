using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using BookStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IBookRepository repository;
        private IOrderProcessor orderProcessor;

        public CartController(IBookRepository repo, IOrderProcessor processor)
        {
            repository = repo;
            orderProcessor = processor;
        }
        //имея модель представления CartView... можно реализовать метод 
        //действия Index() в CartController

        //после создания связывателя CartModelBinder
        //можно модифицировать контроллер Cart избавившись от метода GetCart

            //было
        //public ViewResult Index(string returnUrl)
        //{
        //    return View(new CartIndexViewModel
        //    {
        //        Cart = GetCart(),
        //        ReturnUrl = returnUrl
        //    });
        //}

            //стало
        //переложив всю работу на созданный связыватель модели, который будет снабжать
        //контроллер объектами Cart
        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }





        //для сохранения и извлечения объектов используется метод
        //Getcart(), использующий средство состояния
        //сеанса ASP.NET
        //инфраструкткра ASP.NET поддкрживает удобное средство сеансов,
        //которое использует cookie-наборы или переписывание URL
        //что бы фссоциировать вместо множество запросов от
        //определенного пользователя с целью формирования отдельного
        //сеанса просмотра
        //С данным средством связано состояние сеансаБ позволяющее
        //ассоциировать данные с сеансом
        //Нам нужно, что бы каждый пользователь имел собственную корзинуБ и эта корзина 
        //сохранялась между запросами
        //Данные, связанные с сеансом удаляются по истечении времени
        //существования сеанса что обычно происходит , если пользователь долго 
        //не выдает запроса в течении заданного переода
        //по умолчанию объекты состояния сеанса храняться в памяти сервера ASP.NET
        //public Cart GetCart()
        //{
        //    Cart cart = (Cart)Session["Cart"];
        //    if (cart == null)
        //    {
        //        cart = new Cart();
        //        Session["Cart"] = cart;
        //    }
        //    return cart;
        //}



        public RedirectToRouteResult AddToCart(Cart cart, int bookId, string returnUrl)
        {
            Book book = repository.Books
                .FirstOrDefault(b => b.BookId == bookId);
            if(book != null)
            {
                cart.AddItem(book, 1);
            }
            return RedirectToAction("Index", new { returnUrl });

        }
        //методы AddToCart и RemoveFromCart вызывают метод RedirectToFction
        //В результате этого клиентскому браузеру отправляется инструкция
        //перенаправления HTTP заставляя браузер запросить новый URL
        //В этом случае браузер запросит URL, который вызывает метод Index
        //контроллера Cart
        public RedirectToRouteResult RemoveFromCart(Cart cart, int bookId, string returnUrl)
        {
            Book book = repository.Books
                .FirstOrDefault(b => b.BookId == bookId);
            if (book != null)
            {
                cart.RemoveLine(book);
            }
            return RedirectToAction("Index", new { returnUrl });

        }

        //мы создадим виджет (графический элемент) с итоговой информацией по содержимому корзины
        //Этот простой метод должен визуализировать представление, передавая в качестве 
        //данных представления текущий объект Cart(который будет получен с 
        //использованием специального связывателя модели)

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        //кнопка оформить заказ
        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }


        //метод действия Checkout() декорирован атрибутом HttpPost, 
        //а это значит что он будет вызываться для запроса POST - в данном случае, 
        //когда пользователь отправляет форму.
        //Мы снова полагаемся на систему привязки моделей для параметра ShippingDetails 
        //(создаваемого автоматически с применением данных формы HTTP) и параметра 
        //Cart(который создается с использованием специального связывателя).

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                //Для проверки наличия проблем предназначено свойство ModelState.IsValid.
                ModelState.AddModelError("", "Извините, ваша корзина пуста!");
            }

            if (ModelState.IsValid)
            {
                orderProcessor.ProcessorOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(shippingDetails);
            }
        }


    }
}