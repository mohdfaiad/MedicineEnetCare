using ENetCareMVC.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ENetCareMVC.Web.SelectBarCodesOperations
{
    public class SendSelectedBarCodesOperations : BaseSelectedBarCodesOperations
    {
        protected override SelectionResult ValidatePackage(Package package)
        {
            SelectionResult result = new SelectionResult();
            result.Succeeded = true;

            var employee = GetCurrentEmployee();
            if (package.CurrentStatus == PackageStatus.Discarded)
            {
                result.Succeeded = false;
                result.ErrorMessage = " You can not distribute discarded package";
            }
            else if (package.CurrentStatus == PackageStatus.InTransit)
            {
                result.Succeeded = false;
                result.ErrorMessage = "Package is in transit";
            }
            else if (package.CurrentStatus == PackageStatus.Distributed)
            {
                result.Succeeded = false;
                result.ErrorMessage = "Package is distributed before.";
            }
            else if (package.CurrentStatus == PackageStatus.Lost)
            {
                result.Succeeded = false;
                result.ErrorMessage = "Package is lost";
            }
            else if (package.CurrentLocation.CentreId != employee.LocationCentreId)
            {
                result.Succeeded = false;
                result.ErrorMessage = "The package is not in your location";
            }

            return result;
        }
    }
}