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
    public class PackageController : Controller
    {
        private IPackageRepository _packageRepository;
        private IEmployeeRepository _employeeRepository;
        public PackageController()
        {
         _packageRepository = new PackageRepository(ConfigurationManager.ConnectionStrings["ENetCareLiveAll"].ConnectionString);
         _employeeRepository = new EmployeeRepository(ConfigurationManager.ConnectionStrings["ENetCareLiveAll"].ConnectionString);
        }
        
        public PackageController(IEmployeeRepository employeeRepository, IPackageRepository packageRepository)
        {
         _packageRepository = packageRepository;
         _employeeRepository = employeeRepository;
        }
        
        // Register a new Package
        [Authorize(Roles = "Agent, Doctor")]
        public ActionResult Register()
        {
            PackageRegisterViewModel model = new PackageRegisterViewModel();

            model.ExpirationDate = DateTime.Today;

            var packageService = GetPackageService();
            var employeeService = GetEmployeeService();

            model.DistributionCentres = employeeService.GetAllDistributionCentres();
            model.StandardPackageTypes = packageService.GetAllStandardPackageTypes();

            return View(model);
        }

        [HttpPost]
        public ActionResult RegisterChangePackageType(PackageRegisterViewModel model)
        {
            var packageService = GetPackageService();
            var employeeService = GetEmployeeService();

            model.DistributionCentres = employeeService.GetAllDistributionCentres();
            model.StandardPackageTypes = packageService.GetAllStandardPackageTypes();

            if (model.StandardPackageTypeId > 0)
            {
                StandardPackageType selectedPackageType = packageService.GetStandardPackageType(model.StandardPackageTypeId);

                model.ExpirationDate = packageService.CalculateExpirationDate(selectedPackageType, DateTime.Today);
            }
            else
            {
                model.ExpirationDate = DateTime.Today;
            }

            return View("Register", model);
        }

        [HttpPost]
        public ActionResult Register(PackageRegisterViewModel model)
        {
            var packageService = GetPackageService();
            var employeeService = GetEmployeeService();

            if (ModelState.IsValid)
            {
                int packageId = 1;

                StandardPackageType selectedPackageType = packageService.GetStandardPackageType(model.StandardPackageTypeId);
                DistributionCentre selectedCentre = employeeService.GetDistributionCentre(model.LocationCentreId);
                string barCode;

                Result result = packageService.Register(selectedPackageType, selectedCentre, model.ExpirationDate, out barCode);
                if (result.Success)
                {
                    model.BarCode = barCode;
                    return View("RegisterComplete", model);
                }
                else
                {
                    ModelState.AddModelError("", result.ErrorMessage);
                }               
            }

            model.DistributionCentres = employeeService.GetAllDistributionCentres();
            model.StandardPackageTypes = packageService.GetAllStandardPackageTypes();
                
            return View("Register", model);            
        }

        [Authorize(Roles = "Agent, Doctor")]
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
            var packageService = GetPackageService();
            var employeeService = GetEmployeeService();

            Result result = new Result();

            if (ModelState.IsValid)
            {
                foreach (var package in model.SelectedPackages)
                {
                    DistributionCentre selectedCentre = employeeService.GetDistributionCentre(package.CentreId);
                    StandardPackageType spt = packageService.GetStandardPackageType(package.PackageTypeId);
                    Employee employee = employeeService.GetEmployeeByUserName(package.CurrentEmployeeUserName);

                    result = packageService.Discard(package.BarCode, selectedCentre, employee, package.ExpirationDate, spt, package.PackageId);
                    if (result.Success)
                    {
                        package.ProcessResultMessage = "Succeeded";
                    }
                    else
                    {
                        package.ProcessResultMessage = result.ErrorMessage;
                    }

                }

                return View("DiscardComplete", model);
            }

            return View("Discard", model);
        }

        [HttpPost]
        [MultiButton(Path = "/Package/Discard", MatchFormKey = "action", MatchFormValue = "Close")]
        public ActionResult DiscardClose(PackageDiscardViewModel model)
        {
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Doctor")]
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
            var packageService = GetPackageService();
            var employeeService = GetEmployeeService();

            Result result = new Result();

            if (ModelState.IsValid)
            {           
                foreach (var package in model.SelectedPackages)
                {
                    DistributionCentre selectedCentre = employeeService.GetDistributionCentre(package.CentreId);
                    StandardPackageType spt = packageService.GetStandardPackageType(package.PackageTypeId);
                    Employee employee = employeeService.GetEmployeeByUserName(package.CurrentEmployeeUserName);

                    result = packageService.Distribute(package.BarCode, selectedCentre, employee, package.ExpirationDate, spt, package.PackageId);
                    if (result.Success)
                    {
                        package.ProcessResultMessage = "Succeeded";
                    }
                    else
                    {
                        package.ProcessResultMessage = result.ErrorMessage;
                    }
                }

                return View("DistributeComplete", model);
            }

            return View("Distribute", model);
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

        private PackageService GetPackageService()
        {
            return new PackageService(_packageRepository);
        }

        private EmployeeService GetEmployeeService()
        {
            return new EmployeeService(_employeeRepository);
        }
    }
}