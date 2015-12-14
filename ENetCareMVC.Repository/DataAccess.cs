using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using ENetCareMVC.Repository.Data;

namespace ENetCareMVC.Repository
{
    public class DataAccess
    {

        Dictionary<int, Employee> mockEmployeeDb = new Dictionary<int, Employee>();

        public static int InsertPackage(string connectionString, Package package)
        {
            using (var ctx = new Entities(connectionString))
            {                
                ctx.Package.Add(package);
                ctx.SaveChanges();
                
                return package.PackageId;
            }
        }

        public static void UpdatePackage(string connectionString, Package package)
        {
            using (var ctx = new Entities(connectionString))
            {
                var packageRecord = ctx.Package.Where(p => p.PackageId == package.PackageId);

                packageRecord.FirstOrDefault<Package>().BarCode = package.BarCode;
                packageRecord.FirstOrDefault<Package>().ExpirationDate = package.ExpirationDate;
                packageRecord.FirstOrDefault<Package>().CurrentLocationCentreId = package.CurrentLocationCentreId;
                packageRecord.FirstOrDefault<Package>().CurrentStatus = package.CurrentStatus;
                packageRecord.FirstOrDefault<Package>().DistributedByEmployeeId = package.DistributedByEmployeeId;
                
                ctx.SaveChanges();
            }
        }
      
        public static void UpdateEmployee(string connectionString, Employee employee)
        {
            using (var ctx = new Entities(connectionString))
            {
                var employeeRecord = (from e in ctx.Employee
                                      where e.UserName == employee.UserName
                                      && e.EmployeeId == employee.EmployeeId
                                      select e).First();

                employeeRecord.FullName = employee.FullName;
                employeeRecord.EmployeeType = employee.EmployeeType;
                employeeRecord.EmailAddress = employee.EmailAddress;
                employeeRecord.LocationCentreId = employee.LocationCentreId;
                employeeRecord.UserId = employee.UserId;
                ctx.SaveChanges();
            }
        }

        public static Package GetPackage(string connectionString, int? packageId, string barcode = null)
        {
            Package package = null;

            using (var ctx = new Entities(connectionString))
            {
                var packageRecord = (from e in ctx.Package.Include("CurrentLocation").Include("PackageType").Include("DistributedBy")
                                     where e.PackageId == (packageId.HasValue ? packageId.Value : e.PackageId)
                                     && e.BarCode == (string.IsNullOrEmpty(barcode) ? e.BarCode : barcode)
                                     select e).FirstOrDefault();
                package = new Package();
                package = packageRecord;
            }
            return package;
        }

        public static Employee GetEmployee(string connectionString, int? employeeId, string username)
        {
            Employee employee = null;

            using (var ctx = new Entities(connectionString))
            {                   
                var employeeRecord = (from e in ctx.Employee.Include("Location")
                                      where e.EmployeeId == (employeeId ?? e.EmployeeId) && e.UserName == (string.IsNullOrEmpty(username) ? e.UserName : username)
                                      select e).FirstOrDefault();
                
                employee = employeeRecord;
            }

            return employee;
        }

        public static DistributionCentre GetDistributionCentre(string connectionString, int centreId)
        {
            DistributionCentre centre = null;

            using (var ctx = new Entities(connectionString))
            {
                var centreRecord = (from dc in ctx.DistributionCentre
                                    where dc.CentreId == centreId
                                    select dc).First();

                centre = new DistributionCentre();
                centre = centreRecord;
            }

            return centre;
        }

        public static StandardPackageType GetStandardPackageType(string connectionString, int packageTypeId)
        {        

            using (var ctx = new Entities(connectionString))
            {
                var packageTypeRecord = ctx.StandardPackageType.Where(pt => pt.PackageTypeId == packageTypeId);

                var packageType = packageTypeRecord.FirstOrDefault();
                return packageType;
            }
        }

        public static List<DistributionCentre> GetAllDistributionCentres(string connectionString)
        {
            List<DistributionCentre> centres = new List<DistributionCentre>();

            using (var ctx = new Entities(connectionString))
            {
                centres = ctx.DistributionCentre.ToList();
            }

            return centres;
        }

