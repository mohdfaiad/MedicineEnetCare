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
    public class AuditController : Controller
    {
        [Authorize(Roles = "Agent, Doctor")]
        public ActionResult Prompt()
        {
            var model = new AuditPromptViewModel();
            model.SelectedPackages = new List<SelectedPackage>();
            model.AuditDate = DateTime.Today;

            string connectionString = ConfigurationManager.ConnectionStrings["ENetCareLiveAll"].ConnectionString;
            Entities context = new Entities(connectionString);

            model.DistributionCentres = context.DistributionCentre;
            model.StandardPackageTypes = context.StandardPackageType;

            return View(model);
        }

        [HttpPost]
        [MultiButton(Path = "/Audit/Prompt", MatchFormKey = "action", MatchFormValue = "Add")]
        public ActionResult PromptAdd(AuditPromptViewModel model)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ENetCareLiveAll"].ConnectionString;
            Entities context = new Entities(connectionString);

            model.DistributionCentres = context.DistributionCentre;
            model.StandardPackageTypes = context.StandardPackageType;

            var operations = new AuditSelectedBarCodesOperations();
            operations.StandardPackageTypeId = model.StandardPackageTypeId;

            var result = operations.Add(model);
            if (!result.Succeeded)
                ModelState.AddModelError("", result.ErrorMessage);

            return View("Prompt", model);
        }

        [HttpPost]
        [MultiButton(Path = "/Audit/Prompt", MatchFormKey = "action", MatchFormValue = "Remove")]
        public ActionResult PromptRemove(AuditPromptViewModel model)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ENetCareLiveAll"].ConnectionString;
            Entities context = new Entities(connectionString);

            model.DistributionCentres = context.DistributionCentre;
            model.StandardPackageTypes = context.StandardPackageType;

            var operations = new AuditSelectedBarCodesOperations();
            string buttonValue = HttpContext.Request["action"];

            operations.Remove(model, buttonValue);

            return View("Prompt", model);
        }

        [HttpPost]
        [MultiButton(Path = "/Audit/Prompt", MatchFormKey = "action", MatchFormValue = "Next")]
        public ActionResult PromptNext(AuditPromptViewModel model)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ENetCareLiveAll"].ConnectionString;
            Entities context = new Entities(connectionString);

            model.DistributionCentres = context.DistributionCentre;
            model.StandardPackageTypes = context.StandardPackageType;

            if (ModelState.IsValid)
            {
                return View("Display", model);
            }
            else
            {
                return View("Prompt", model);
            }
        }

        [HttpPost]
        [MultiButton(Path = "/Audit/Prompt", MatchFormKey = "action", MatchFormValue = "Close")]
        public ActionResult PromptClose(AuditPromptViewModel model)
        {
            return RedirectToAction("Index", "Home");
        }
    }
}