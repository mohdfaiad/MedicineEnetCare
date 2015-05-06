using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENetCareMVC.Repository.Data;
using System.Data;
using System.Xml.Linq;

namespace ENetCareMVC.Repository
{
    public class MockDataAccess
    {

	 static Dictionary<int, DistributionCentre> mockDistributionCentreDb = new Dictionary<int, DistributionCentre>();
	 static Dictionary<int, Employee> mockEmployeeDb = new Dictionary<int, Employee>();
	 static Dictionary<int, StandardPackageType> mockPackageTypeDb = new Dictionary<int, StandardPackageType>();
	 static Dictionary<int, Package> mockPackageDb = new Dictionary<int, Package>();
	 static Dictionary<int, PackageTransit> mockPackageTransitDb = new Dictionary<int, PackageTransit>();
	//Dictionary<int, Audit> mockAuditDb = new Dictionary<int, Audit>();

       
        // ****************************************************

        public  static DistributionCentre GetDistributionCentre(int id)
        { return mockDistributionCentreDb[id]; }

        public  static Package GetPackage(int id)
        { return mockPackageDb[id]; }

        public  static StandardPackageType GetPackageType(int id)
        { return mockPackageTypeDb[id]; }
        
        public  static PackageTransit GetPackageTransit(int id)
        { return mockPackageTransitDb[id]; }

        public  static Employee GetEmployee(int id)
        { return mockEmployeeDb[id]; }

        // *********************************************************************

        public static int InsertPackageTransit(PackageTransit t)
        {
            int newId = mockPackageTransitDb.Count();
            t.TransitId = newId;
            mockPackageTransitDb[newId] = t;
            return newId;
        }

        public  static int InsertPackage(Package p)
        {
            int newId = mockPackageDb.Count();
            p.PackageId = newId;
            mockPackageDb[newId]= p;
            return newId;
        }

        public  static int InsertDistributionCentre(DistributionCentre d)
        {
            int newId = mockDistributionCentreDb.Count();
            d.CentreId = newId; 
            mockDistributionCentreDb[newId] = d; 
            return newId;
        }

        public  static int InsertEmployee(Employee e)
        {
            int newId = mockEmployeeDb.Count();
            e.EmployeeId = newId;
            mockEmployeeDb[newId] = e;
            return newId;
        }

        public  static int InsertPackageType(StandardPackageType t)
        {
            int newId = mockPackageTypeDb.Count();
            t.PackageTypeId = newId;
            mockPackageTypeDb[newId] = t;
            return newId;
        }

        // ********************************************************

        public  static List<Employee> GetAllEmployees()
        { return mockEmployeeDb.Values.ToList<Employee>(); }

        public  static List<DistributionCentre> GetAllDistibutionCentres()
        { return mockDistributionCentreDb.Values.ToList<DistributionCentre>(); }

        public  static List<Package> GetAllPackages()
        { return mockPackageDb.Values.ToList<Package>(); }

        public  static List<PackageTransit> GetAllPackageTransits()
        { return mockPackageTransitDb.Values.ToList<PackageTransit>(); }

        public static List<StandardPackageType> GetAllPackageTypes()
        { return mockPackageTypeDb.Values.ToList<StandardPackageType>(); }

        // *************************************************************
        
        public static void UpdateDistributionCentre(DistributionCentre centre)
        { mockDistributionCentreDb[centre.CentreId] = centre;  }

        public static void UpdatePackage(Package package)
        { mockPackageDb[package.PackageId] = package; }

        public static void UpdatePackageType(StandardPackageType type)
        { mockPackageTypeDb[type.PackageTypeId] = type; }

        public static void UpdatePackageTransit(PackageTransit transit)
        { mockPackageTransitDb[transit.TransitId] = transit; }

        public static void UpdateEmployee(Employee employee)
        { mockEmployeeDb[employee.EmployeeId] = employee; }
        
        // *********************************************************************

