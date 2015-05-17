using ENetCareMVC.Repository.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ENetCareMVC.Repository
{
    public class ViewDataAccess
    {
        /// <summary>
        /// Query for Distribution Centre Stock Report
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static List<DistributionCentreStock> GetDistributionCentreStock()
        {                                                   
            var stockList = new List<DistributionCentreStock>();
            using (var ctx = new Entities())
            {
                stockList = ctx.DistributionCentreStock.ToList();
            }
            /*
            string query = "select PackageTypeId, PackageTypeDescription, CostPerPackage, DistributionCentreId, DistributionCenterName, NumberOfPackages, TotalValue from DistributionCentreStock order by DistributionCentreId, PackageTypeId";
            var cmd = new SqlCommand(query);
            cmd.Connection = connection;
            using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default))
            {
                while (reader.Read())
                {
                    var stock = new DistributionCentreStock();
                    if (reader["PackageTypeId"] != DBNull.Value)
                        stock.PackageTypeId = Convert.ToInt32(reader["PackageTypeId"]);
                    if (reader["PackageTypeDescription"] != DBNull.Value)
                        stock.PackageTypeDescription = (string)reader["PackageTypeDescription"];
                    if (reader["CostPerPackage"] != DBNull.Value)
                        stock.CostPerPackage = Convert.ToDecimal(reader["CostPerPackage"]);
                    if (reader["DistributionCentreId"] != DBNull.Value)
                        stock.DistributionCentreId = Convert.ToInt32(reader["DistributionCentreId"]);
                    if (reader["DistributionCenterName"] != DBNull.Value)
                        stock.DistributionCenterName = (string)reader["DistributionCenterName"];
                    if (reader["NumberOfPackages"] != DBNull.Value)
                        stock.NumberOfPackages = Convert.ToInt32(reader["NumberOfPackages"]);
                    if (reader["TotalValue"] != DBNull.Value)
                        stock.TotalValue = Convert.ToDecimal(reader["TotalValue"]);
                    stockList.Add(stock);

                }
            }
             */
            return stockList;
        }

        /// <summary>
        /// Query for Distribution Centre Losses Report
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static List<DistributionCentreLosses> GetDistributionCentreLosses()
        {
            List<DistributionCentreLosses> centreList = new List<DistributionCentreLosses>();
            
            using (var ctx = new Entities())
            {
                centreList = ctx.DistributionCentreLosses.ToList();
            }
            /*
            string query = "select DistributionCentreId, DistributionCenterName, LossRatioNumerator, LossRatioDenominator, TotalLossDiscardedValue " +
                           "from DistributionCentreLosses " +
                            "order by DistributionCentreId";
            var cmd = new SqlCommand(query);
            cmd.Connection = connection;
            using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default))
            {
                while (reader.Read())
                {
                    var centre = new DistributionCentreLosses();
                    if (reader["DistributionCentreId"] != DBNull.Value)
                        centre.DistributionCentreId = Convert.ToInt32(reader["DistributionCentreId"]);
                    if (reader["DistributionCenterName"] != DBNull.Value)
                        centre.DistributionCenterName = (string)reader["DistributionCenterName"];
                    if (reader["LossRatioNumerator"] != DBNull.Value)
                        centre.LossRatioNumerator = Convert.ToInt32(reader["LossRatioNumerator"]);
                    if (reader["LossRatioDenominator"] != DBNull.Value)
                        centre.LossRatioDenominator = Convert.ToInt32(reader["LossRatioDenominator"]);
                    if (reader["TotalLossDiscardedValue"] != DBNull.Value)
                        centre.TotalLossDiscardedValue = Convert.ToDecimal(reader["TotalLossDiscardedValue"]);

                    centreList.Add(centre);
                }
            }
            */
            return centreList;
        }

        /// <summary>
        /// Query for Doctor Activity Report
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static List<DoctorActivity> GetDoctorActivity()
        {

            List<DoctorActivity> doctors = new List<DoctorActivity>();
            using (var ctx = new Entities())
            {
                doctors = ctx.DoctorActivity.ToList();
            }
            /*
            string query = "select DoctorId, DoctorName, PackageTypeId, PackageTypeDescription, PackageCount, TotalPackageValue " +
                "from DoctorActivity " +
                "order by DoctorId";
            var cmd = new SqlCommand(query);
            cmd.Connection = connection;
            using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default))
            {
                while (reader.Read())
                {
                    var doctor = new DoctorActivity();
                    if (reader["DoctorId"] != DBNull.Value)
                        doctor.DoctorId = Convert.ToInt32(reader["DoctorId"]);
                    if (reader["DoctorName"] != DBNull.Value)
                        doctor.DoctorName = (string)reader["DoctorName"];
                    if (reader["PackageTypeId"] != DBNull.Value)
                        doctor.PackageTypeId = Convert.ToInt32(reader["PackageTypeId"]);
                    if (reader["PackageTypeDescription"] != DBNull.Value)
                        doctor.PackageTypeDescription = (string)reader["PackageTypeDescription"];
                    if (reader["PackageCount"] != DBNull.Value)
                        doctor.PackageCount = Convert.ToInt32(reader["PackageCount"]);
                    if (reader["TotalPackageValue"] != DBNull.Value)
                        doctor.TotalPackageValue = Convert.ToDecimal(reader["TotalPackageValue"]);
                    doctors.Add(doctor);
                }
            }
             */
            return doctors;
        }

        /// <summary>
        /// Query for Global Stock Report
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static List<GlobalStock> GetGlobalStock()
        {
            List<GlobalStock> stocks = new List<GlobalStock>();
            using (var ctx = new Entities())
            {
                stocks = ctx.GlobalStock.ToList();
            }
            /*
            string query = "select PackageTypeId, PackageTypeDescription, CostPerPackage, NumberOfPackages, TotalValue " +
                "from GlobalStock " +
                "order by PackageTypeId";

            var cmd = new SqlCommand(query);
            cmd.Connection = connection;
            using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default))
            {
                while (reader.Read())
                {
                    var stock = new GlobalStock();
                    if (reader["PackageTypeId"] != DBNull.Value)
                        stock.PackageTypeId = Convert.ToInt32(reader["PackageTypeId"]);
                    if (reader["PackageTypeDescription"] != DBNull.Value)
                        stock.PackageTypeDescription = (string)reader["PackageTypeDescription"];
                    if (reader["CostPerPackage"] != DBNull.Value)
                        stock.CostPerPackage = Convert.ToDecimal(reader["CostPerPackage"]);
                    if (reader["NumberOfPackages"] != DBNull.Value)
                        stock.NumberOfPackages = Convert.ToInt32(reader["NumberOfPackages"]);
                    if (reader["TotalValue"] != DBNull.Value)
                        stock.TotalValue = Convert.ToDecimal(reader["TotalValue"]);
                    stocks.Add(stock);
                }
            }
            */
            return stocks;
        }

        /// <summary>
        /// Query for Value In Transit Report
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static List<ValueInTransit> GetValueInTransit()
        {
            List<ValueInTransit> valueList = new List<ValueInTransit>();
            using (var ctx = new Entities())
            {
                valueList = ctx.ValueInTransit.ToList();
            }
            /*
            string query = "select SenderDistributionCentreId, SenderDistributionCentreName, ReceiverDistributionCentreId, RecieverDistributionCentreName, TotalPackages, TotalValue " +
                "from ValueInTransit " +
                "order by SenderDistributionCentreId, ReceiverDistributionCentreId ";
            var cmd = new SqlCommand(query);
            cmd.Connection = connection;
            using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default))
            {
                while (reader.Read())
                {
                    var valueItem = new ValueInTransit();
                    if (reader["SenderDistributionCentreId"] != DBNull.Value)
                        valueItem.SenderDistributionCentreId = Convert.ToInt32(reader["SenderDistributionCentreId"]);
                    if (reader["SenderDistributionCentreName"] != DBNull.Value)
                        valueItem.SenderDistributionCentreName = (string)reader["SenderDistributionCentreName"];
                    if (reader["ReceiverDistributionCentreId"] != DBNull.Value)
                        valueItem.ReceiverDistributionCentreId = Convert.ToInt32(reader["ReceiverDistributionCentreId"]);
                    if (reader["RecieverDistributionCentreName"] != DBNull.Value)
                        valueItem.RecieverDistributionCentreName = (string)reader["RecieverDistributionCentreName"];
                    if (reader["TotalPackages"] != DBNull.Value)
                        valueItem.TotalPackages = Convert.ToInt32(reader["TotalPackages"]);
                    if (reader["TotalValue"] != DBNull.Value)
                        valueItem.TotalValue = Convert.ToDecimal(reader["TotalValue"]);
                    valueList.Add(valueItem);
                }
            }
            */
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
        public static List<ReconciledPackage> GetReconciledPackages(SqlConnection connection, DistributionCentre currentLocation, StandardPackageType packageType, XElement barCodeXml)
        {
            string query = "SELECT PackageId, BarCode, CurrentLocationCentreId, d.Name as CurrentLocationCentreName, CurrentStatus, 'INSTOCK' AS NewStatus " +
                            "FROM Package p " +
                            "INNER JOIN @BarCodeList.nodes('/Root/BarCode') AS Tbl(C) ON p.BarCode = Tbl.C.value('@Text', 'varchar(20)') " +
                            "LEFT OUTER JOIN DistributionCentre d ON d.CentreId = p.CurrentLocationCentreId " +
                            "WHERE p.PackageTypeId = @PackageTypeId AND " +
                                "(CurrentLocationCentreId <> @DistributionCentreId OR CurrentStatus <> 'INSTOCK') " +                            
                            "UNION ALL " +
                            "SELECT PackageId, BarCode, CurrentLocationCentreId, d.Name as CurrentLocationCentreName, CurrentStatus, 'LOST' AS NewStatus " +
                            "FROM Package p " +
                            "LEFT OUTER JOIN DistributionCentre d ON d.CentreId = p.CurrentLocationCentreId " +
                            "LEFT OUTER JOIN @BarCodeList.nodes('/Root/BarCode') AS Tbl(C) ON p.BarCode = Tbl.C.value('@Text', 'varchar(20)') " +
                            "WHERE Tbl.C.value('@Text', 'varchar(20)') IS NULL AND p.CurrentStatus = 'INSTOCK' AND p.PackageTypeId = @PackageTypeId AND p.CurrentLocationCentreId = @DistributionCentreId " +
                            "ORDER BY PackageId ";
            
            var packageList = new List<ReconciledPackage>();

            var cmd = new SqlCommand(query);
            cmd.Connection = connection;

            cmd.Parameters.Add("@BarCodeList", SqlDbType.Xml).Value = barCodeXml.ToString();
            cmd.Parameters.Add("@DistributionCentreId", SqlDbType.Int).Value = currentLocation.CentreId;            
            cmd.Parameters.Add("@PackageTypeId", SqlDbType.Int).Value = packageType.PackageTypeId;

            using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default))
            {
                while (reader.Read())
                {
                    var package = new ReconciledPackage();

                    if (reader["PackageId"] != DBNull.Value)
                        package.PackageId = Convert.ToInt32(reader["PackageId"]);
                    if (reader["BarCode"] != DBNull.Value)
                        package.BarCode = (string)reader["BarCode"];
                    if (reader["CurrentLocationCentreId"] != DBNull.Value)
                        package.CurrentLocationCentreId = Convert.ToInt32(reader["CurrentLocationCentreId"]);
                    if (reader["CurrentLocationCentreName"] != DBNull.Value)
                        package.CurrentLocationCentreName = (string)reader["CurrentLocationCentreName"];
                    if (reader["CurrentStatus"] != DBNull.Value)
                        package.CurrentStatus = (PackageStatus)Enum.Parse(typeof(PackageStatus), (string)reader["CurrentStatus"], true);
                    if (reader["NewStatus"] != DBNull.Value)
                        package.NewStatus = (PackageStatus)Enum.Parse(typeof(PackageStatus), (string)reader["NewStatus"], true);

                    packageList.Add(package);
                }
            }
            return packageList;
        }

        /// <summary>
        /// Query for Stocktaking Report
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="centreId"></param>
        /// <returns></returns>
        public static List<StockTaking> GetStocktaking(, int centreId)
        {
            List<StockTaking> stocks = new List<StockTaking>();
            using (var ctx = new Entities())
            {
                stocks = (from e in ctx.StockTaking
                          where e.CurrentLocationCentreId == centreId
                          select e).ToList();
            }
            /*
            string query = "select PackageId, BarCode, PackageTypeId, PackageTypeDescription, CostPerPackage, ExpirationDate " +
                "from StockTaking " +
                "where CurrentLocationCentreId = @LocationCentreId " +
                "order by PackageTypeId, ExpirationDate";
            var cmd = new SqlCommand(query);
            cmd.Connection = connection;

            cmd.Parameters.Add("@LocationCentreId", SqlDbType.Int).Value = centreId;

            using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default))
            {
                while (reader.Read())
                {                    
                    var stock = new StockTaking();
                    if (reader["PackageId"] != DBNull.Value)
                        stock.PackageId = Convert.ToInt32(reader["PackageId"]);
                    if (reader["BarCode"] != DBNull.Value)
                        stock.BarCode = Convert.ToString(reader["barcode"]);
                    if (reader["PackageTypeId"] != DBNull.Value)
                        stock.PackageTypeId = Convert.ToInt32(reader["PackageTypeId"]);
                    if (reader["PackageTypeDescription"] != DBNull.Value)
                        stock.PackageTypeDescription = (string)reader["PackageTypeDescription"];
                    if (reader["CostPerPackage"] != DBNull.Value)
                        stock.CostPerPackage = Convert.ToDecimal(reader["CostPerPackage"]);
                    if (reader["ExpirationDate"] != DBNull.Value)
                        stock.ExpirationDate = Convert.ToDateTime(reader["ExpirationDate"]);

                    stocks.Add(stock);
                }
            }
             */
            Debug.WriteLine("ViewDataAccess returns " + stocks.Count() + " items");
            return stocks;
        }
    }
}
