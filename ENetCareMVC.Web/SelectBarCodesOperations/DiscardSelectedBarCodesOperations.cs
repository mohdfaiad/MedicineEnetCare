using System;
using ENetCareMVC.Repository.Data;

namespace ENetCareMVC.Web.SelectBarCodesOperations
{
    public class DiscardSelectedBarCodesOperations : BaseSelectedBarCodesOperations
    {
        protected override SelectionResult ValidatePackage(Package package)
        {
            SelectionResult result = new SelectionResult();
            result.Succeeded = true;

            var employee = GetCurrentEmployee();

            if (package.CurrentStatus != PackageStatus.InStock)
            {
                result.Succeeded = false;
                result.ErrorMessage = "Package not in stock";
            }
            else if (package.ExpirationDate > DateTime.Today)
            {
                result.Succeeded = false;
                result.ErrorMessage = "That package has not expired yet, it will expire on " + package.ExpirationDate.ToString("d");
            }
            else if (package.CurrentLocationCentreId != employee.LocationCentreId)
            {
                result.Succeeded = false;
                result.ErrorMessage = "That package is not in the same distribution centre as the logged in user.";
            }

            return result;
        }
    }
}