        public static int AddPackageTransit(Package package, DistributionCentre sender, DistributionCentre receiver, DateTime dateSent)
        { 
           PackageTransit newTransit = new PackageTransit();
           newTransit.SenderCentre=sender;
           newTransit.SenderCentre = receiver;
           newTransit.Package = package;
           newTransit.DateSent = dateSent;
           newTransit.DateReceived = null;
           newTransit.DateCancelled = null;
           return InsertPackageTransit(newTransit);
        }

        public static int AddPackage(StandardPackageType Type, string BarCode, DistributionCentre Location, PackageStatus Status, DateTime Expiration)
        {
            Package newPackage = new Package();
            newPackage.BarCode = BarCode;
            newPackage.CurrentLocation = Location;
            newPackage.CurrentStatus = Status;
            newPackage.ExpirationDate = Expiration;
            newPackage.PackageType = Type;
            return InsertPackage(newPackage);
        }

        public static int AddDistributionCentre(string Name, string Address, string Phone ,bool IsHeadOffice)
        {
            DistributionCentre newCentre = new DistributionCentre();
            newCentre.Address = Address;
            newCentre.Name = Name;
            newCentre.IsHeadOffice = IsHeadOffice;
            newCentre.Phone = Phone;
            return InsertDistributionCentre(newCentre);
        }

        public static int AddEmployee(string FullName, string Email, DistributionCentre Location, EmployeeType Type,string  UserName, Guid UserId)
        { 
            Employee newEmployee = new Employee();
            newEmployee.FullName = FullName;
            newEmployee.EmailAddress = Email;
            newEmployee.Location = Location;
            newEmployee.UserName = UserName;
            newEmployee.UserId = UserId;
            newEmployee.EmployeeType = Type;
            return InsertEmployee(newEmployee);
        }

        public static int AddPackageType(string Description, int MedNumber, int ShelfLifeUnits, ShelfLifeUnitType UnitType, int Value)
        {
            StandardPackageType newType = new StandardPackageType();
            newType.Description = Description;
            newType.NumberOfMedications = MedNumber;
            newType.ShelfLifeUnits = ShelfLifeUnits;
            newType.ShelfLifeUnitType = UnitType;
            newType.Value = Value;
            return InsertPackageType(newType);
        }

        public static DistributionCentre GetHeadOffice()
        {
            foreach (DistributionCentre centre in mockDistributionCentreDb.Values.ToList())
                 if (centre.IsHeadOffice) return centre;
            return null;
        }

