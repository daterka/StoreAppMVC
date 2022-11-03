using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;
using StoreApp.Services;

namespace StoreApp.App_Start
{
    public static class DIConfig
    {
        public static void RegisterDependencies(Container container)
        {
            container.RegisterMvcControllers();

            container.Register<IProductService, ProductService>();

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
    }
}