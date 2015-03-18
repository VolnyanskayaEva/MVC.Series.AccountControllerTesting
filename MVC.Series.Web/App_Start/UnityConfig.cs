using System.Web.Mvc;
using Microsoft.Practices.Unity;
using MVC.Series.Common;
using MVC.Series.Web.Models;
using Unity.Mvc5;

namespace MVC.Series.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();

			container.RegisterType<AccountManager<ApplicationUserManager, ApplicationDbContext, ApplicationUser>>(new InjectionConstructor());
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}