using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyFinance.Service;
using System.Web.Helpers;
using System.Globalization;
using MyFinance.Data;
using MyFinance.Data.Infrastructure;
namespace MyFinance.Web.Controllers
{
    public class HomeController : Controller
    {

       
        public ActionResult Chart()
        {           
            return null;
         }
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult About()
        {
            return View();
        }        
    }   
}
