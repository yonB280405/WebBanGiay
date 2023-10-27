using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SneakerStore.Models;

namespace SneakerStore.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Admin()
        {
            return View();
        }
    }
}