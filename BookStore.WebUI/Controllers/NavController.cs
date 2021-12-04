using BookStore.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.WebUI.Controllers
{
    //контроллер визуализирует навигационное меню
    //затем посредством вспомагательного метода
    //Html.Action() выводит из этого метода
    //встраивается в компоновку.(cm _Layout.cshtml)
    public class NavController : Controller
    {
        private IBookRepository repository;

        public NavController(IBookRepository repo)
        {
            repository = repo;
        }

        public PartialViewResult Menu(string genre = null)
        {
            //ViewBag это динамический объект и его новые св=ва
            //можно создавать, просто устанавливая для них значения
            ViewBag.SelectedGenre = genre;

            IEnumerable<string> genres = repository.Books
                .Select(book => book.Genre)
                .Distinct()
                .OrderBy(x => x);

            return PartialView(genres);
        }
    }
}