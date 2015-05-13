using ENetCareMVC.Repository.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ENetCareMVC.Web.Models
{
    public class PackageRegisterViewModel
    {
        [Required]
        public int StandardPackageTypeId { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }

        [Required]
        public int LocationCentreId { get; set; }

        public string BarCode { get; set; }

        public IEnumerable<StandardPackageType> StandardPackageTypes { get; set; }
        public IEnumerable<DistributionCentre> DistributionCentres { get; set; }
    }

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