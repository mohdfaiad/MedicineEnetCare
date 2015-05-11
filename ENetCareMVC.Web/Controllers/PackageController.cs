using ENetCareMVC.Web.Models;
using ENetCareMVC.Web.SelectBarCodesOperations;
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

        public ActionResult Discard()
        {
            var model = new PackageDiscardViewModel();
            model.SelectedPackages = new List<SelectedPackage>();

            return View(model);
        }

        [HttpPost]
        [MultiButton(Path = "/Package/Discard", MatchFormKey = "action", MatchFormValue = "Add")]
        public ActionResult DiscardAdd(PackageDiscardViewModel model)
        {
            var operations = new DiscardSelectedBarCodesOperations();

            var result = operations.Add(model);
            if (!result.Succeeded)
                ModelState.AddModelError("", result.ErrorMessage);

            return View("Discard", model);
        }

        [HttpPost]
        [MultiButton(Path = "/Package/Discard", MatchFormKey = "action", MatchFormValue = "Remove")]
        public ActionResult DiscardRemove(PackageDiscardViewModel model)
        {            
            string buttonValue = HttpContext.Request["action"];

            var operations = new DiscardSelectedBarCodesOperations();

            operations.Remove(model, buttonValue);

            return View("Discard", model);
        }

        [HttpPost]
        [MultiButton(Path = "/Package/Discard", MatchFormKey = "action", MatchFormValue = "Save")]
        public ActionResult DiscardSave(PackageDiscardViewModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (var package in model.SelectedPackages)
                    package.ProcessResultMessage = "Succeeded";

                return View("DiscardComplete", model);
            }
            else
            {
                return View("Discard", model);
            }
        }

        [HttpPost]
        [MultiButton(Path = "/Package/Discard", MatchFormKey = "action", MatchFormValue = "Close")]
        public ActionResult ReceiveClose(PackageDiscardViewModel model)
        {
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Distribute()
        {
            var model = new PackageDistributeViewModel();
            model.SelectedPackages = new List<SelectedPackage>();

            return View(model);
        }

        [HttpPost]
        [MultiButton(Path = "/Package/Distribute", MatchFormKey = "action", MatchFormValue = "Add")]
        public ActionResult DistributeAdd(PackageDistributeViewModel model)
        {
            var operations = new DistributeSelectedBarCodesOperations();

            var result = operations.Add(model);
            if (!result.Succeeded)
                ModelState.AddModelError("", result.ErrorMessage);

            return View("Distribute", model);
        }

        [HttpPost]
        [MultiButton(Path = "/Package/Distribute", MatchFormKey = "action", MatchFormValue = "Remove")]
        public ActionResult DistributeRemove(PackageDistributeViewModel model)
        {
            string buttonValue = HttpContext.Request["action"];

            var operations = new DistributeSelectedBarCodesOperations();

            operations.Remove(model, buttonValue);

            return View("Distribute", model);
        }

        [HttpPost]
        [MultiButton(Path = "/Package/Distribute", MatchFormKey = "action", MatchFormValue = "Save")]
        public ActionResult DistributeSave(PackageDistributeViewModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (var package in model.SelectedPackages)
                    package.ProcessResultMessage = "Succeeded";

                return View("DistributeComplete", model);
            }
            else
            {
                return View("Distribute", model);
            }
        }

        [HttpPost]
        [MultiButton(Path = "/Package/Distribute", MatchFormKey = "action", MatchFormValue = "Close")]
        public ActionResult ReceiveClose(PackageDistributeViewModel model)
        {
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Audit()
        {
            return View();
        }
    }
}