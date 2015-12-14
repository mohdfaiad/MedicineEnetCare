using System.Data;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using ENetCareMVC.Repository.Data;

namespace ENetCareMVC.Repository
{
    public class ViewDataAccess
    {
        /// <summary>
        /// Query for Distribution Centre Stock Report
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static List<DistributionCentreStock> GetDistributionCentreStock(string connectionString)
        {                                                   
            var stockList = new List<DistributionCentreStock>();
            using (var ctx = new Entities(connectionString))
            {
                stockList = ctx.DistributionCentreStock.ToList();
            }

            return stockList;
        }

        /// <summary>
        /// Query for Distribution Centre Losses Report
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static List<DistributionCentreLosses> GetDistributionCentreLosses(string connectionString)
        {
            List<DistributionCentreLosses> centreList = new List<DistributionCentreLosses>();

            using (var ctx = new Entities(connectionString))
            {
                centreList = ctx.DistributionCentreLosses.ToList();
            }

            return centreList;
        }

        /// <summary>
        /// Query for Doctor Activity Report
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static List<DoctorActivity> GetDoctorActivity(string connectionString)
        {

            List<DoctorActivity> doctors = new List<DoctorActivity>();
            using (var ctx = new Entities(connectionString))
            {
                doctors = ctx.DoctorActivity.ToList();
            }
  
            return doctors;
        }

        /// <summary>
        /// Query for Global Stock Report
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static List<GlobalStock> GetGlobalStock(string connectionString)
        {
            List<GlobalStock> stocks = new List<GlobalStock>();
            using (var ctx = new Entities(connectionString))
            {
                stocks = ctx.GlobalStock.ToList();
            }
   
            return stocks;
        }

        /// <summary>
        /// Query for Value In Transit Report
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static List<ValueInTransit> GetValueInTransit(string connectionString)
        {
            List<ValueInTransit> valueList = new List<ValueInTransit>();
            using (var ctx = new Entities(connectionString))
            {
                valueList = ctx.ValueInTransit.ToList();
            }
   
            return valueList;

        }

        /// <summary>
        /// Query for Reconciled Package screen in Package Audit Wizard
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="currentLocation"></param>
        /// <param name="packageType"></param>
        /// <param name="barCodeXml"></param>
        /// <returns></returns>
        public static List<ReconciledPackage> GetReconciledPackages(string connectionString, DistributionCentre currentLocation, StandardPackageType packageType, List<string> barCodeList)
        {
            var reconciledPackages = new List<ReconciledPackage>();
            
            using (var ctx = new Entities(connectionString))
            {
                var receivedPackages = (from p in ctx.Package.Include("CurrentLocation")
                                       join b in barCodeList on p.BarCode equals b                                       
                                       where p.PackageTypeId == packageType.PackageTypeId && (p.CurrentLocationCentreId != currentLocation.CentreId || p.CurrentStatus != PackageStatus.InStock)
                                       select p).ToList();
                foreach (var package in receivedPackages)
                {
                    var reconciledPackage = new ReconciledPackage()
                    {
                        PackageId = package.PackageId,
                        BarCode = package.BarCode,
                        CurrentLocationCentreId = package.CurrentLocationCentreId ?? -1,
                        CurrentLocationCentreName = package.CurrentLocationCentreId == null ? string.Empty : package.CurrentLocation.Name,
                        CurrentStatus = package.CurrentStatus,
                        NewStatus = PackageStatus.InStock  
                    };
                    reconciledPackages.Add(reconciledPackage);
                }
                var lostPackages = (from p in ctx.Package.Include("CurrentLocation")
                    join b in barCodeList on p.BarCode equals b into ps                    
                    from b in ps.DefaultIfEmpty()
                    where p.PackageTypeId == packageType.PackageTypeId && p.CurrentLocationCentreId == currentLocation.CentreId &&
                    b == null && p.CurrentStatus == PackageStatus.InStock
                select p).ToList();
                foreach (var package in lostPackages)
                {
                     var reconciledPackage = new ReconciledPackage()
                     {
                        PackageId = package.PackageId,
                        BarCode = package.BarCode,
                        CurrentLocationCentreId = package.CurrentLocationCentreId.Value,
                        CurrentLocationCentreName = package.CurrentLocation.Name,
                        CurrentStatus = package.CurrentStatus,
                        NewStatus = PackageStatus.Lost  
                     };
                    reconciledPackages.Add(reconciledPackage);
                }
            }
            return reconciledPackages;
        }

        /// <summary>
        /// Query for Stocktaking Report
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="centreId"></param>
        /// <returns></returns>
        public static List<StockTaking> GetStocktaking(string connectionString, int centreId)
        {
            List<StockTaking> stocks = new List<StockTaking>();
            using (var ctx = new Entities(connectionString))
            {
                stocks = (from e in ctx.StockTaking
                          where e.CurrentLocationCentreId == centreId
                          select e).ToList();
            }

            Debug.WriteLine("ViewDataAccess returns " + stocks.Count() + " items");
            return stocks;
        }
    }
}
