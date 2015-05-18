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
    public class PackageRepository : IPackageRepository
    {
        private string _connectionString;

        public PackageRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int Insert(Package package)
        {

            return DataAccess.InsertPackage(_connectionString, package);
        }

        public void Update(Package package)
        {

            DataAccess.UpdatePackage(_connectionString, package);
        }

        public void UpdateTransit(PackageTransit transit)
        {

            DataAccess.UpdatePackageTransit(_connectionString, transit);
        }


        public Package Get(int? packageId, string barcode)
        {
            Package package = null;

            package = DataAccess.GetPackage(_connectionString, packageId, barcode);
            if (package == null)
                return null;

            package.PackageType = DataAccess.GetStandardPackageType(_connectionString, package.PackageType.PackageTypeId);

            if (package.CurrentLocation != null)
            {
                package.CurrentLocation = DataAccess.GetDistributionCentre(_connectionString, package.CurrentLocation.CentreId);
            }

            if (package.DistributedBy != null)
            {
                package.DistributedBy = DataAccess.GetEmployee(_connectionString, package.DistributedBy.EmployeeId, null);
                package.DistributedBy.Location = DataAccess.GetDistributionCentre(_connectionString, package.DistributedBy.Location.CentreId);
            }

            return package;
        }

        public Package GetPackageWidthBarCode(string barCode)                           // Added by Pablo on 24-03-15
        {

            return DataAccess.GetPackage(_connectionString, null, barCode);

            return null;
        }

        public List<StandardPackageType> GetAllStandardPackageTypes()
        {
            List<StandardPackageType> packageTypes = null;

            packageTypes = DataAccess.GetAllStandardPackageTypes(_connectionString);

            return packageTypes;
        }

        public StandardPackageType GetStandardPackageType(int packageId)
        {
            StandardPackageType packageTypes = null;

            packageTypes = DataAccess.GetStandardPackageType(_connectionString, packageId);

            return packageTypes;
        }


        public string getConnectionString() { return _connectionString; }

        public int InsertTransit(PackageTransit packageTransit)
        {


            return DataAccess.InsertPackageTransit(_connectionString, packageTransit);

        }


        public PackageTransit GetTransit(Package package, DistributionCentre receiver)
        {
            PackageTransit packageTransit = null;

            packageTransit = DataAccess.GetPackageTransit(_connectionString, package, receiver);

            if (packageTransit == null)
                return null;

            return packageTransit;
        }

        public DistributionCentre GetHeadOffice()
        {                                                               // (P. 05-04-2015)

            List<DistributionCentre> allCentres = DataAccess.GetAllDistributionCentres(_connectionString);

            foreach (DistributionCentre centre in allCentres)
                if (centre.IsHeadOffice) return centre;

            return null;
        }

        public int InsertAudit(Employee employee, StandardPackageType packageType, List<string> barCodes)
        {

            int auditId = DataAccess.InsertAudit(_connectionString, employee, packageType);

            XElement barCodeXml = barCodes.GetBarCodeXML();

            DataAccess.InsertAuditPackages(_connectionString, auditId, packageType, barCodeXml);
            return auditId;

        }

        public int UpdateLostFromAudit(int auditId, DistributionCentre location, StandardPackageType packageType)
        {

            return DataAccess.UpdateLostFromAudit(_connectionString, auditId, location, packageType);
        }

        public int UpdateInstockFromAudit(int auditId, DistributionCentre location, StandardPackageType packageType)
        {
            return DataAccess.UpdateInstockFromAudit(_connectionString, auditId, location, packageType);
        }

        public int UpdateTransitReceivedFromAudit(int auditId, DistributionCentre location)
        {

            return DataAccess.UpdateTransitReceivedFromAudit(_connectionString, auditId, location);
        }

        public int UpdateTransitCancelledFromAudit(int auditId, DistributionCentre location)
        {

            return DataAccess.UpdateTransitCancelledFromAudit(_connectionString, auditId, location);
        }

        // *************************************************************************

        public List<PackageTransit> GetAllPackageTransits()
        {                                                                        //   Added by Pablo on 23-03-15
            List<PackageTransit> allTransits = null;
            allTransits = DataAccess.GetAllPackageTransits(_connectionString);
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
