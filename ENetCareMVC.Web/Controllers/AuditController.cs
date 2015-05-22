using ENetCareMVC.BusinessService;
using ENetCareMVC.Repository.Data;
using ENetCareMVC.Repository.Repository;
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
            
            var packageService = GetPackageService();

            model.StandardPackageTypes = packageService.GetAllStandardPackageTypes();

            return View(model);
        }

        [HttpPost]
        public ActionResult AuditChangePackageType(AuditPromptViewModel model)
        {
            var packageService = GetPackageService();

            model.StandardPackageTypes = packageService.GetAllStandardPackageTypes();

            if (model.SelectedPackages == null)
                model.SelectedPackages = new List<SelectedPackage>();

            model.SelectedPackages.Clear();

            return View("Prompt", model);
        }

        [HttpPost]
        [MultiButton(Path = "/Audit/Prompt", MatchFormKey = "action", MatchFormValue = "Add")]
        public ActionResult PromptAdd(AuditPromptViewModel model)
        {
            var packageService = GetPackageService();

            model.StandardPackageTypes = packageService.GetAllStandardPackageTypes();

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
            var packageService = GetPackageService();

            model.StandardPackageTypes = packageService.GetAllStandardPackageTypes();

            var operations = new AuditSelectedBarCodesOperations();
            string buttonValue = HttpContext.Request["action"];

            operations.Remove(model, buttonValue);

            return View("Prompt", model);
        }

        [HttpPost]
        [MultiButton(Path = "/Audit/Prompt", MatchFormKey = "action", MatchFormValue = "Next")]
        public ActionResult PromptNext(AuditPromptViewModel model)
        {
            var packageService = GetPackageService();

            model.StandardPackageTypes = packageService.GetAllStandardPackageTypes();
            if (model.SelectedPackages == null)
                model.SelectedPackages = new List<SelectedPackage>();

            if (ModelState.IsValid)
            {
                var currentLocation = GetCurrentEmployee().Location;
                var packageType = model.StandardPackageTypes.FirstOrDefault(t => t.PackageTypeId == model.StandardPackageTypeId);
                List<string> barCodeList = model.SelectedPackages.Select(p => p.BarCode).ToList();
                var reportService = GetReportService();

                var reconciledPackages = reportService.GetReconciledPackages(currentLocation, packageType, barCodeList);
                TempData["AuditModel"] = model;

                return View("Display", reconciledPackages);
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

        [HttpPost]
        [MultiButton(Path = "/Audit/Display", MatchFormKey = "action", MatchFormValue = "Next")]
        public ActionResult DisplayNext()
        {
            var model = (AuditPromptViewModel)TempData["AuditModel"];

            var employee = GetCurrentEmployee();
            var packageType = model.StandardPackageTypes.FirstOrDefault(t => t.PackageTypeId == model.StandardPackageTypeId);
            List<string> barCodeList = model.SelectedPackages.Select(p => p.BarCode).ToList();
            var packageService = GetPackageService();

            Result result = packageService.PerformAudit(employee, packageType, barCodeList);

            if (result.Success)
            {
                ViewBag.CompleteMessage = "Audit completed successfully";
            }
            else
            {
                ViewBag.CompleteMessage = string.Format("Audit Failed: {0}", result.ErrorMessage);
            }

            return View("Finish");
        }

        [HttpPost]
        [MultiButton(Path = "/Audit/Display", MatchFormKey = "action", MatchFormValue = "Previous")]
        public ActionResult DisplayPrevious()
        {
            var model = (AuditPromptViewModel)TempData["AuditModel"];

            return View("Prompt", model);
        }

        private PackageService GetPackageService()
        {
            IPackageRepository packageRepository = new PackageRepository(ConfigurationManager.ConnectionStrings["ENetCareLiveAll"].ConnectionString);
            return new PackageService(packageRepository);
        }

        private EmployeeService GetEmployeeService()
        {
            IEmployeeRepository repository = new EmployeeRepository(ConfigurationManager.ConnectionStrings["ENetCareLiveAll"].ConnectionString);
            return new EmployeeService(repository);
        }

        private ReportService GetReportService()
        {
            IReportRepository repository = new ReportRepository(ConfigurationManager.ConnectionStrings["ENetCareLiveAll"].ConnectionString);
            return new ReportService(repository);
        }

        private Employee GetCurrentEmployee()
        {
            string username = User.Identity.Name;
            if (string.IsNullOrEmpty(username))
                return null;

            var employeeService = GetEmployeeService();

            return employeeService.Retrieve(username);
        }
    }
}