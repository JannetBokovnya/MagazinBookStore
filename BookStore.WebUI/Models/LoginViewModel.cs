using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BookStore.WebUI.Models
{
    public class LoginViewModel
    {
        //Атрибуты Required, примененные к свойствам модели представления, 
        //приводят к использованию проверки достоверности на стороне клиента. 
        //требуемые для этого JavaScript-библиотеки были включены в компоновки 
        //_AdminLayout.cshtml.) Пользователи могут отправлять форму только после того, 
        //как предоставят имя пользователя и пароль, а аутентификация производится 
        //на сервере при вызове метода FormsAuthentication.Authenticate().
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}