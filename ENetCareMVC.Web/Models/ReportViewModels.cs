using ENetCareMVC.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ENetCareMVC.Web.Models
{
    public class StocktakingReportViewModel
    {
        public DistributionCentre SelectedCentre { get; set; }
        public List<StockTaking> StocktakingList { get; set; }
    }
}