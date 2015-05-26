using ENetCareMVC.Repository.Data;
using ENetCareMVC.BusinessService;
using ENetCareMVC.Web.Controllers;
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
    public class PackageTransitController : Controller
    {
        // GET: PackageTransit
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Agent, Doctor")]
        public ActionResult Send()
        {

            var model = new PackageTransitSendViewModel();
            model.SelectedPackages = new List<SelectedPackage>();
            model.SendDate = DateTime.Today;

            //Result result = packageService.Send()

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
            var packageService = GetPackageService();
            var employeeService = GetEmployeeService();
            var employee = GetCurrentEmployee();

            //Package tempPackage = packageService.Retrieve(model.BarCode);
            ///DistributionCentre senderCentre = tempPackage.CurrentLocation;
            //DistributionCentre recieverCentre1 = employeeService.GetDistributionCentre(model.DestinationCentreId);
            //DistributionCentre senderCentre1 = employeeService.GetDistributionCentre(employee.LocationCentreId);

            if (ModelState.IsValid)
            {
                foreach (var package in model.SelectedPackages)
                {
                    DistributionCentre recieverCentre = employeeService.GetDistributionCentre(model.DestinationCentreId);
                    DistributionCentre senderCentre = employeeService.GetDistributionCentre(employee.LocationCentreId);
                    Package tempPack = packageService.Retrieve(package.BarCode);
                    Result result = packageService.Send(tempPack, senderCentre, recieverCentre, model.SendDate);
                    if (result.Success)
                    {
                        package.ProcessResultMessage = "Successful!";
                    }
                    else
                    {
                        package.ProcessResultMessage = result.ErrorMessage;
                    }
                }

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

        [Authorize(Roles = "Agent, Doctor")]
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
            var packageService = GetPackageService();
            var employeeService = GetEmployeeService();
            var employee = GetCurrentEmployee();
            model.ReceiveDate = DateTime.Today;

            if (ModelState.IsValid)
            {
                foreach (var package in model.SelectedPackages)
                {
                    DistributionCentre locationCentre = employeeService.GetDistributionCentre(employee.LocationCentreId);
                    Result result = packageService.Receive(package.BarCode, locationCentre, model.ReceiveDate);
                    if (result.Success)
                    {
                        package.ProcessResultMessage = "Successful!";
                    }
                    else
                    {
                        package.ProcessResultMessage = "Not Successful!";
                    }
                }

                return View("ReceiveComplete", model);
            }
            else
            {
                return View("Receive", model);
            }
            //if (ModelState.IsValid)
            //{
            //    return View("ReceiveComplete", model);
            //}
            //else
            //{
            //    return View("Receive", model);
            //}
        }

        [HttpPost]
        [MultiButton(Path = "/PackageTransit/Receive", MatchFormKey = "action", MatchFormValue = "Close")]
        public ActionResult ReceiveClose(PackageTransitReceiveViewModel model)
        {
            return RedirectToAction("Index", "Home");
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