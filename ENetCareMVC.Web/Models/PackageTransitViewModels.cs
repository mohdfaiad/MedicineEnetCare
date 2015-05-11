using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Owin.Security;
using ENetCareMVC.Repository.Data;

namespace ENetCareMVC.Web.Models
{
    public class PackageTransitSendViewModel : ISelectedBarCodesViewModel
    {
        [Required]
        public string BarCode { get; set; }

        [Required]
        [Display(Name = "Send Date")]
        public DateTime SendDate { get; set; }

        [Required]
        public int DestinationCentreId { get; set; }

        public List<SelectedPackage> SelectedPackages { get; set; }

        public IEnumerable<DistributionCentre> DistributionCentres { get; set; }
    }

    public class SelectedPackage
    {
        public int PackageId { get; set; }
        public string BarCode { get; set; }
        public string PackageTypeDescription { get; set; }
        public DateTime ExpirationDate { get; set; }

        public string ProcessResultMessage { get; set; }
    }

    public interface ISelectedBarCodesViewModel
    {
        string BarCode { get; set; }

        List<SelectedPackage> SelectedPackages { get; set; }
    }
    
    public class PackageTransitReceiveViewModel : ISelectedBarCodesViewModel
    {
        public string BarCode { get; set; }

        public List<SelectedPackage> SelectedPackages { get; set; }
        
        [Required]
        public DateTime ReceiveDate { get; set; }
    }
}