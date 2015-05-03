using ENetCareMVC.Repository.Data;
using ENetCareMVC.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ENetCare.Repository.Repository
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
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                stockList = ViewDataAccess.GetDistributionCentreStock(connection);
            }
            return stockList;
        }

        /// <summary>
        /// Repository for Distribution Centre Losses Report
        /// </summary>
        /// <returns></returns>
        public List<DistributionCentreLoss> GetDistributionCentreLosses()
        {
            List<DistributionCentreLoss> centreList = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                centreList = ViewDataAccess.GetDistributionCentreLosses(connection);
            }
            return centreList;
        }

        /// <summary>
        /// Repository for Doctor Activity Report
        /// </summary>
        /// <returns></returns>
        public List<DoctorActivity> GetDoctorActivity()
        {
            List<DoctorActivity> doctors = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                doctors = ViewDataAccess.GetDoctorActivity(connection);
            }
            return doctors;
        }

        /// <summary>
        /// Repository for Global Stock Report
        /// </summary>
        /// <returns></returns>
        public List<GlobalStock> GetGlobalStock()
        {
            List<GlobalStock> stocks = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                stocks = ViewDataAccess.GetGlobalStock(connection);
            }
            return stocks;
        }

        /// <summary>
        /// Repository for Value In Transit Report
        /// </summary>
        /// <returns></returns>
        public List<ValueInTransit> GetValueInTransit()
        {
            List<ValueInTransit> valueList = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                valueList = ViewDataAccess.GetValueInTransit(connection);
            }
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
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                XElement barCodeXml = barCodeList.GetBarCodeXML();

                packageList = ViewDataAccess.GetReconciledPackages(connection, currentLocation, packageType, barCodeXml);
            }
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
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                packageList = ViewDataAccess.GetStocktaking(connection, centreId);
            }
            return packageList;
        }
    }
}
