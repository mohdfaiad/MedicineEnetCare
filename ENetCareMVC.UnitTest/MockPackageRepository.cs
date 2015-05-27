using ENetCareMVC.Repository;
using ENetCareMVC.Repository.Data;
using ENetCareMVC.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENetCareMVC.UnitTest
{
    public class MockPackageRepository : IPackageRepository
    {
        public MockPackageRepository()          // Constructor              (P. 05-04-2015)
        {
            MockDataAccess.LoadMockTables();
        }

        public int Insert(Package xPackage)                              // (P. 04-04-2015)
        {
            return MockDataAccess.InsertPackage(xPackage);
        }

        public void Update(Package xPackage)                             // (P. 04-04-2015)
        {
            MockDataAccess.UpdatePackage(xPackage);
        }

        public Package Get(int? packageId, string barcode)
        {
            List<Package> packages = MockDataAccess.GetAllPackages();
            for (int i = 0; i < packages.Count; i++)
            {
                if (packages[i].BarCode == barcode)
                {
                    return packages[i];
                }
            }

            Package package = new Package
            {
                PackageId = packageId ?? 1,
                PackageType = MockDataAccess.GetPackageType(2),
                BarCode = string.IsNullOrEmpty(barcode) ? "000012015070100001" : barcode,
                ExpirationDate = new DateTime(2015, 7, 1),
                CurrentLocation = MockDataAccess.GetDistributionCentre(4),
                CurrentStatus = PackageStatus.InStock
            };
            return package;
        }

        public StandardPackageType GetStandardPackageType(int packageTypeId)
        {            
            return MockDataAccess.GetPackageType(packageTypeId);
        }

        public List<StandardPackageType> GetAllStandardPackageTypes()       // (P. 05-04-2015)
        {
            return MockDataAccess.GetAllPackageTypes();       
        }


        public Package GetPackage(int packageId)                                           // (P. 05-04-2015)
        {
            return MockDataAccess.GetPackage(packageId);
        }


        public Package GetPackageWidthBarCode(string BarCode)                           //   (P. 05-04-2015)
        {            
        foreach(Package p in MockDataAccess.GetAllPackages())
            if(p.BarCode==BarCode) return p;
        return null;
        }

        public int InsertTransit(PackageTransit PackageTransit)
        {
            Package tempPackage = new Package();
            tempPackage.PackageId = 1;
            tempPackage.BarCode = "012365423";
            DateTime dateTemp = new DateTime(2015, 05, 20);
            tempPackage.ExpirationDate = dateTemp;
            tempPackage.PackageType.PackageTypeId = 1;
            tempPackage.CurrentLocation.CentreId = 1;
            tempPackage.CurrentStatus = PackageStatus.InStock;
            // need more work to complete!
            return 1;
        }

        public void UpdateTransit(PackageTransit pt)
        {
            MockDataAccess.UpdatePackageTransit(pt);    
        }

        public PackageTransit GetTransit(Package Package, DistributionCentre receiver) // dc
        {            
            foreach (PackageTransit t in MockDataAccess.GetAllPackageTransits())
                if (t.Package == Package &&
                    (receiver == null || t.ReceiverCentre == receiver) &&
                    t.DateReceived == null &&
                    t.DateCancelled == null)
                    return t;
    
            return null;
        }

        public List<PackageTransit> GetActiveTransitsByPackage(Package xPackage)
        { 
            List<PackageTransit> filteredList = new List<PackageTransit>();
            foreach(PackageTransit t in MockDataAccess.GetAllPackageTransits() )
                if(t.Package==xPackage && t.DateReceived==null && t.DateCancelled==null) filteredList.Add(t);
            return filteredList;
        }

        public int InsertAudit(Employee employee, StandardPackageType packageType, List<string> barCodes)
        {
            return 1;
        }
        public int UpdateLostFromAudit(int auditId, DistributionCentre location, StandardPackageType packageType)
        {
            return 1;
        }
        public int UpdateInstockFromAudit(int auditId, DistributionCentre location, StandardPackageType packageType)
        {
            return 1;
        }
        public int UpdateTransitReceivedFromAudit(int auditId, DistributionCentre location)
        {
            return 1;
        }
        public int UpdateTransitCancelledFromAudit(int auditId, DistributionCentre location)
        {
            return 1;
        }
    
    }
}