        public static List<StandardPackageType> GetAllStandardPackageTypes(string connectionString)
        {
            var packageTypes = new List<StandardPackageType>();
            using (var ctx = new Entities(connectionString))
            {
                packageTypes = ctx.StandardPackageType.ToList();
            }

            return packageTypes;
        }

        public static List<Package> GetAllPackages(string connectionString, DistributionCentre location = null)
        {                                                          
            List<Package> packages = null;

            using (var ctx = new Entities(connectionString))
            {
                packages = ctx.Package.Where(p => (location == null ? p.CurrentLocationCentreId : location.CentreId) == p.CurrentLocationCentreId).ToList();
            }

            return packages;
        }


        public static List<Employee> GetAllEmployees(string connectionString)
        {
            List<Employee> allEmployees = new List<Employee>();

            using (var ctx = new Entities(connectionString))
            {
                allEmployees = ctx.Employee.ToList();
            }
            return allEmployees;
        }

        public static int InsertPackageTransit(string connectionString, PackageTransit packageTransit)
        {

            using (var ctx = new Entities(connectionString))
            {
                ctx.PackageTransit.Add(packageTransit);
                ctx.SaveChanges();

                return packageTransit.TransitId;
            }
        }

        public static void UpdatePackageTransit(string connectionString, PackageTransit transit)
        {            // Define Insert Query with Parameter
            using (var ctx = new Entities(connectionString))
            {
                var packageTransitRecord = (from e in ctx.PackageTransit
                                        where e.TransitId == transit.TransitId
                                        select e).First();

                packageTransitRecord.PackageId = transit.PackageId;
                packageTransitRecord.SenderCentreId = transit.SenderCentreId;
                packageTransitRecord.ReceiverCentreId = transit.ReceiverCentreId;
                packageTransitRecord.DateSent = transit.DateSent;
                packageTransitRecord.DateReceived = transit.DateReceived;
                packageTransitRecord.DateCancelled = transit.DateCancelled;

                ctx.SaveChanges();
            }
        }


        public static List<PackageTransit> GetAllPackageTransits(string connectionString)
        {                                                       // (P. 04/04/2015)
            var allTransits = new List<PackageTransit>();
            using (var ctx = new Entities(connectionString))
            {
                allTransits = ctx.PackageTransit.ToList();
            }

            return allTransits;
        }

        public static PackageTransit GetPackageTransit(string connectionString, Package package, DistributionCentre receiver)
        {
            PackageTransit packageTransit = null;
            // Define Update Query with Parameter

            using (var ctx = new Entities(connectionString))
            {
                var packageTransitRecord = (from e in ctx.PackageTransit
                                            where e.PackageId == package.PackageId
                                            && e.ReceiverCentreId == receiver.CentreId
                                            && e.DateReceived == null
                                            && e.DateCancelled == null
                                            select e).FirstOrDefault();
                
                packageTransit = packageTransitRecord;
            }

            return packageTransit;
        }

        public static PackageTransit GetPackageTransit(string connectionString, Package package)
        {
            PackageTransit packageTransit = null;
            // Define Update Query with Parameter

            using (var ctx = new Entities(connectionString))
            {
                var packageTransitRecord = (from e in ctx.PackageTransit
                                            where e.PackageId == package.PackageId                                            
                                            && e.DateReceived == null
                                            && e.DateCancelled == null
                                            select e).FirstOrDefault();

                packageTransit = packageTransitRecord;
            }

            return packageTransit;
        }

        public static int InsertAudit(string connectionString, Employee employee, StandardPackageType packageType)
        {            // define INSERT query with parameters 
            using (var ctx = new Entities(connectionString))
            {
                Audit audit = new Audit();
                audit.DateAudited = DateTime.Today;
                audit.DistributionCentreId = employee.Location.CentreId;
                audit.EmployeeId = employee.EmployeeId;
                audit.PackageTypeId = packageType.PackageTypeId;
                
                ctx.Audit.Add(audit);
                ctx.SaveChanges();

                return audit.AuditId;
            }
        }

