using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ENetCareMVC.Web.Models;
using ENetCareMVC.Repository.Data;
using ENetCareMVC.BusinessService;
using ENetCareMVC.Repository.Repository;

namespace ENetCareMVC.Web.SelectBarCodesOperations
{
    public class SelectionResult
    {
        public string ErrorMessage { get; set; }
        public bool Succeeded { get; set; }
    }
    
    public abstract class BaseSelectedBarCodesOperations
    {
        public SelectionResult Add(ISelectedBarCodesViewModel model)
        {
            if (model.SelectedPackages == null)
                model.SelectedPackages = new List<SelectedPackage>();

            var result = new SelectionResult();

            var packageService = GetPackageService();
            var package = packageService.Retrieve(model.BarCode);

            if (package == null)
            {
                result.Succeeded = false;
                result.ErrorMessage = "BarCode does not exist";
            }
            else if (model.SelectedPackages.FirstOrDefault(p => p.BarCode == model.BarCode) != null)
            {
                result.Succeeded = false;
                result.ErrorMessage = "BarCode already selected";
            }
            else
            {
                result = ValidatePackage(package);
            }

            if (result.Succeeded)
            {
                var selectedPackage = new SelectedPackage()
                {
                    BarCode = model.BarCode,
                    ExpirationDate = package.ExpirationDate,
                    PackageId = package.PackageId,
                    PackageTypeDescription = package.PackageType.Description,
                    PackageTypeId = package.PackageTypeId,
                    CentreId = package.CurrentLocation == null ? 0 : package.CurrentLocation.CentreId
                };

                model.SelectedPackages.Add(selectedPackage);
            }

            return result;
        }

        public void Remove(ISelectedBarCodesViewModel model, string buttonValue)
        {
            int id = -1;
            var splits = buttonValue.Replace(" Id: ", " ").Split(' ');
            if (splits.Count() > 1)
                id = int.Parse(splits[1]);

            var deletePackage = model.SelectedPackages.FirstOrDefault(m => m.PackageId == id);

            if (deletePackage != null)
                model.SelectedPackages.Remove(deletePackage);
        }

        protected abstract SelectionResult ValidatePackage(Package package);

        protected Employee GetCurrentEmployee()
        {
            string userId = HttpContext.Current.User.Identity.GetUserId();
            var UserManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();

            var user = UserManager.FindById(userId);

            var employeeService = GetEmployeeService();

            return employeeService.Retrieve(user.UserName);
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
    }
}