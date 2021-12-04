using BookStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Abstract
{
    //Реализация интерфейса IOrderProcessor будет обрабатывать заказы, 
    //отправляя их по электронной почте администратору сайта EmailOrderProcessor
    public interface IOrderProcessor
    {
        void ProcessorOrder(Cart cart, ShippingDetails shippingDetails);
    }
}
