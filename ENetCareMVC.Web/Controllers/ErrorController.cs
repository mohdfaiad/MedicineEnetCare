using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ENetCareMVC.Web.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Generic()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Generic(string action)
        {
            return RedirectToAction("Index", "Home");
        }

        public ActionResult MissingResource()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MissingResource(string action)
        {
            return RedirectToAction("Index", "Home");
        }
    }
}