using ENetCareMVC.Repository.Data;
using ENetCareMVC.Web.Models;
using ENetCareMVC.Web.SelectBarCodesOperations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ENetCareMVC.Web.Controllers
{
    public class PackageTransitController : Controller
    {
        // GET: PackageTransit
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Send()
        {

            var model = new PackageTransitSendViewModel();
            model.SelectedPackages = new List<SelectedPackage>();

            string connectionString = ConfigurationManager.ConnectionStrings["ENetCareLiveAll"].ConnectionString;
            Entities context = new Entities(connectionString);

            model.DistributionCentres = context.DistributionCentre;

            return View(model);
        }

        [HttpPost]
        [MultiButton(Path = "/PackageTransit/Send", MatchFormKey = "action", MatchFormValue = "Add")]
        public ActionResult SendAdd(PackageTransitSendViewModel model)
        {            
            string connectionString = ConfigurationManager.ConnectionStrings["ENetCareLiveAll"].ConnectionString;
            Entities context = new Entities(connectionString);

            model.DistributionCentres = context.DistributionCentre;

            var operations = new SendSelectedBarCodesOperations();

            var result = operations.Add(model);
            if (!result.Succeeded)
                ModelState.AddModelError("", result.ErrorMessage);

            return View("Send", model);
        }

        [HttpPost]
        [MultiButton(Path = "/PackageTransit/Send", MatchFormKey = "action", MatchFormValue = "Remove")]
        public ActionResult SendRemove(PackageTransitSendViewModel model)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ENetCareLiveAll"].ConnectionString;
            Entities context = new Entities(connectionString);

            model.DistributionCentres = context.DistributionCentre;

            var operations = new SendSelectedBarCodesOperations();
            string buttonValue = HttpContext.Request["action"];

            operations.Remove(model, buttonValue);

            return View("Send", model);
        }

        [HttpPost]
        [MultiButton(Path = "/PackageTransit/Send", MatchFormKey = "action", MatchFormValue = "Save")]
        public ActionResult SendSave(PackageTransitSendViewModel model)
        {
            if (ModelState.IsValid)
            {
                return View("SendComplete", model);
            }
            else
            {
                return View("Send", model);
            }
        }

        [HttpPost]
        [MultiButton(Path = "/PackageTransit/Send", MatchFormKey = "action", MatchFormValue = "Close")]
        public ActionResult SendClose(PackageTransitSendViewModel model)
        {
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Receive()
        {
            PackageTransitReceiveViewModel model = new PackageTransitReceiveViewModel();
            model.SelectedPackages = new List<SelectedPackage>();

            return View(model);
        }

        [HttpPost]
        [MultiButton(Path = "/PackageTransit/Receive", MatchFormKey = "action", MatchFormValue = "Add")]
        public ActionResult RevieveAdd(PackageTransitReceiveViewModel model)
        {
            var operations = new ReceiveSelectedBarCodesOperations();

            var result = operations.Add(model);
            if (!result.Succeeded)
                ModelState.AddModelError("", result.ErrorMessage);

            return View("Receive", model);
        }

        [HttpPost]
        [MultiButton(Path = "/PackageTransit/Receive", MatchFormKey = "action", MatchFormValue = "Remove")]
        public ActionResult ReceiveRemove(PackageTransitReceiveViewModel model)
        {
            int id = -1;
            string buttonValue = HttpContext.Request["action"];

            var operations = new ReceiveSelectedBarCodesOperations();

            operations.Remove(model, buttonValue);

            return View("Receive", model);
        }

        [HttpPost]
        [MultiButton(Path = "/PackageTransit/Receive", MatchFormKey = "action", MatchFormValue = "Save")]
        public ActionResult ReceiveSave(PackageTransitReceiveViewModel model)
        {
            if (ModelState.IsValid)
            {
                return View("ReceiveComplete", model);
            }
            else
            {
                return View("Receive", model);
            }
        }

        [HttpPost]
        [MultiButton(Path = "/PackageTransit/Receive", MatchFormKey = "action", MatchFormValue = "Close")]
        public ActionResult ReceiveClose(PackageTransitReceiveViewModel model)
        {
            return RedirectToAction("Index", "Home");
        }

    }
}