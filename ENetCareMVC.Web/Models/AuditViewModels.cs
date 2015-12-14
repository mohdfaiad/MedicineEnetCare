﻿using System.Collections.Generic;
using ENetCareMVC.Repository.Data;
using System.ComponentModel.DataAnnotations;


namespace ENetCareMVC.Web.Models
{
    public class AuditPromptViewModel : ISelectedBarCodesViewModel
    {        
        [Display(Name = "BarCode")]
        public string BarCode { get; set; }

        [Required]
        [Display(Name = "Standard Package Type")]
        public int StandardPackageTypeId { get; set; }
        public List<SelectedPackage> SelectedPackages { get; set; }

        public IEnumerable<StandardPackageType> StandardPackageTypes { get; set; }
    }
}