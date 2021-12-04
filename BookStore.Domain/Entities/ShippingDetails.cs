using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Entities
{
    public class ShippingDetails
    {
        //В классе используются атрибуты проверки достоверности
        //из пространства имен System.ComponentModel.DataAnnotations
        [Required(ErrorMessage = "Укажите как вас зовут")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Вставьте первый адрес доставки")]
        [Display(Name = "Первый адрес")]
        public string Line1 { get; set; }

        [Display(Name = "Второй адрес")]
        public string Line2 { get; set; }

        [Display(Name = "Третий адрес")]
        public string Line3 { get; set; }

        [Required(ErrorMessage = "Укажите город")]
        [Display(Name = "Город")]
        public string City { get; set; }

        [Required(ErrorMessage = "Укажите страну")]
        [Display(Name = "Страна")]
        public string Country { get; set; }

        public bool GiftWrap { get; set; }

    }
}

//В приложении нам необходим компонент, которому можно было бы поручить обработку информации 
//о заказе.Соблюдая принципы модели MVC, мы должны определить для этой функциональности интерфейс, 
//написать его реализацию и затем связать их с помощью контейнера внедрения зависимости(Ninject).
//интерфейс по имени IOrderProcessor в папку Abstract
