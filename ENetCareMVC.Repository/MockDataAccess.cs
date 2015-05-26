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


// ******************************************************


        public static List<DistributionCentreLosses> getMockedLosses()
        {
            //List<Package> packagesList = MockDataAccess.GetAllPackages();
            List<DistributionCentreLosses> lossesList = new List<DistributionCentreLosses>();

            DistributionCentreLosses l1 = new DistributionCentreLosses();
            l1.DistributionCenterName = "CentreA";
            l1.DistributionCentreId = 1;
            l1.LossRatioDenominator = 75;
            l1.LossRatioNumerator = 12;
            l1.TotalLossDiscardedValue = 445;

            DistributionCentreLosses l2 = new DistributionCentreLosses();
            l2.DistributionCenterName = "CentreB";
            l2.DistributionCentreId = 2;
            l2.LossRatioDenominator = 275;
            l2.LossRatioNumerator = 18;
            l2.TotalLossDiscardedValue = 1445;

            DistributionCentreLosses l3 = new DistributionCentreLosses();
            l3.DistributionCenterName = "CentreC";
            l3.DistributionCentreId = 3;
            l3.LossRatioDenominator = 175;
            l3.LossRatioNumerator = 22;
            l3.TotalLossDiscardedValue = 335;

            lossesList.Add(l1); lossesList.Add(l2); lossesList.Add(l3);
            return lossesList;

        }

        public static List<DistributionCentreStock> getMockedDistributionCentreStock()
        {
            List<DistributionCentreStock> stockList = new List<DistributionCentreStock>();
            DistributionCentreStock s1 = new DistributionCentreStock();
            s1.DistributionCenterName = "CentreA";
            s1.DistributionCentreId = 1;
            s1.CostPerPackage = 44;
            s1.NumberOfPackages = 5;
            s1.PackageTypeId = 4;
            s1.TotalValue = 145;

            DistributionCentreStock s2 = new DistributionCentreStock();
            s2.DistributionCenterName = "CentreB";
            s2.DistributionCentreId = 2;
            s2.CostPerPackage = 34;
            s2.NumberOfPackages = 15;
            s2.PackageTypeId = 3;
            s2.TotalValue = 245;

            DistributionCentreStock s3 = new DistributionCentreStock();
            s3.DistributionCenterName = "CentreC";
            s3.DistributionCentreId = 3;
            s3.CostPerPackage = 14;
            s3.NumberOfPackages = 12;
            s3.PackageTypeId = 1;
            s3.TotalValue = 45;

            stockList.Add(s1); stockList.Add(s2); stockList.Add(s3);
            return stockList;
        }

        public static List<DoctorActivity> getMockedActivity()
        {
            List<DoctorActivity> activityList = new List<DoctorActivity>();

            DoctorActivity d1 = new DoctorActivity();
            d1.DoctorId = 1;
            d1.DoctorName = "Dr. Newells";
            d1.PackageCount = 11;
            d1.PackageTypeDescription = "blah blah blah";
            d1.PackageTypeId = 1;
            d1.TotalPackageValue = 145;

            DoctorActivity d2 = new DoctorActivity();
            d2.DoctorId = 2;
            d2.DoctorName = "Dr. Mad";
            d2.PackageCount = 13;
            d2.PackageTypeDescription = "blah blah ";
            d2.PackageTypeId = 4;
            d2.TotalPackageValue = 35;

            DoctorActivity d3 = new DoctorActivity();
            d3.DoctorId = 3;
            d3.DoctorName = "Dr. Hell";
            d3.PackageCount = 23;
            d3.PackageTypeDescription = "blah blah ...";
            d3.PackageTypeId = 5;
            d3.TotalPackageValue = 245;

            activityList.Add(d1); activityList.Add(d2); activityList.Add(d3);
            return activityList;
        }

        public static List<StockTaking> getMockedStocktaking()
        {
            List<StockTaking> stockList = new List<StockTaking>();

            StockTaking s1 = new StockTaking();
            s1.PackageId = 2;
            s1.PackageTypeId = 2;
            s1.PackageTypeDescription = "some type";
            s1.BarCode = "3454534567567575";
            s1.CostPerPackage = 45;
            s1.CurrentLocationCentreId = 1;
            s1.ExpirationDate = DateTime.Today.AddDays(34);

            StockTaking s2 = new StockTaking();
            s2.PackageId = 3;
            s2.PackageTypeId = 6;
            s2.PackageTypeDescription = "some type";
            s2.BarCode = "43634545345345345";
            s2.CostPerPackage = 25;
            s2.CurrentLocationCentreId = 2;
            s2.ExpirationDate = DateTime.Today.AddDays(24);

            StockTaking s3 = new StockTaking();
            s3.PackageId = 4;
            s3.PackageTypeId = 2;
            s3.PackageTypeDescription = "some type";
            s3.BarCode = "345445344663466346";
            s3.CostPerPackage = 85;
            s3.CurrentLocationCentreId = 1;
            s3.ExpirationDate = DateTime.Today.AddDays(12);

            StockTaking s4 = new StockTaking();
            s4.PackageId = 5;
            s4.PackageTypeId = 7;
            s4.PackageTypeDescription = "some type";
            s4.BarCode = "34534523423234";
            s4.CostPerPackage = 15;
            s4.CurrentLocationCentreId = 2;
            s4.ExpirationDate = DateTime.Today.AddDays(2);

            StockTaking s5 = new StockTaking();
            s5.PackageId = 6;
            s5.PackageTypeId = 3;
            s5.PackageTypeDescription = "some type";
            s5.BarCode = "45345345234234";
            s5.CostPerPackage = 15;
            s5.CurrentLocationCentreId = 2;
            s5.ExpirationDate = DateTime.Today.AddDays(-14);

            stockList.Add(s1); stockList.Add(s2); stockList.Add(s3); stockList.Add(s4); stockList.Add(s5);
            return stockList;
        }

        public static List<ValueInTransit> getMockedValueInTransit()
        {
            List<ValueInTransit> transitList = new List<ValueInTransit>();
            ValueInTransit v1 = new ValueInTransit();
            v1.ReceiverDistributionCentreId = 1;
            v1.RecieverDistributionCentreName = "Centre D";
            v1.SenderDistributionCentreId = 2;
            v1.SenderDistributionCentreName = "Cantre A";
            v1.TotalPackages = 8;
            v1.TotalValue = 45;

            ValueInTransit v2 = new ValueInTransit();
            v2.ReceiverDistributionCentreId = 2;
            v2.RecieverDistributionCentreName = "Centre B";
            v2.SenderDistributionCentreId = 3;
            v2.SenderDistributionCentreName = "Cantre C";
            v2.TotalPackages = 7;
            v2.TotalValue = 65;

            transitList.Add(v1); transitList.Add(v2);
            return transitList;
        }

        public static List<GlobalStock> getMockedGlobalStock()
        {
            List<GlobalStock> stockList = new List<GlobalStock>();
            
            GlobalStock g1 = new GlobalStock();
            g1.CostPerPackage = 56;
            g1.NumberOfPackages = 9;
            g1.PackageTypeDescription = "blah blah blah";
            g1.PackageTypeId = 2;
            g1.TotalValue = 70;

            GlobalStock g2 = new GlobalStock();
            g2.CostPerPackage = 16;
            g2.NumberOfPackages = 2;
            g2.PackageTypeDescription = " blah blah";
            g2.PackageTypeId = 1;
            g2.TotalValue = 50;

            stockList.Add(g1); stockList.Add(g2);
            return stockList;
        }


    }
}
