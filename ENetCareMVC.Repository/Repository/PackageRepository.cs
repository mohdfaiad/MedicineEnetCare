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
    public class PackageRepository : IPackageRepository
    {
        private string _connectionString;
        
        public PackageRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int Insert(Package package)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return DataAccess.InsertPackage(connection, package);
            }
        }

        public void Update(Package package)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                DataAccess.UpdatePackage(connection, package);
            }
            return;
        }

        public void UpdateTransit(PackageTransit transit)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                DataAccess.UpdatePackageTransit(connection, transit);
            }
            return;
        }
      

        public Package Get(int? packageId, string barcode)
        {
            Package package = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                package = DataAccess.GetPackage(connection, packageId, barcode);
                if (package == null)
                    return null;

                package.PackageType = DataAccess.GetStandardPackageType(connection, package.PackageType.PackageTypeId);

                if (package.CurrentLocation != null)
                {
                    package.CurrentLocation = DataAccess.GetDistributionCentre(connection, package.CurrentLocation.CentreId);
                }

                if (package.DistributedBy != null)
                {
                    package.DistributedBy = DataAccess.GetEmployee(connection, package.DistributedBy.EmployeeId, null);
                    package.DistributedBy.Location = DataAccess.GetDistributionCentre(connection, package.DistributedBy.Location.CentreId);
                }
            }
            return package;
        }

        public Package GetPackageWidthBarCode(string barCode)                           // Added by Pablo on 24-03-15
        { 
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return DataAccess.GetPackage(connection, null, barCode);                
            }
            return null;     
        }

        public List<StandardPackageType> GetAllStandardPackageTypes()
        {
            List<StandardPackageType> packageTypes = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                packageTypes = DataAccess.GetAllStandardPackageTypes(connection);
            }
            return packageTypes;
        }

        public StandardPackageType GetStandardPackageType(int packageId)
        {
            StandardPackageType packageTypes = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                packageTypes = DataAccess.GetStandardPackageType(connection, packageId);
            }
            return packageTypes;
        }


        public string getConnectionString() { return _connectionString; }

        public int InsertTransit(PackageTransit packageTransit)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return DataAccess.InsertPackageTransit(connection, packageTransit);
            }
        }

       
        public PackageTransit GetTransit(Package package, DistributionCentre receiver)
        {
            PackageTransit packageTransit = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                packageTransit = DataAccess.GetPackageTransit(connection, package, receiver);
              
                if (packageTransit == null)
                    return null;
            }
            return packageTransit;
        }

        public DistributionCentre GetHeadOffice()
        {                                                               // (P. 05-04-2015)
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                List<DistributionCentre> allCentres = DataAccess.GetAllDistributionCentres(connection);
                connection.Close();
                foreach (DistributionCentre centre in allCentres)
                    if (centre.IsHeadOffice) return centre;
            }   
            return null;
        }

        public int InsertAudit(Employee employee, StandardPackageType packageType, List<string> barCodes)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                int auditId = DataAccess.InsertAudit(connection, employee, packageType);

                XElement barCodeXml = barCodes.GetBarCodeXML();

                DataAccess.InsertAuditPackages(connection, auditId, packageType, barCodeXml);
                return auditId;
            }
        }

        public int UpdateLostFromAudit(int auditId, DistributionCentre location, StandardPackageType packageType)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return DataAccess.UpdateLostFromAudit(connection, auditId, location, packageType);
            }
        }

        public int UpdateInstockFromAudit(int auditId, DistributionCentre location, StandardPackageType packageType)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return DataAccess.UpdateInstockFromAudit(connection, auditId, location, packageType);
            }
        }

        public int UpdateTransitReceivedFromAudit(int auditId, DistributionCentre location)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return DataAccess.UpdateTransitReceivedFromAudit(connection, auditId, location);
            }
        }

        public int UpdateTransitCancelledFromAudit(int auditId, DistributionCentre location)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return DataAccess.UpdateTransitCancelledFromAudit(connection, auditId, location);
            }
        }

        // *************************************************************************

        public List<PackageTransit> GetAllPackageTransits()
        {                                                                        //   Added by Pablo on 23-03-15
            List<PackageTransit> allTransits = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                allTransits = DataAccess.GetAllPackageTransits(connection);
            }
            return allTransits;
        }


        public List<PackageTransit> GetActiveTransitsByPackage(Package xPackage)     // Added by Pablo on 23-03-15
        {
            List<PackageTransit> allTransits = GetAllPackageTransits();          //.this.GetAllPackageTransits();
            List<PackageTransit> myTransits = new List<PackageTransit>();        // create empty list
            foreach (PackageTransit t in allTransits)
            {
                if (t.Package == xPackage && t.DateReceived == null && t.DateCancelled == null) myTransits.Add(t);
            }
            return myTransits;
        }

          



    }
}
