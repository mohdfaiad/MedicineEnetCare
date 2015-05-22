using ENetCareMVC.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ENetCareMVC.Web.SelectBarCodesOperations
{
    public class AuditSelectedBarCodesOperations : BaseSelectedBarCodesOperations
    {
        public int StandardPackageTypeId { get; set; }

        protected override SelectionResult ValidatePackage(Package package)
        {
            SelectionResult result = new SelectionResult();
            result.Succeeded = true;            

            if (StandardPackageTypeId == 0)
            {
                result.Succeeded = false;
                result.ErrorMessage = "Please select a Package Type";
            }
            else if (package.PackageTypeId != StandardPackageTypeId)
            {
                result.Succeeded = false;
                result.ErrorMessage = "The package with this barcode isn't the same type as the selected package type";
            }

            return result;
        }
    }
}