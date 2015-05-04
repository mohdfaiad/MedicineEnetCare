using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ENetCareMVC.Web.Controllers
{
    public class PackageController : Controller
    {
        // GET: Package
        public ActionResult RegisterPackage()
        {
            return View();
        }

        public ActionResult Send()
        {
            return View();
        }

        public ActionResult Receive()
        {
            return View();
        }

        public ActionResult Distribute()
        {
            return View();
        }

        public ActionResult Discard()
        {
            return View();
        }

        public ActionResult Audit()
        {
            return View();
        }
    }
}