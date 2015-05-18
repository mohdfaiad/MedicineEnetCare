using ENetCareMVC.Repository.Data;
using ENetCareMVC.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ENetCareMVC.Repository.Repository
{
    public class ReportRepository : IReportRepository
    {
        private string _connectionString;
        
        public ReportRepository(string connectionString)
        {
            _connectionString = connectionString;
        }     
        
        /// <summary>
        /// Repository for Distribution Centre Stock Report
        /// </summary>
        /// <returns></returns>
        public List<DistributionCentreStock> GetDistributionCentreStock()
        {
            List<DistributionCentreStock> stockList = null;

            stockList = ViewDataAccess.GetDistributionCentreStock(_connectionString);
            return stockList;
        }

        /// <summary>
        /// Repository for Distribution Centre Losses Report
        /// </summary>
        /// <returns></returns>
        public List<DistributionCentreLosses> GetDistributionCentreLosses()
        {
            List<DistributionCentreLosses> centreList = null;
          
            centreList = ViewDataAccess.GetDistributionCentreLosses(_connectionString);       
            return centreList;
        }

        /// <summary>
        /// Repository for Doctor Activity Report
        /// </summary>
        /// <returns></returns>
        public List<DoctorActivity> GetDoctorActivity()
        {
            List<DoctorActivity> doctors = null;
            doctors = ViewDataAccess.GetDoctorActivity(_connectionString);
            
            return doctors;
        }

        /// <summary>
        /// Repository for Global Stock Report
        /// </summary>
        /// <returns></returns>
        public List<GlobalStock> GetGlobalStock()
        {
            List<GlobalStock> stocks = null;
            stocks = ViewDataAccess.GetGlobalStock(_connectionString);
            
            return stocks;
        }

        /// <summary>
        /// Repository for Value In Transit Report
        /// </summary>
        /// <returns></returns>
        public List<ValueInTransit> GetValueInTransit()
        {
            List<ValueInTransit> valueList = null;
            valueList = ViewDataAccess.GetValueInTransit(_connectionString);
            
            return valueList;
        }

        /// <summary>
        /// Repository for Reconciled Package screen in Package Audit Wizard
        /// </summary>
        /// <param name="currentLocation"></param>
        /// <param name="packageType"></param>
        /// <param name="barCodeList"></param>
        /// <returns></returns>
        public List<ReconciledPackage> GetReconciledPackages(DistributionCentre currentLocation, StandardPackageType packageType, List<string> barCodeList)
        {
            List<ReconciledPackage> packageList = null;
            XElement barCodeXml = barCodeList.GetBarCodeXML();

            packageList = ViewDataAccess.GetReconciledPackages(_connectionString, currentLocation, packageType, barCodeXml);            
            return packageList;
        }

        /// <summary>
        /// Repository for Stocktaking Report
        /// </summary>
        /// <param name="centreId"></param>
        /// <returns></returns>
        public List<StockTaking> GetStocktaking(int centreId)
        {
            List<StockTaking> packageList = null;
            packageList = ViewDataAccess.GetStocktaking(_connectionString, centreId);
            
            return packageList;
        }
    }
}
