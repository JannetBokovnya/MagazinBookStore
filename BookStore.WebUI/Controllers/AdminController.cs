using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        IBookRepository repository;
        // GET: Admin
        //в конструкторе объявляем зависимость от интерфейса IBookRepository
        //которую Ninject распознает при создании экземпляров
        public AdminController(IBookRepository repo)
        {
            repository = repo;
        }


        public ViewResult Index()
        {
            return View(repository.Books);
        }
        //этот метод ищет товар с идентификатором bookId
        //и передает его как объект модели представления методу View()
        public ViewResult Edit(int bookId)
        {
            Book book = repository.Books.FirstOrDefault(b => b.BookId == bookId);
            return View(book);
        }

        [HttpPost]
        public ActionResult Edit(Book book, HttpPostedFileBase image=null)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    book.ImageMimeType = image.ContentType;
                    book.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(book.ImageData, 0, image.ContentLength);
                }


                repository.SaveBook(book);
                TempData["message"] = string.Format("Изменение информации о книге \"{0}\" были сохранены", book.Name);
                return RedirectToAction("Index");
            }
            else
            {
                //что то не так
                return View(book);
            }
            //В нашей ситуации нельзя применять ViewBag, поскольку пользователь находится
            //в состоянии перенаправления
            //Объект ViewBag передает данные между контроллером и представленим,
            //и он не может удерживать данные дольше, чем длится HTTP запрос
            //Мы могли бы воспользоваться средством данных сеанса, но тогда сообщение хранилось бы вплоть до 
            //его явного удаления, что мы предпочитаем не делать
            //таким образом объект TempData подходит лучше.
            //Данные ограничиваются сеансом одного пользователя(пользователи не видят объекты)\
            //TempData друг друга и хранятся достаточно долго, что бы быть прочитанными
        }

        public ViewResult Create()
        {
            return View("Edit", new Book());
        }


        //Этот метод действия должен поддерживать только запросы POST, 
        //поскольку удаление объектов является изменяющей операцией. 
        //Браузеры и кеши вольны выдавать запросы GET без явного согласия 
        //пользователя, поэтому следует проявлять осторожность, 
        //чтобы избежать внесения изменений в результате обработки запросов GET. 
        [HttpPost]
        public ActionResult Delete(int bookId)
        {
            Book deletedBook = repository.DeleteBook(bookId);
            if(deletedBook != null)
            {
                TempData["message"] = string.Format("Книга \"{0}\"была удалена", deletedBook.Name);
            }
            return RedirectToAction("Index");
        }
    }
}