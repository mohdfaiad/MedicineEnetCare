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
            if (package.CurrentStatus == PackageStatus.InTransit)
            {
                result.Succeeded = false;
                result.ErrorMessage = "Package is in transit";
            }
            if (package.CurrentStatus == PackageStatus.Distributed)
            {
                result.Succeeded = false;
                result.ErrorMessage = "Package is distributed before.";
            }
            //if (package.CurrentStatus != PackageStatus.InStock)
            //{
            //    result.Succeeded = false;
            //    result.ErrorMessage = "Package not in stock";
            //}
            if (package.CurrentLocation.CentreId != employee.LocationCentreId)
            {
                result.Succeeded = false;
                result.ErrorMessage = "The package is not in your location";
            }

            return result;
        }
    }
}