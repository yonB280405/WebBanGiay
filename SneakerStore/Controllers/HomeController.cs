using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SneakerStore.Models;

namespace SneakerStore.Controllers
{
    public class HomeController : Controller
    {
        DBSneakerStoreEntities database = new DBSneakerStoreEntities();
        public ActionResult Admin()
        {
            return View();
        }
       
    }
}