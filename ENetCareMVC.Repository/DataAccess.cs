using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Xml.Linq;
using ENetCareMVC.Repository.Data;

namespace ENetCareMVC.Repository
{
    public class DataAccess
    {

        Dictionary<int, Employee> mockEmployeeDb = new Dictionary<int, Employee>();

        public static int InsertPackage(Package package)
        {
            using (var ctx = new Entities()) {
                    ctx.Package.Add(package);
                    ctx.SaveChanges();

                    return package.PackageId;
            }
        }

        public static void UpdatePackage(Package package)
        {
            using (var ctx = new Entities())
            {
                var packageRecord = (from e in ctx.Package
                                      where e.PackageId == package.PackageId
                                      && e.BarCode == package.BarCode
                                      select e).First();

                packageRecord = package;
                ctx.SaveChanges();
            }
        }

        public static void UpdateTransit(PackageTransit transit)
        {
            using (var ctx = new Entities())
            {
                var packageTransitRecord = (from e in ctx.PackageTransit
                                            where e.TransitId == transit.TransitId
                                            select e).First();

                packageTransitRecord = transit;
                ctx.SaveChanges();
            }
        }


        public static void UpdateEmployee(Employee employee)
        {
            using (var ctx = new Entities())
            {
                var employeeRecord = (from e in ctx.Employee
                                      where e.UserName == employee.UserName
                                      && e.EmployeeId == employee.EmployeeId
                                      select e).First();

                employeeRecord = employee;
                ctx.SaveChanges();
            }
        }

        public static Package GetPackage(int? packageId, string barcode = null)
        {
            Package package = null;

            using (var ctx = new Entities())
            {
                var packageRecord = (from e in ctx.Package
                                     where e.PackageId == (packageId.HasValue ? packageId.Value : (int?)null)
                                     && e.BarCode == (string.IsNullOrEmpty(barcode) ? (object)DBNull.Value : barcode)
                                     select e).First();
                package = new Package();
                package = packageRecord;
            }
            return package;
        }
        
        public static Employee GetEmployee(int? employeeId, string username)
        {
            Employee employee = null;

            using (var ctx = new Entities())
            {
                var employeeRecord = (from e in ctx.Employee
                                      where e.EmployeeId == employeeId && e.UserName == username
                                      select e).First();
                employee = new Employee();
                employee = employeeRecord;
            }

            return employee;
        }

        public static DistributionCentre GetDistributionCentre(int centreId)
        {
            DistributionCentre centre = null;

            using (var ctx = new Entities())
            {
                var centreRecord = (from dc in ctx.DistributionCentre
                                    where dc.CentreId == centreId
                                    select dc).First();

                centre = new DistributionCentre();
                centre = centreRecord;
            }

            return centre;
        }

        public static StandardPackageType GetStandardPackageType(int packageTypeId)
        {
            StandardPackageType packageType = null;

            using (var ctx = new Entities())
            {
                var packageTypeRecord = (from e in ctx.StandardPackageType
                                     where e.PackageTypeId == packageTypeId
                                     select e).First();
                packageType = new StandardPackageType();
                packageType = packageTypeRecord;
            }

            return packageType;
        }

        public static List<DistributionCentre> GetAllDistributionCentres()
        {
            List<DistributionCentre> centres = new List<DistributionCentre>();

            using (var ctx = new Entities())
            {
                centres = ctx.DistributionCentre.ToList();
            }

            return centres;
        }

        public static List<StandardPackageType> GetAllStandardPackageTypes()
        {
            var packageTypes = new List<StandardPackageType>();
            using (var ctx = new Entities())
            {
                packageTypes = ctx.StandardPackageType.ToList();
            }

            return packageTypes;
        }

