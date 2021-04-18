using Adonis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Adonis.Controllers
{
    public class HomeController : Controller
    {
        AppGeneral app = new AppGeneral();
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            StartApp();
            return View();
        }

        public void StartApp()
        {
            ViewBag.AppVersion = app.GetVersion();
            ViewBag.Features = app.GetFeatures();
            ViewBag.Warning = app.GetWarning();

            User loggedInUser = new User()
            {
                IdUser = -1,
                Name = "Guest",
            };
        }
    }
}
