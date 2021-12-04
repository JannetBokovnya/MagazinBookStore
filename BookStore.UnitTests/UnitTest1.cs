using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using BookStore.WebUI.Controllers;
using BookStore.WebUI.Models;
using BookStore.WebUI.HtmlHelpers;

namespace BookStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // Организация (arrange)
            //имитация хранилища
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { BookId = 1, Name = "Book1" },
                new Book { BookId = 2, Name = "Book2" },
                new Book { BookId = 3, Name = "Book3" },
                new Book { BookId = 4, Name = "Book4" },
                new Book { BookId = 5, Name = "Book5" }

                });
            BookController controller = new BookController(mock.Object);
            controller.pageSize = 3;//количество элементов на странице

            //Действие(act)
            //обращаемся к свойству Model объекта результата,
            //что бы получить последовательности IEnumerable<Book>
            //второй страницы
            // IEnumerable<Book> result = (IEnumerable<Book>)controller.List(2).Model;
            BooksListViewModel result = (BooksListViewModel)controller.List(null,2).Model;

            //проверяем количество элементов на второй странице
            //утверждение(assert)
            //List<Book> books = result.ToList();
            //Assert.IsTrue(books.Count == 2);
            List<Book> books = result.Books.ToList();
            Assert.IsTrue(books.Count == 2);




            //проверяем отдельные значения
            Assert.AreEqual(books[0].Name, "Book4");
            Assert.AreEqual(books[1].Name, "Book5");
        }
        //тестирование вспомагательного метода PageLinks
        //мы вызываем его с тестовыми данными и сравниваем результаты
        //с ожидаемой HTML разметкой
        [TestMethod]
        public void Can_generate_Page_Links()
        {
            // Организация - определение вспомогательного метода HTML - это необходимо
            // для применения расширяющего метода
            HtmlHelper myHelper = null;

            // Организация - создание объекта PagingInfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };
            // Организация - настройка делегата с помощью лямбда-выражения
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Действие
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Утверждение
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());

        }
        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Организация (arrange)
            //имитация хранилища
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { BookId = 1, Name = "Book1" },
                new Book { BookId = 2, Name = "Book2" },
                new Book { BookId = 3, Name = "Book3" },
                new Book { BookId = 4, Name = "Book4" },
                new Book { BookId = 5, Name = "Book5" }

                });
            BookController controller = new BookController(mock.Object);
            controller.pageSize = 3;//количество элементов на странице

            BooksListViewModel result = (BooksListViewModel)controller.List(null,2).Model;
            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);


        }

        [TestMethod]
        public void Can_Filter_Books()
        {
            // Организация (arrange)
            //имитация хранилища
            //Mock<IBookRepository> mock = new Mock<IBookRepository>();
            //mock.Setup(m => m.Books).Returns(new List<Book>
            //{
            //    new Book { BookId = 1, Name = "Book1", Genre="Genre1" },
            //    new Book { BookId = 2, Name = "Book2", Genre="Genre2" },
            //    new Book { BookId = 3, Name = "Book3", Genre="Genre1" },
            //    new Book { BookId = 4, Name = "Book4", Genre="Genre3" },
            //    new Book { BookId = 5, Name = "Book5", Genre="Genre2" }

            //    });
            //BookController controller = new BookController(mock.Object);
            //controller.pageSize = 3;//количество элементов на странице

            //List<Book> result = ((BooksListViewModel)controller.List("Genre2", 2).Model).Books.ToList();
            //// Assert

            //Assert.AreEqual(result.Count(), 2);
            //Assert.IsTrue(result[0].Name == "Book2" && result[0].Genre == "Genre2");
            //Assert.IsTrue(result[0].Name == "Book5" && result[0].Genre == "Genre2");
        }

        //модульный тест. Создание списка, отсортированного в 
        //алфавитном порядке и не содержащего дубликатов
        [TestMethod]
        public void Can_Create_Categories()
        {
            // Организация (arrange)
            //имитация хранилища
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { BookId = 1, Name = "Book1", Genre="Genre1" },
                new Book { BookId = 2, Name = "Book2", Genre="Genre2" },
                new Book { BookId = 3, Name = "Book3", Genre="Genre1" },
                new Book { BookId = 4, Name = "Book4", Genre="Genre3" },
                new Book { BookId = 5, Name = "Book5", Genre="Genre2" }

                });
            NavController controller = new NavController(mock.Object);


            List<string> result = ((IEnumerable<string>)controller.Menu().Model).ToList();
            // Assert

            Assert.AreEqual(result.Count(), 3);
            Assert.AreEqual(result[0], "Genre1");
            Assert.AreEqual(result[1], "Genre2");
            Assert.AreEqual(result[2], "Genre3");

        }
        //проверка с viewbag. selected
        [TestMethod]
        public void Indicates_Selected_Genre()
        {
            // Организация (arrange)
            //имитация хранилища
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { BookId = 1, Name = "Book1", Genre="Genre1" },
                new Book { BookId = 2, Name = "Book2", Genre="Genre2" },
                new Book { BookId = 3, Name = "Book3", Genre="Genre1" },
                new Book { BookId = 4, Name = "Book4", Genre="Genre3" },
                new Book { BookId = 5, Name = "Book5", Genre="Genre2" }

                });
            NavController controller = new NavController(mock.Object);


            string genreToSelect = "Genre2";
            // Assert
            string result = controller.Menu(genreToSelect).ViewBag.SelectedGenre;
            Assert.AreEqual(genreToSelect, result);
 
        }

        //возможность генерации корректных счетчиков товаров
        //для различных категорий
        [TestMethod]
        public void Generate_Genre_Specific_Book_Count()
        {
            // Организация (arrange)
            //имитация хранилища
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { BookId = 1, Name = "Book1", Genre="Genre1" },
                new Book { BookId = 2, Name = "Book2", Genre="Genre2" },
                new Book { BookId = 3, Name = "Book3", Genre="Genre1" },
                new Book { BookId = 4, Name = "Book4", Genre="Genre3" },
                new Book { BookId = 5, Name = "Book5", Genre="Genre2" }

                });
            BookController controller = new BookController(mock.Object);
            controller.pageSize = 3;//количество элементов на странице

            int res1 = ((BooksListViewModel)controller.List("Genre1").Model).PagingInfo.TotalItems;
            int res2 = ((BooksListViewModel)controller.List("Genre2").Model).PagingInfo.TotalItems;
            int res3 = ((BooksListViewModel)controller.List("Genre3").Model).PagingInfo.TotalItems;
            int resAll = ((BooksListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }
        //тестирование пользовательской корзины
        [TestClass]
        public class CartTests
        {
            //добавление в корзину
            [TestMethod]
            public void Can_Add_New_Lines()
            {
                // Организация - создание нескольких тестовых игр
                Book book1 = new Book { BookId = 1, Name = "Book1" };
                Book book2 = new Book { BookId = 2, Name = "Book2" };

                // Организация - создание корзины
                Cart cart = new Cart();
               
                // Действие
                cart.AddItem(book1, 1);
                cart.AddItem(book2, 1);
                List<CartLine> results = cart.Lines.ToList();

                // Утверждение
                Assert.AreEqual(results.Count(), 2);
                Assert.AreEqual(results[0].Book, book1);
                Assert.AreEqual(results[1].Book, book2);
            }
            //если объект добален в корзину то повторное добавление увеличение его стоимости
            [TestMethod]
            public void Can_Add_Quantity_For_Existing_Lines()
            {
                // Организация - создание нескольких тестовых игр
                Book book1 = new Book { BookId = 1, Name = "Book1" };
                Book book2 = new Book { BookId = 2, Name = "Book2" };

                // Организация - создание корзины
                Cart cart = new Cart();

                // Действие
                cart.AddItem(book1, 1);
                cart.AddItem(book2, 1);
                cart.AddItem(book1, 5);
                List<CartLine> results = cart.Lines.OrderBy(c => c.Book.BookId).ToList();

                // Утверждение
                Assert.AreEqual(results.Count(), 2);
                Assert.AreEqual(results[0].Quantity, 6);    // 6 экземпляров добавлено в корзину
                Assert.AreEqual(results[1].Quantity, 1);
            }
            //удаление из корзины
            [TestMethod]
            public void Can_Remove_Line()
            {
                // Организация - создание нескольких тестовых игр
                Book book1 = new Book { BookId = 1, Name = "Book1" };
                Book book2 = new Book { BookId = 2, Name = "Book2" };
                Book book3 = new Book { BookId = 3, Name = "Book3" };

                // Организация - создание корзины
                Cart cart = new Cart();

                // Организация - добавление нескольких игр в корзину
                cart.AddItem(book1, 1);
                cart.AddItem(book2, 4);
                cart.AddItem(book3, 2);
                cart.AddItem(book2, 1);

                // Действие
                cart.RemoveLine(book2);

                // Утверждение
                Assert.AreEqual(cart.Lines.Where(c => c.Book == book2).Count(), 0);
                Assert.AreEqual(cart.Lines.Count(), 2);
            }
            //общая стоимость
            [TestMethod]
            public void Calculate_Cart_Total()
            {
                // Организация - создание нескольких тестовых игр
                Book book1 = new Book { BookId = 1, Name = "Book1", Price = 100 };
                Book book2 = new Book { BookId = 2, Name = "Book2", Price = 55 };

                // Организация - создание корзины
                Cart cart = new Cart();

                // Действие
                cart.AddItem(book1, 1);
                cart.AddItem(book2, 1);
                cart.AddItem(book1, 5);
                decimal result = cart.ComputeTotalValue();

                // Утверждение
                Assert.AreEqual(result, 655);
            }
            //очистка корзины
            [TestMethod]
            public void Can_Clear_Contents()
            {
                // Организация - создание нескольких тестовых игр
                Book book1 = new Book { BookId = 1, Name = "Book1", Price = 100 };
                Book book2 = new Book { BookId = 2, Name = "Book2", Price = 55 };

                // Организация - создание корзины
                Cart cart = new Cart();

                // Действие
                cart.AddItem(book1, 1);
                cart.AddItem(book2, 1);
                cart.AddItem(book1, 5);
                cart.Clear();

                // Утверждение
                Assert.AreEqual(cart.Lines.Count(), 0);
            }

            //добавление выбранного товара в корзину 
            //пользователя(тестирование со связывателем) CartModelBuilder
            [TestMethod]
            public void Can_Add_To_Cart()
            {
                Mock<IBookRepository> mock = new Mock<IBookRepository>();
                mock.Setup(m => m.Books).Returns(new List<Book> {
                    new Book {BookId = 1, Name = "Book1", Genre="Genre1"},
                    }.AsQueryable());

                // Организация - создание корзины
                Cart cart = new Cart();

                // Организация - создание контроллера
                CartController controller = new CartController(mock.Object, null);

                // Действие - добавить игру в корзину
                controller.AddToCart(cart, 1, null);

                // Утверждение
                Assert.AreEqual(cart.Lines.Count(), 1);
                Assert.AreEqual(cart.Lines.ToList()[0].Book.BookId, 1);
            }

            /// <summary>
            /// После добавления игры в корзину, должно быть перенаправление на страницу корзины
            /// </summary>
            [TestMethod]
            public void Adding_Game_To_Cart_Goes_To_Cart_Screen()
            {
                // Организация - создание имитированного хранилища
                Mock<IBookRepository> mock = new Mock<IBookRepository>();
                mock.Setup(m => m.Books).Returns(new List<Book> {
                    new Book {BookId = 1, Name = "Book1", Genre="Genre1"},
                    }.AsQueryable());

                // Организация - создание корзины
                Cart cart = new Cart();

                // Организация - создание контроллера
                CartController controller = new CartController(mock.Object, null);

                // Действие - добавить игру в корзину
                RedirectToRouteResult result = controller.AddToCart(cart, 2, "myUrl");

                // Утверждение
                Assert.AreEqual(result.RouteValues["action"], "Index");
                Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
            }

            // Проверяем URL
            [TestMethod]
            public void Can_View_Cart_Contents()
            {
                // Организация - создание корзины
                Cart cart = new Cart();

                // Организация - создание контроллера
                CartController target = new CartController(null,null);

                // Действие - вызов метода действия Index()
                CartIndexViewModel result
                    = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

                // Утверждение
                Assert.AreSame(result.Cart, cart);
                Assert.AreEqual(result.ReturnUrl, "myUrl");
            }
        }

    }
}
