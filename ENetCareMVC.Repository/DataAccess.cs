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
        {            // define INSERT query with parameters 
            string query = "INSERT INTO dbo.Package (BarCode, ExpirationDate, PackageTypeId, CurrentLocationCentreId, CurrentStatus, DistributedByEmployeeId) " +
                           "VALUES (@BarCode, @ExpirationDate, @PackageTypeId, @CurrentLocationCentreId, @CurrentStatus, @DistributedByEmployeeId) " +
                           "SET @newId = SCOPE_IDENTITY();";

            using (var cmd = new SqlCommand(query))
            {                // define parameters and their values 
                cmd.Parameters.Add("@BarCode", SqlDbType.VarChar, 20).Value = package.BarCode ?? string.Empty;
                cmd.Parameters.Add("@ExpirationDate", SqlDbType.DateTime).Value = package.ExpirationDate;
                cmd.Parameters.Add("@PackageTypeId", SqlDbType.Int).Value = package.PackageType.PackageTypeId;
                cmd.Parameters.Add("@CurrentLocationCentreId", SqlDbType.Int).Value = package.CurrentLocation.CentreId;
                cmd.Parameters.Add("@CurrentStatus", SqlDbType.VarChar, 20).Value = package.CurrentStatus.ToString().ToUpper();
                if (package.DistributedBy == null)
                    cmd.Parameters.Add("@DistributedByEmployeeId", SqlDbType.Int).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@DistributedByEmployeeId", SqlDbType.Int).Value = package.DistributedBy.EmployeeId;
                cmd.Parameters.Add("@newId", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.CommandType = CommandType.Text;

                string qry = cmd.CommandText;

                cmd.ExecuteScalar();

                return (int)cmd.Parameters["@newId"].Value;
            }
        }

        public static void UpdatePackage(Package package)
        {
            string cmdStr = "UPDATE dbo.Package SET BarCode = @BarCode, " +
                                "CurrentLocationCentreId = @CurrentLocationCentreId, " +
                                "CurrentStatus = @CurrentStatus, " +
                                "DistributedByEmployeeId = @DistributedByEmployeeId " +
                                "WHERE PackageId = @PackageId";

            using (var cmd = new SqlCommand(cmdStr))
            {
                cmd.Parameters.AddWithValue("@BarCode", package.BarCode);
                cmd.Parameters.AddWithValue("@CurrentLocationCentreId", package.CurrentLocation == null ? DBNull.Value : (object)package.CurrentLocation.CentreId);
                cmd.Parameters.AddWithValue("@CurrentStatus", package.CurrentStatus.ToString().ToUpper());
                cmd.Parameters.AddWithValue("@DistributedByEmployeeId", package.DistributedBy == null ? DBNull.Value : (object)package.DistributedBy.EmployeeId);
                cmd.Parameters.AddWithValue("@PackageId", package.PackageId);

                int effected = cmd.ExecuteNonQuery();
            }
        }

        public static void UpdateTransit(PackageTransit transit)
        {
            string cmdStr = "UPDATE dbo.PackageTransit SET Package = @Package, SenderCentre = @SenderId, " +
                                "ReceiverCentre = @ReceiverCentreId, DateSent = @DateSent , " +
                                " DateReceived = @DateReceived, DateCancelled = @DateCancelled " +
                                "WHERE TransitId = @TransitId ";
            using (var cmd = new SqlCommand(cmdStr))
            {
                cmd.Parameters.AddWithValue("@Package", SqlDbType.Int).Value = (int)transit.Package.PackageId;
                cmd.Parameters.AddWithValue("@SenderCentre", SqlDbType.Int).Value = (int)transit.SenderCentre.CentreId;
                cmd.Parameters.AddWithValue("@ReceiverCentre", SqlDbType.Int).Value = (int)transit.ReceiverCentre.CentreId;
                cmd.Parameters.AddWithValue("@DateSent", SqlDbType.DateTime).Value = (DateTime)transit.DateSent;
                cmd.Parameters.AddWithValue("@DateReceived", SqlDbType.DateTime).Value = (DateTime)transit.DateReceived;
                cmd.Parameters.AddWithValue("@DateCancelled", SqlDbType.DateTime).Value = (DateTime)transit.DateCancelled;
                int effected = cmd.ExecuteNonQuery();
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

                employeeRecord.UserName = employee.UserName;
                employeeRecord.FullName = employee.FullName;
                employeeRecord.UserId = employee.UserId;
                employeeRecord.EmailAddress = employee.EmailAddress;
                employeeRecord.LocationCentreId = employee.LocationCentreId;
                employeeRecord.EmployeeType = employee.EmployeeType;

                ctx.SaveChanges();
            }
        }

        public static Package GetPackage(int? packageId, string barcode = null)
        {
            Package package = null;

            string query = "SELECT PackageId, BarCode, ExpirationDate, PackageTypeId, CurrentLocationCentreId, CurrentStatus, DistributedByEmployeeId FROM Package WHERE PackageId = ISNULL(@packageId, PackageId) AND BarCode = ISNULL(@barcode, BarCode)";

            var cmd = new SqlCommand(query);
            cmd.Connection = null;

            cmd.Parameters.AddWithValue("@packageId", packageId.HasValue ? packageId.Value : (object)DBNull.Value);

            cmd.Parameters.AddWithValue("@barcode", string.IsNullOrEmpty(barcode) ? (object)DBNull.Value : barcode);

            using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default))
            {
                if (reader.Read())
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
                }
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
            string query = "SELECT PackageTypeId, Description, NumberOfMedications, ShelfLifeUnitType, ShelfLifeUnits, TemperatureSensitive, Value FROM StandardPackageType WHERE PackageTypeId = @packageTypeId";

            var cmd = new SqlCommand(query);
            cmd.Connection = null;

            cmd.Parameters.AddWithValue("@packageTypeId", packageTypeId);

            using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default))
            {
                if (reader.Read())
                {
                    packageType = new StandardPackageType();

                    packageType.PackageTypeId = Convert.ToInt32(reader["PackageTypeId"]);
                    packageType.Description = (string)reader["Description"];
                    packageType.NumberOfMedications = Convert.ToInt32(reader["NumberOfMedications"]);
                    packageType.ShelfLifeUnitType = (ShelfLifeUnitType)Enum.Parse(typeof(ShelfLifeUnitType), (string)reader["ShelfLifeUnitType"], true);
                    packageType.ShelfLifeUnits = Convert.ToInt32(reader["ShelfLifeUnits"]);
                    packageType.TemperatureSensitive = (bool)reader["TemperatureSensitive"];
                    packageType.Value = (decimal)reader["Value"];
                }
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

            string query = "SELECT CentreId, Name, Address, Phone, IsHeadOffice FROM DistributionCentre ORDER BY CentreId";

            var cmd = new SqlCommand(query);
            //cmd.Connection = connection;

            using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default))
            {
                while (reader.Read())
                {
                    var centre = new DistributionCentre();

                    centre.CentreId = Convert.ToInt32(reader["CentreId"]);
                    centre.Name = (string)reader["Name"];
                    centre.Address = (string)reader["Address"];
                    centre.Phone = (string)reader["Phone"];
                    centre.IsHeadOffice = (bool)reader["IsHeadOffice"];

                    centres.Add(centre);
                }
            }

            return centres;
        }

        public static List<StandardPackageType> GetAllStandardPackageTypes()
        {
            var packageTypes = new List<StandardPackageType>();
            string query = "SELECT PackageTypeId, Description, NumberOfMedications, ShelfLifeUnitType, ShelfLifeUnits, TemperatureSensitive, Value FROM StandardPackageType ORDER BY PackageTypeId";

            var cmd = new SqlCommand(query);
            cmd.Connection = null;

            using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default))
            {
                while (reader.Read())
                {
                    var packageType = new StandardPackageType();

                    packageType.PackageTypeId = Convert.ToInt32(reader["PackageTypeId"]);
                    packageType.Description = (string)reader["Description"];
                    packageType.NumberOfMedications = Convert.ToInt32(reader["NumberOfMedications"]);
                    packageType.ShelfLifeUnitType = (ShelfLifeUnitType)Enum.Parse(typeof(ShelfLifeUnitType), (string)reader["ShelfLifeUnitType"], true);
                    packageType.ShelfLifeUnits = Convert.ToInt32(reader["ShelfLifeUnits"]);
                    packageType.TemperatureSensitive = (bool)reader["TemperatureSensitive"];
                    packageType.Value = (decimal)reader["Value"];

                    packageTypes.Add(packageType);
                }
            }

            return packageTypes;
        }

        public static List<Package> GetAllPackages(DistributionCentre Location = null)
        {                                                          // Added by Pablo on 24-03-15
            Package package = null;
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
        
        public static int InsertPackageTransit(PackageTransit packageT)
        {                                                                       // (p. 24/03/15 ) 
            string query = " INSERT INTO dbo.PackageTransit (PackageId , SenderCentreId,  " +
                           " ReceiverCentreId, DateSent, DateReceived, DateCancelled)  " +
                           " VALUES (@Package , @SenderCentre, @ReceiverCentre, @DateSent, @DateReceived, @DateCancelled)" +
                           " SET @newId = SCOPE_IDENTITY();";
            //var cmd = new SqlCommand(query);
            //cmd.Connection = connection;
            using (var cmd = new SqlCommand(query)/*SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default)*/)
            {
                cmd.Parameters.Add("@Package", SqlDbType.Int).Value = (int)packageT.Package.PackageId;
                cmd.Parameters.Add("@SenderCentre", SqlDbType.Int).Value = (int)packageT.SenderCentre.CentreId;
                cmd.Parameters.Add("@ReceiverCentre", SqlDbType.Int).Value = (int)packageT.ReceiverCentre.CentreId;
                cmd.Parameters.Add("@DateSent", SqlDbType.DateTime).Value = (DateTime)packageT.DateSent;
                if (packageT.DateReceived == null)
                    cmd.Parameters.Add("@DateReceived", SqlDbType.DateTime).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@DateReceived", SqlDbType.DateTime).Value = (DateTime)packageT.DateReceived;
                if (packageT.DateCancelled == null)
                    cmd.Parameters.Add("@DateCancelled", SqlDbType.DateTime).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@DateCancelled", SqlDbType.DateTime).Value = (DateTime)packageT.DateCancelled;
                cmd.Parameters.Add("@newId", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.CommandType = CommandType.Text;
                string qry = cmd.CommandText;
                int effect = cmd.ExecuteNonQuery();
                //cmd.ExecuteScalar();
                return (int)cmd.Parameters["@newId"].Value;
            }
        }

        public static void UpdatePackageTransit(PackageTransit transit)
        {            // Define Insert Query with Parameter
            string cmdStr = "UPDATE dbo.PackageTransit " +
                           "SET PackageId = @PackageId, " +
                                "SenderCentreId = @SenderCentreId, " +
                                "ReceiverCentreId = @ReceiverCentreId, " +
                                "DateSent = @DateSent, " +
                                "DateReceived = @DateReceived," +
                                "DateCancelled = @DateCancelled " +
                            " WHERE TransitId = @TransitId";

            using (var cmd = new SqlCommand(cmdStr))
            {
                cmd.Parameters.AddWithValue("@PackageId", transit.Package.PackageId);
                cmd.Parameters.AddWithValue("@SenderCentreId", transit.SenderCentre.CentreId);
                cmd.Parameters.AddWithValue("@ReceiverCentreId", transit.ReceiverCentre.CentreId);
                cmd.Parameters.AddWithValue("@DateSent", transit.DateSent);
                cmd.Parameters.AddWithValue("@DateReceived", transit.DateReceived == null ? DBNull.Value : (object)transit.DateReceived.Value);
                cmd.Parameters.AddWithValue("@DateCancelled", transit.DateCancelled == null ? DBNull.Value : (object)transit.DateCancelled.Value);
                cmd.Parameters.AddWithValue("@TransitId", transit.TransitId);

                int effected = cmd.ExecuteNonQuery();
            }
        }


        public static List<PackageTransit> GetAllPackageTransits()
        {                                                       // (P. 04/04/2015)
            var allTransits = new List<PackageTransit>();
            string query = "SELECT TransitId, PackageId, SenderCentreId, ReceiverCentreId, DateSent, DateReceived, DateCancelled FROM PackageTransit ORDER BY TransitId";
            var cmd = new SqlCommand(query);
            cmd.Connection = null;

            //Console.WriteLine(query);            //string a = Console.ReadLine();

            using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default))
            {
                while (reader.Read())
                {
                    //Console.WriteLine(reader["transitId"]);               //Console.ReadLine();
                    var transit = new PackageTransit();
                    transit = new PackageTransit();
                    transit.TransitId = Convert.ToInt32(reader["transitId"]);
                    transit.Package = DataAccess.GetPackage(Convert.ToInt32(reader["PackageId"]));
                    // .PackageId=Convert.ToInt32(reader["PackageId"]);
                    transit.SenderCentre = DataAccess.GetDistributionCentre(Convert.ToInt32(reader["SenderCentreId"]));
                    //transit.SenderCentre.CentreId = Convert.ToInt32(reader["SenderCentreId"]);
                    transit.ReceiverCentre = DataAccess.GetDistributionCentre(Convert.ToInt32(reader["ReceiverCentreId"]));
                    // .CentreId = Convert.ToInt32(reader["ReceiverCentreId"]);
                    transit.DateSent = Convert.ToDateTime(reader["DateSent"]);
                    if (reader["DateReceived"] != DBNull.Value)
                    {
                        transit.DateReceived = Convert.ToDateTime(reader["DateReceived"]);
                    }
                    if (reader["DateCancelled"] != DBNull.Value)
                    {
                        transit.DateReceived = Convert.ToDateTime(reader["DateCancelled"]);
                    }

                    allTransits.Add(transit);
                }
            }
            return allTransits;
        }



        public static PackageTransit GetPackageTransit(Package package, DistributionCentre receiver)
        {
            PackageTransit packageTransit = null;
            // Define Update Query with Parameter
            string query = "SELECT TransitId, PackageId, SenderCentreId, ReceiverCentreId, " +
                              "DateSent, DateReceived, DateCancelled " +
                            "FROM dbo.PackageTransit " +
                            "WHERE PackageId = @PackageId and " +
                              "ReceiverCentreId = ISNULL(@ReceiverCentreId, ReceiverCentreId) " +
                              "and DateReceived is null and DateCancelled is null";

            var cmd = new SqlCommand(query);
            cmd.Connection = null;

            cmd.Parameters.AddWithValue("@PackageId", package.PackageId);
            cmd.Parameters.AddWithValue("@ReceiverCentreId", receiver == null ? DBNull.Value : (object)receiver.CentreId);

            using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default))
            {
                if (reader.Read())
                {
                    packageTransit = new PackageTransit();
                    packageTransit.TransitId = Convert.ToInt32(reader["TransitId"]);

                    packageTransit.Package = new Package();
                    packageTransit.Package.PackageId = Convert.ToInt32(reader["PackageId"]);

                    packageTransit.SenderCentre = new DistributionCentre();
                    packageTransit.SenderCentre.CentreId = Convert.ToInt32(reader["SenderCentreId"]);

                    packageTransit.ReceiverCentre = new DistributionCentre();
                    packageTransit.ReceiverCentre.CentreId = Convert.ToInt32(reader["ReceiverCentreId"]);
                    packageTransit.DateSent = Convert.ToDateTime(reader["DateSent"]);
                    if (reader["DateReceived"] != DBNull.Value)
                    {
                        packageTransit.DateReceived = Convert.ToDateTime(reader["DateReceived"]);
                    }
                    if (reader["DateCancelled"] != DBNull.Value)
                    {
                        packageTransit.DateCancelled = Convert.ToDateTime(reader["DateCancelled"]);
                    }
                }
            }

            return packageTransit;
        }

        public static int InsertAudit(Employee employee, StandardPackageType packageType)
        {            // define INSERT query with parameters 
            string query = "INSERT Audit (DateAudited, DistributionCentreId, EmployeeId, PackageTypeId) " +
                            "VALUES (@DateAudited, @DistributionCentreId, @EmployeeId, @PackageTypeId);  " +
                           "SET @newId = SCOPE_IDENTITY();";

            using (var cmd = new SqlCommand(query))
            {                // define parameters and their values 
                cmd.Parameters.Add("@DateAudited", SqlDbType.DateTime).Value = DateTime.Today;
                cmd.Parameters.Add("@DistributionCentreId", SqlDbType.Int).Value = employee.Location.CentreId;
                cmd.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = employee.EmployeeId;
                cmd.Parameters.Add("@PackageTypeId", SqlDbType.Int).Value = packageType.PackageTypeId;

                cmd.Parameters.Add("@newId", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.CommandType = CommandType.Text;

                string qry = cmd.CommandText;

                cmd.ExecuteScalar();

                return (int)cmd.Parameters["@newId"].Value;
            }
        }

        public static void InsertAuditPackages(int auditId, StandardPackageType packageType, XElement barCodeXml)
        {            // define INSERT query with parameters 
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