        public static void LoadMockTables()                         // (P. 05-04-2015)
        {
            if(mockDistributionCentreDb.Count()>5) return;   // dont insert samples if they are there already

            AddDistributionCentre("West Centre","12 Long Rd","0476 765234",false);
            AddDistributionCentre("South Centre", "502 Main Rd", "0534 123456", false);
            AddDistributionCentre("North Centre", "192 narrow Rd", "0376 09874324", false);
            AddDistributionCentre("East Centre", "39 fast Rd", "0276 75639233", false);
            AddDistributionCentre("Main Centre", "102 fast Rd", "0176 1734233", true);

            DistributionCentre mainCentre = GetHeadOffice();
            DistributionCentre centreTwo = GetDistributionCentre(2);
            DistributionCentre centreThree = GetDistributionCentre(3);
            AddEmployee("Benjamin", "ben@hotmail.com", mainCentre, EmployeeType.Agent, "ben", Guid.NewGuid());
            AddEmployee("Ihab", "ihab@hotmail.com", mainCentre, EmployeeType.Agent, "ihab", Guid.NewGuid());
            AddEmployee("Ramin", "ramin@hotmail.com", mainCentre, EmployeeType.Agent, "ramin", Guid.NewGuid());
            AddEmployee("Pablo", "pablo@hotmail.com", mainCentre, EmployeeType.Agent, "pablo", Guid.NewGuid());
            AddEmployee("Robert Smith", "rsmith@hotmail.com", mainCentre, EmployeeType.Manager, "rsmith", Guid.NewGuid());
            AddEmployee("Dr Hell", "drh@hotmail.com", centreTwo, EmployeeType.Doctor, "drhell", Guid.NewGuid());
            AddEmployee("Dr Love", "drl@hotmail.com", centreTwo, EmployeeType.Doctor, "drlove", Guid.NewGuid());
            AddEmployee("Dr Fishy", "drf@hotmail.com", centreTwo, EmployeeType.Doctor, "drfishy", Guid.NewGuid());
            AddEmployee("Dr Smith", "fsmith@hotmail.com", centreTwo, EmployeeType.Doctor, "fsmith", Guid.NewGuid());

            AddPackageType("Xophinol", 100, 5, ShelfLifeUnitType.Month, 200);
            AddPackageType("Utshol", 200, 5, ShelfLifeUnitType.Month, 400);
            AddPackageType("Happynol", 500, 5, ShelfLifeUnitType.Month, 1000);
            AddPackageType("Micolex", 100, 2, ShelfLifeUnitType.Month, 300);
            AddPackageType("PHilordoxin", 200, 2, ShelfLifeUnitType.Month, 600);
            AddPackageType("Panadol tablets", 100, 2, ShelfLifeUnitType.Month, 600);
            AddPackageType("Clorophin tablets", 25, 2, ShelfLifeUnitType.Month, 600);
            AddPackageType("Felix Tablets", 50, 2, ShelfLifeUnitType.Month, 600);

            StandardPackageType type2 = GetPackageType(2);
            StandardPackageType type3 = GetPackageType(3);
            StandardPackageType type4 = GetPackageType(4);
            AddPackage(type3, "45634278436", mainCentre, PackageStatus.InStock, DateTime.Parse("12/12/2016"));
            AddPackage(type3, "05548478000", mainCentre, PackageStatus.InTransit, DateTime.Parse("12/12/2016"));
            AddPackage(type4, "11623542734", mainCentre, PackageStatus.Distributed, DateTime.Parse("12/12/2016"));
            AddPackage(type3, "04983238436", mainCentre, PackageStatus.InStock, DateTime.Parse("12/12/2016"));

            AddPackage(type4, "13154242431", centreTwo, PackageStatus.InTransit, DateTime.Parse("12/11/2016"));
            AddPackage(type2, "96854278434", centreTwo, PackageStatus.InStock, DateTime.Parse("12/09/2016"));
            AddPackage(type3, "75634278434", centreTwo, PackageStatus.Distributed, DateTime.Parse("12/08/2016"));
            AddPackage(type4, "12344278431", centreTwo, PackageStatus.InStock, DateTime.Parse("12/11/2016"));

            AddPackage(type2, "04334278430", centreThree, PackageStatus.Distributed, DateTime.Parse("12/10/2016"));
            AddPackage(type2, "04867393563", centreThree, PackageStatus.InStock, DateTime.Parse("12/10/2016"));
            AddPackage(type2, "09423752364", centreThree, PackageStatus.InTransit, DateTime.Parse("12/09/2016"));
            AddPackage(type3, "76523442745", centreThree, PackageStatus.InStock, DateTime.Parse("12/08/2016"));

            AddPackage(type2, "0000000000", mainCentre, PackageStatus.InStock, DateTime.Parse("25/04/2016"));
            AddPackage(type3, "1232655456", mainCentre, PackageStatus.InTransit, DateTime.Parse("25/04/2016"));
            AddPackage(type2, "1278710000", mainCentre, PackageStatus.Discarded, DateTime.Parse("25/04/2016"));
            //No:15
            AddPackage(type4, "9875400000", mainCentre, PackageStatus.InStock, DateTime.Parse("25/04/2016"));
            AddPackage(type4, "0000215488", mainCentre, PackageStatus.InStock, DateTime.Parse("25/04/2016"));
            AddPackage(type2, "0000000000", mainCentre, PackageStatus.InStock, DateTime.Parse("25/04/2016"));
        }



    }
}