        public static List<Package> GetAllPackages(DistributionCentre Location = null)
        {                                                          // Added by Pablo on 24-03-15
            Package package = null;

            using (var ctx = new Entities())
            {
                // I Faced a little problem here, could you take a look at this please, Ben?
            }

            string query = "SELECT PackageId, BarCode, ExpirationDate, PackageTypeId, CurrentLocationCentreId, CurrentStatus, DistributedByEmployeeId FROM Package ";
            if (Location != null) query += " WHERE CurrentLocationCenterId=" + Location.CentreId;

            var cmd = new SqlCommand(query);
            List<Package> allPackages = new List<Package>();
            cmd.Connection = null;
            using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default))
            {
                while (reader.Read())
                {
                    package = new Package();
                    package.PackageType = new StandardPackageType();
                    package.PackageId = Convert.ToInt32(reader["PackageId"]);
                    package.BarCode = (string)reader["BarCode"];
                    package.ExpirationDate = (DateTime)reader["ExpirationDate"];
                    package.PackageType.PackageTypeId = Convert.ToInt32(reader["PackageTypeId"]);
                    if (reader["CurrentLocationCentreId"] != DBNull.Value)
                    {
                        package.CurrentLocation = new DistributionCentre();
                        package.CurrentLocation.CentreId = Convert.ToInt32(reader["CurrentLocationCentreId"]);
                    }
                    package.CurrentStatus = (PackageStatus)Enum.Parse(typeof(PackageStatus), (string)reader["CurrentStatus"], true);
                    if (reader["DistributedByEmployeeId"] != DBNull.Value)
                    {
                        package.DistributedBy = new Employee();
                        package.DistributedBy.EmployeeId = Convert.ToInt32(reader["DistributedByEmployeeId"]);
                    }
                    allPackages.Add(package);
                }
            }
            return allPackages;
        }


        public static List<Employee> GetAllEmployees()
        {
            List<Employee> allEmployees = new List<Employee>();

            using (var ctx = new Entities())
            {
                allEmployees = ctx.Employee.ToList();
            }
            return allEmployees;
        }
        
        public static int InsertPackageTransit(PackageTransit packageTransit)
        {

            using (var ctx = new Entities())
            {
                ctx.PackageTransit.Add(packageTransit);

                return packageTransit.TransitId;
            }
        }

        public static void UpdatePackageTransit(PackageTransit transit)
        {            // Define Insert Query with Parameter
            using (var ctx = new Entities())
            {
                var packageTransitRecord = (from e in ctx.PackageTransit
                                        where e.TransitId == transit.TransitId
                                        select e).First();

                packageTransitRecord = transit;

                ctx.SaveChanges();
            }
        }


        public static List<PackageTransit> GetAllPackageTransits()
        {                                                       // (P. 04/04/2015)
            var allTransits = new List<PackageTransit>();
            using (var ctx = new Entities())
            {
                allTransits = ctx.PackageTransit.ToList();
            }

            return allTransits;
        }

        public static PackageTransit GetPackageTransit(Package package, DistributionCentre receiver)
        {
            PackageTransit packageTransit = null;
            // Define Update Query with Parameter

            using (var ctx = new Entities())
            {
                var packageTransitRecord = (from e in ctx.PackageTransit
                                            where e.PackageId == package.PackageId
                                            && e.ReceiverCentreId == (receiver == null ? (int?)null : receiver.CentreId)
                                            && e.DateReceived == null
                                            && e.DateCancelled == null
                                            select e).First();

                packageTransit = new PackageTransit();
                packageTransit = packageTransitRecord;
            }

            return packageTransit;
        }

        public static int InsertAudit(Employee employee, StandardPackageType packageType)
        {            // define INSERT query with parameters 
            using (var ctx = new Entities())
            {
                Audit audit = new Audit();
                audit.DateAudited = DateTime.Today;
                audit.DistributionCentreId = employee.Location.CentreId;
                audit.EmployeeId = employee.EmployeeId;
                audit.PackageTypeId = packageType.PackageTypeId;
                
             // I Don't know how to do the "SET @newId = SCOPE_IDENTITY()" part in the following query
                
             /*string query = "INSERT Audit (DateAudited, DistributionCentreId, EmployeeId, PackageTypeId) " +
                            "VALUES (@DateAudited, @DistributionCentreId, @EmployeeId, @PackageTypeId);  " +
                           "SET @newId = SCOPE_IDENTITY();";*/

                ctx.Audit.Add(audit);

                return audit.AuditId;
            }
        }

        public static void InsertAuditPackages(int auditId, StandardPackageType packageType, XElement barCodeXml)
        {            // define INSERT query with parameters 
            using (var ctx = new Entities())
            {
                //This is a tough one for me, I couldn't comprehand this one, not goot at sql queries
            }

            string query = "INSERT AuditPackage (AuditId, PackageId) " +
                            "SELECT @AuditId, p.PackageId " +
                            "FROM Package p " +
                            "INNER JOIN @BarcodeList.nodes('/Root/BarCode') AS Tbl(C) ON p.BarCode = Tbl.C.value('@Text', 'varchar(20)') " +
                            "WHERE p.PackageTypeId = @PackageTypeId";

            using (var cmd = new SqlCommand(query))
            {                // define parameters and their values 
                cmd.Parameters.Add("@BarCodeList", SqlDbType.Xml).Value = barCodeXml.ToString();
                cmd.Parameters.Add("@AuditId", SqlDbType.Int).Value = auditId;
                cmd.Parameters.Add("@PackageTypeId", SqlDbType.Int).Value = packageType.PackageTypeId;

                cmd.ExecuteNonQuery();
            }
        }

        public static int UpdateLostFromAudit(int auditId, DistributionCentre location, StandardPackageType packageType)
        {            // define INSERT query with parameters 
            using (var ctx = new Entities())
            {
                //This is a tough one for me, I couldn't comprehand this one, not goot at sql queries
            }
            string query = "UPDATE Package SET CurrentStatus = 'LOST' " +
                            "FROM Package p " +
                            "LEFT OUTER JOIN AuditPackage a ON a.PackageId = p.PackageId AND a.AuditId = @AuditId " +
                            "WHERE a.PackageId IS NULL AND p.CurrentStatus = 'INSTOCK' AND p.PackageTypeId = @PackageTypeId AND p.CurrentLocationCentreId = @DistributionCentreId ";

            using (var cmd = new SqlCommand(query))
            {                // define parameters and their values                 
                cmd.Parameters.Add("@DistributionCentreId", SqlDbType.Int).Value = location.CentreId;
                cmd.Parameters.Add("@AuditId", SqlDbType.Int).Value = auditId;
                cmd.Parameters.Add("@PackageTypeId", SqlDbType.Int).Value = packageType.PackageTypeId;

                cmd.CommandType = CommandType.Text;

                return cmd.ExecuteNonQuery();
            }
        }

        public static int UpdateInstockFromAudit(int auditId, DistributionCentre location, StandardPackageType packageType)
        {            // define INSERT query with parameters 

            using (var ctx = new Entities())
            {
                //This is a tough one for me, I couldn't comprehand this one, not goot at sql queries
            }

            string query = "UPDATE Package SET CurrentStatus = 'INSTOCK', CurrentLocationCentreId = @DistributionCentreId, DistributedByEmployeeId = null " +
                            "FROM Package p " +
                            "INNER JOIN AuditPackage a ON a.PackageId = p.PackageId AND a.AuditId = @AuditId " +
                            "WHERE p.PackageTypeId = @PackageTypeId AND " +
                                "(p.CurrentLocationCentreId <> @DistributionCentreId OR CurrentStatus <> 'INSTOCK') ";
            using (var cmd = new SqlCommand(query))
            {                // define parameters and their values                 
                cmd.Parameters.Add("@DistributionCentreId", SqlDbType.Int).Value = location.CentreId;
                cmd.Parameters.Add("@AuditId", SqlDbType.Int).Value = auditId;
                cmd.Parameters.Add("@PackageTypeId", SqlDbType.Int).Value = packageType.PackageTypeId;

                cmd.CommandType = CommandType.Text;

                return cmd.ExecuteNonQuery();
            }
        }

        public static int UpdateTransitReceivedFromAudit(int auditId, DistributionCentre location)
        {            // define INSERT query with parameters 
            using (var ctx = new Entities())
            {
                //This is a tough one for me, I couldn't comprehand this one, not goot at sql queries
            }

            string query = "UPDATE PackageTransit SET DateReceived = a.DateAudited " +
                            "FROM PackageTransit pt " +
                            "INNER JOIN AuditPackage ap ON pt.PackageId = ap.PackageId " +
                            "INNER JOIN Audit a ON ap.AuditId = a.AuditId " +
                            "WHERE a.AuditId = @AuditId AND pt.ReceiverCentreId = @DistributionCentreId AND pt.DateReceived IS null AND pt.DateCancelled IS null ";

            using (var cmd = new SqlCommand(query))
            {                // define parameters and their values                 
                cmd.Parameters.Add("@DistributionCentreId", SqlDbType.Int).Value = location.CentreId;
                cmd.Parameters.Add("@AuditId", SqlDbType.Int).Value = auditId;

                cmd.CommandType = CommandType.Text;

                return cmd.ExecuteNonQuery();
            }
        }

        public static int UpdateTransitCancelledFromAudit(int auditId, DistributionCentre location)
        {            // define INSERT query with parameters 

            using (var ctx = new Entities())
            {
                //This is a tough one for me, I couldn't comprehand this one, not goot at sql queries
            }
            string query = "UPDATE PackageTransit SET DateCancelled = a.DateAudited " +
                            "FROM PackageTransit pt " +
                            "INNER JOIN AuditPackage ap ON pt.PackageId = ap.PackageId " +
                            "INNER JOIN Audit a ON ap.AuditId = a.AuditId " +
                            "WHERE a.AuditId = @AuditId AND pt.ReceiverCentreId <> @DistributionCentreId AND pt.DateReceived IS null AND pt.DateCancelled IS null ";

            using (var cmd = new SqlCommand(query))
            {                // define parameters and their values                 
                cmd.Parameters.Add("@DistributionCentreId", SqlDbType.Int).Value = location.CentreId;
                cmd.Parameters.Add("@AuditId", SqlDbType.Int).Value = auditId;

                cmd.CommandType = CommandType.Text;

                return cmd.ExecuteNonQuery();
            }
        }
    }
}
