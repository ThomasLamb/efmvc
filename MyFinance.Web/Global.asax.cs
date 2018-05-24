using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using MyFinance.Web.Models;
using System.Web.Security;
using MyFinance.Web.IoC;
using MyFinance.Data;
using MyFinance.Data.Infrastructure;
using MyFinance.Domain;
using MyFinance.Web.ViewModel;
using MyFinance.Service;
using MyFinance.Web.Controllers;
using MyFinance.Web.Helpers;
using System.Data.Entity;
namespace MyFinance.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("elmah.axd");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },// Parameter defaults
                new[] { "MyFinance.Web.Controllers" }  
            );

        }

protected void Application_Start()
{
    AreaRegistration.RegisterAllAreas();
    // Initializes and seeds the database.
    Database.SetInitializer(new MyFinanceContextInitializer());
    GlobalFilters.Filters.Add(new RedirectMobileDevicesToMobileAreaAttribute(), 1);    
    RegisterGlobalFilters(GlobalFilters.Filters);
    RegisterRoutes(RouteTable.Routes);
    IUnityContainer container = GetUnityContainer();
    DependencyResolver.SetResolver(new UnityDependencyResolver(container));
}

    private IUnityContainer GetUnityContainer()
    {
        //Create UnityContainer          
        IUnityContainer container = new UnityContainer()
            //.RegisterType<IControllerActivator, CustomControllerActivator>() // No nned to a controller activator
        .RegisterType<IFormsAuthenticationService, FormsAuthenticationService>()
        .RegisterType<IMembershipService, AccountMembershipService>()
        .RegisterInstance<MembershipProvider>(Membership.Provider)
        .RegisterType<IDatabaseFactory, DatabaseFactory>(new HttpContextLifetimeManager<IDatabaseFactory>())
        .RegisterType<IUnitOfWork, UnitOfWork>(new HttpContextLifetimeManager<IUnitOfWork>())
        .RegisterType<ICategoryRepository, CategoryRepository>(new HttpContextLifetimeManager<ICategoryRepository>())
        .RegisterType<IExpenseRepository, ExpenseRepository>(new HttpContextLifetimeManager<IExpenseRepository>())
        .RegisterType<ICategoryService, CategoryService>(new HttpContextLifetimeManager<ICategoryService>())
        .RegisterType<IExpenseService, ExpenseService>(new HttpContextLifetimeManager<IExpenseService>())
        .RegisterType<IUserRepository, UserRepository>(new HttpContextLifetimeManager<IUserRepository>())
         .RegisterType<IRoleRepository, RoleRepository>(new HttpContextLifetimeManager<IRoleRepository>())
          .RegisterType<ISecurityService, SecurityService>(new HttpContextLifetimeManager<ISecurityService>());
        return container;         
    }
 }
}