using System.Collections.Generic;
using ENetCareMVC.Repository.Data;

namespace ENetCareMVC.Web.Models
{
    public class StocktakingReportViewModel
    {
        public DistributionCentre SelectedCentre { get; set; }
        public List<StockTaking> StocktakingList { get; set; }
    }
}