using ENetCareMVC.Repository.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ENetCareMVC.Web.Models
{
    public class AuditPromptViewModel : ISelectedBarCodesViewModel
    {        
        [Display(Name = "BarCode")]
        public string BarCode { get; set; }

        [Required]
        [Display(Name = "Audit Date")]
        public DateTime AuditDate { get; set; }

        [Required]
        [Display(Name = "Distribution Centre")]
        public int DestinationCentreId { get; set; }

        [Required]
        [Display(Name = "Standard Package Type")]
        public int StandardPackageTypeId { get; set; }
        public List<SelectedPackage> SelectedPackages { get; set; }

        public IEnumerable<DistributionCentre> DistributionCentres { get; set; }
        public IEnumerable<StandardPackageType> StandardPackageTypes { get; set; }
    }
}