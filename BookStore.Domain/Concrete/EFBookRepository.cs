using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Domain.Entities;
using BookStore.Domain.Abstract;

namespace BookStore.Domain.Concrete
{
    public class EFBookRepository : IBookRepository
    {
        EFDbContext context = new EFDbContext();
        public IEnumerable<Book> Books
        {
            get { return context.Books; }
        }

        public Book DeleteBook(int bookId)
        {
            Book dbEntry = context.Books.Find(bookId);
            if(dbEntry != null)
            {
                context.Books.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }



        //Реализация метода Save() добавляет товар в хранилище, если значение BookId равно 0; 
        //в противном случае применяются изменения к существующей записи в базе данных.
        //Нам известно, что обновление должно выполняться, когда получен параметр Book, который имеет ненулевое 
        //значение BookId. Это делается путем извлечения из хранилища объекта Book с тем же самым 
        //значением BookID и обновлением всех его свойств согласно объекту, переданному в качестве параметра.
        //Это предпринимается потому, что инфраструктура Entity Framework отслеживает объекты, 
        //которые создает из базы данных. Объект, переданный методу SaveChanges(), создается инфраструктурой
        //MVC Framework с использованием стандартного связывателя модели, а это значит, что Entity Framework
        //ничего не известно об объекте параметра, и обновление не будет применено к базе данных. 
        //Существует множество способов решения указанной проблемы, но мы принимаем простейший из них, 
        //предполагающий поиск соответствующего объекта, о котором известно Entity Framework, и его явное обновление.
        public void SaveBook(Book book)
        {
            if (book.BookId == 0)
            {
                context.Books.Add(book);
            }
            else
            {
                Book dbEntry = context.Books.Find(book.BookId);
                if (dbEntry != null)
                {
                    dbEntry.Name = book.Name;
                    dbEntry.Author = book.Author;
                    dbEntry.Description = book.Description;
                    dbEntry.Genre = book.Genre;
                    dbEntry.Price = book.Price;
                    dbEntry.ImageData = book.ImageData;
                    dbEntry.ImageMimeType = book.ImageMimeType;
                }
            }
            context.SaveChanges();
        }
    }
}