        public static void InsertAuditPackages(string connectionString, int auditId, StandardPackageType packageType, List<string> barCodeList)
        {
            using (var ctx = new Entities(connectionString))
            {
                var selectedPackages = from p in ctx.Package
                    join b in barCodeList on p.BarCode equals b                    
                    select p.PackageId;
                foreach (var packageId in selectedPackages)
                {
                    var auditPackage = new AuditPackage();
                    auditPackage.AuditId = auditId;
                    auditPackage.PackageId = packageId;
                    ctx.AuditPackage.Add(auditPackage);
                }
                ctx.SaveChanges();
            }
        }

        public static int UpdateLostFromAudit(string connectionString, int auditId, DistributionCentre location, StandardPackageType packageType)
        {            
            using (var ctx = new Entities(connectionString))
            {
                var lostPackages = (from p in ctx.Package
                   join ap in ctx.AuditPackage on new { AuditId = auditId, p.PackageId } equals new { ap.AuditId, ap.PackageId } into ps
                   from ap in ps.DefaultIfEmpty()
                   where ap == null && p.CurrentStatus == PackageStatus.InStock && p.PackageTypeId == packageType.PackageTypeId &&
                    p.CurrentLocationCentreId == location.CentreId
                   select p).ToList();
                int lostPackageCount = lostPackages.Count();
                foreach (var lostPackage in lostPackages)
                {
                    lostPackage.CurrentStatus = PackageStatus.Lost;
                    ctx.SaveChanges();
                }

                return lostPackageCount;
            }      
        }

        public static int UpdateInstockFromAudit(string connectionString, int auditId, DistributionCentre location, StandardPackageType packageType)
        {            
            using (var ctx = new Entities(connectionString))
            {
                var receivedPackages = (from p in ctx.Package
                   join ap in ctx.AuditPackage on new { AuditId = auditId, p.PackageId } equals new { ap.AuditId, ap.PackageId }
                   where p.PackageTypeId == packageType.PackageTypeId && (p.CurrentLocationCentreId != location.CentreId || p.CurrentStatus != PackageStatus.InStock)
                   select p).ToList();

                int receivedPackageCount = receivedPackages.Count();
                foreach (var package in receivedPackages)
                {
                    package.CurrentStatus = PackageStatus.InStock;
                    package.CurrentLocationCentreId = location.CentreId;
                    package.DistributedByEmployeeId = null;
                    ctx.SaveChanges();
                }
                return receivedPackageCount;
            }
        }

        public static int UpdateTransitReceivedFromAudit(string connectionString, int auditId, DistributionCentre location)
        { 
            using (var ctx = new Entities(connectionString))
            {
                var audit = ctx.Audit.FirstOrDefault(a => a.AuditId == auditId);
                var receivedTransits = (from pt in ctx.PackageTransit
                   join ap in ctx.AuditPackage on new { AuditId = auditId, pt.PackageId } equals new { ap.AuditId, ap.PackageId }
                   where pt.ReceiverCentreId == location.CentreId && pt.DateReceived == null && pt.DateCancelled == null
                   select pt).ToList();

                int receivedTransitCount = receivedTransits.Count();
                foreach (var transits in receivedTransits)
                {
                    transits.DateReceived = audit.DateAudited;
                    ctx.SaveChanges();
                }
                return receivedTransitCount;
            }
        }

        public static int UpdateTransitCancelledFromAudit(string connectionString, int auditId, DistributionCentre location)
        {
            using (var ctx = new Entities(connectionString))
            {
                var audit = ctx.Audit.FirstOrDefault(a => a.AuditId == auditId);
                var cancelledTransits = (from pt in ctx.PackageTransit
                   join ap in ctx.AuditPackage on new { AuditId = auditId, pt.PackageId } equals new { ap.AuditId, ap.PackageId }
                   where pt.ReceiverCentreId != location.CentreId && pt.DateReceived == null && pt.DateCancelled == null
                   select pt).ToList();

                int cancelledTransitCount = cancelledTransits.Count();
                foreach (var transits in cancelledTransits)
                {
                    transits.DateCancelled = audit.DateAudited;
                    ctx.SaveChanges();
                }

                return cancelledTransitCount;
            }
        }
    }
}
