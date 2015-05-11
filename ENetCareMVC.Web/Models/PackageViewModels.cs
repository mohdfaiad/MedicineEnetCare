using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ENetCareMVC.Web.Models
{
 
    public class PackageDiscardViewModel : ISelectedBarCodesViewModel
    {
        [Required]
        public string BarCode { get; set; }

        public List<SelectedPackage> SelectedPackages { get; set; }
    }

    public class PackageDistributeViewModel : ISelectedBarCodesViewModel
    {
        [Required]
        public string BarCode { get; set; }

        public List<SelectedPackage> SelectedPackages { get; set; }
    }


}