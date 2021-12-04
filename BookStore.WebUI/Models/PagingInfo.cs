using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.WebUI.Models
{
    public class PagingInfo
    {
        //общее кл-во книг
        public int TotalItems { get; set; }
        //коол во книг на странице
        public int ItemsPerPage { get; set; }
        //номер текущей страницы
        public int CurrentPage { get; set; }
        //общее кол-во страниц
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); }
        }
    }
}