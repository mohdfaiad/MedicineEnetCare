using System;
using System.Collections.Generic;
using ENetCareMVC.Repository.Data;
using System.ComponentModel.DataAnnotations;

namespace ENetCareMVC.Web.Models
{
    public class PackageRegisterViewModel
    {
        [Required]
        [Display(Name = "Standard Package Type")]
        public int StandardPackageTypeId { get; set; }

        [Required]
        [Display(Name = "Expiration Date")]
        public DateTime ExpirationDate { get; set; }

        [Required]
        [Display(Name = "Location Centre")]
        public int LocationCentreId { get; set; }

        [Display(Name = "BarCode")]
        public string BarCode { get; set; }

        public IEnumerable<StandardPackageType> StandardPackageTypes { get; set; }
        public IEnumerable<DistributionCentre> DistributionCentres { get; set; }
    }

    public class PackageDiscardViewModel : ISelectedBarCodesViewModel
    {        
        public string BarCode { get; set; }

        public List<SelectedPackage> SelectedPackages { get; set; }
    }

    public class PackageDistributeViewModel : ISelectedBarCodesViewModel
    {       
        public string BarCode { get; set; }

        public List<SelectedPackage> SelectedPackages { get; set; }

    }


}