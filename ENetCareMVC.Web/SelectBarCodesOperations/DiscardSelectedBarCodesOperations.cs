﻿using ENetCareMVC.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
                result.ErrorMessage = "Package has not been expired";
            }

            return result;
        }
    }
}