using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using Moq;
using Ninject;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using BookStore.Domain.Concrete;
using BookStore.WebUI.Infrastructure.Abstract;
using BookStore.WebUI.Infrastructure.Concrete;

namespace BookStore.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            
            AddBindings();
        }

        private void AddBindings()
        {
            //        Mock<IBookRepository> mock = new Mock<IBookRepository>();
            //        mock.Setup(m => m.Books).Returns(new List<Book>
            //{
            //        new Book { Name = "SimCity", Price = 1499 },
            //        new Book { Name = "TITANFALL", Price=2299 },
            //        new Book { Name = "Battlefield 4", Price=899.4M }
            //});
            //        kernel.Bind<IBookRepository>().ToConstant(mock.Object);

            kernel.Bind<IBookRepository>().To<EFBookRepository>();

            //создали объект EmailSettings, который используется в Ninject-методе 
            //WithConstructorArgument(), так что его можно внедрять в конструктор 
            //EmailOrderProcessor, когда создаются новые экземпляры для 
            //обслуживания запросов интерфейса IOrderProcessor.

            //ниже было указано значение только для одного свойства EmailSettings 
            //по имени WriteAsFile. Значение этого свойства читается с применением 
            //свойства ConfigurationManager.AppSettings, 
            //которое предоставляет доступ к настройкам приложения, размещенным 
            //в файле Web.config (из корневой папки проекта)


            EmailSettings emailSettings = new EmailSettings
            {
                WriteAsFile = bool.Parse(ConfigurationManager
                    .AppSettings["Email.WriteAsFile"] ?? "false")
            };

            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>()
                .WithConstructorArgument("settings", emailSettings);

            kernel.Bind<IAuthProvider>().To<FormAuthProvider>();


        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
    }
}