using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using BookStore.WebUI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.WebUI.Controllers;
using System.Web.Mvc;

namespace BookStore.UnitTests
{
    //нас интересует поведение метода действия
    //Index() в контроллере Admin
    //которое заключается в корректном возврате объектов Book
    //находящихся в хранилище
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Books()
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
            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие
            List<Book> result = ((IEnumerable<Book>)controller.Index().
                ViewData.Model).ToList();


            // Утверждение
            Assert.AreEqual(result.Count(),5);
            //проверяем отдельные значения
            Assert.AreEqual(result[0].Name, "Book1");
            Assert.AreEqual(result[1].Name, "Book2");
        }

        [TestMethod]
        public void Can_Edit_Book()
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
            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие
            Book book1 = controller.Edit(1).ViewData.Model as Book;
            Book book2 = controller.Edit(2).ViewData.Model as Book;
            Book book3 = controller.Edit(3).ViewData.Model as Book;


            // Утверждение
            Assert.AreEqual(1, book1.BookId);
            Assert.AreEqual(2, book2.BookId);
            Assert.AreEqual(3, book3.BookId);

        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Book()
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
            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие
            Book result = controller.Edit(7).ViewData.Model as Book;

            // Утверждение
            Assert.IsNull(result);
           

        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IBookRepository> mock = new Mock<IBookRepository>();

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Организация - создание объекта Game
            Book book = new Book { Name = "Test" };

            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(book);

            // Утверждение - проверка того, что к хранилищу производится обращение
            mock.Verify(m => m.SaveBook(book));

            // Утверждение - проверка типа результата метода
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IBookRepository> mock = new Mock<IBookRepository>();

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Организация - создание объекта Game
            Book book = new Book { Name = "Test" };

            // Организация - добавление ошибки в состояние модели
            controller.ModelState.AddModelError("error", "error");

            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(book);

            // Утверждение - проверка того, что обращение к хранилищу НЕ производится 
            mock.Verify(m => m.SaveBook(It.IsAny<Book>()), Times.Never());

            // Утверждение - проверка типа результата метода
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Delete_Valid_Books()
        {
            // Организация - создание объекта Book
            Book book = new Book {BookId = 2, Name = "Book2" };

            // Организация (arrange) создание имитированного хранилища данных
          
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { BookId = 1, Name = "Book1" },
                new Book { BookId = 2, Name = "Book2" },
                new Book { BookId = 3, Name = "Book3" },
                new Book { BookId = 4, Name = "Book4" },
                new Book { BookId = 5, Name = "Book5" }

                });
            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие - удаление книги
                controller.Delete(book.BookId);


            // Утверждение - проверка того, что метод удаления в хранилище
            // вызывается для корректного объекта Game
            mock.Verify(m => m.DeleteBook(book.BookId));
        }

    }
}
