using ENetCareMVC.Repository.Data;

namespace ENetCareMVC.Web.SelectBarCodesOperations
{
    public class ReceiveSelectedBarCodesOperations : BaseSelectedBarCodesOperations
    {
        protected override SelectionResult ValidatePackage(Package package)
        {
            SelectionResult result = new SelectionResult();
            result.Succeeded = true;
            var employee = GetCurrentEmployee();

            if (package.CurrentStatus == PackageStatus.InStock &&
                package.CurrentLocationCentreId == employee.LocationCentreId)
            {
                result.Succeeded = false;
                result.ErrorMessage = "Package already in stock at " + employee.Location.Name;
            }

            return result;
        }
    }
}