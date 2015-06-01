using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ENetCareMVC.Repository;
using ENetCareMVC.Repository.Data;
using System.Diagnostics;
using System.Collections.Generic;
using ENetCareMVC.Repository.Repository;
using ENetCareMVC.BusinessService;
using ENetCareMVC.Web.Controllers;
using System.Web.Mvc;

namespace ENetCareMVC.UnitTest
{
    [TestClass]
    public class MockRepositoryUnitTest
    {

        private static IReportRepository _reportRepository = new MockReportRepository();
        private static ReportService _repService = new ReportService(_reportRepository);


        public MockRepositoryUnitTest()
        {
            MockDataAccess.LoadMockTables();        // Load mock Tables
        }

        /// <summary>
        /// This method outputs all mocked records to the debug window
        /// </summary>
        [TestMethod]
        public void TestMockDb_ShowAllTables()
        {
            
            List<DistributionCentre> distList = MockDataAccess.GetAllDistibutionCentres();
            Debug.WriteLine("DISTRIBUTION CENTRES : ");
            foreach (DistributionCentre centre in distList) Debug.WriteLine(CentreToString(centre));

            List<Employee> employeeList = MockDataAccess.GetAllEmployees();
            Debug.WriteLine("\n\n EMPLOYEES : ");
            foreach (Employee emp in employeeList) Debug.WriteLine(EmployeeToString(emp));

            List<StandardPackageType> typeList = MockDataAccess.GetAllPackageTypes();
            Debug.WriteLine("\n\n STANDARD PACKAGE TYPES : ");
            foreach (StandardPackageType t in typeList) Debug.WriteLine(PackageTypeToString(t));

            List<Package> packageList = MockDataAccess.GetAllPackages();
            Debug.WriteLine("\n\n PACKAGES : ");
            foreach (Package p in packageList) Debug.WriteLine(PackageToString(p));

            List<PackageTransit> transitList = MockDataAccess.GetAllPackageTransits();
            Debug.WriteLine("\n\n PACKAGE TRANSITS : ");
            foreach (PackageTransit t in transitList) Debug.WriteLine(TransitToString(t));

            Assert.IsNotNull(employeeList);
        }



    
        [TestMethod]
        public void TestReportDistriCentreLosses()
        {
            List<DistributionCentreLosses> lossesList = _repService.GetDistributionCentreLosses();
            Debug.WriteLine("\n\n LOSSES : ");
            foreach (DistributionCentreLosses l in lossesList) Debug.WriteLine(LossesToString(l));
            int items=lossesList.Count;
            Assert.IsTrue(items >0);
        }

        [TestMethod]
        public void TestReportGlobalStock()
        {
            List<GlobalStock> stockList = _repService.GetGlobalStock();
            Debug.WriteLine("\n\n GLOBAL STOCK : ");
            foreach (GlobalStock gs in stockList) Debug.WriteLine(GlobalStockToString(gs));
            int items = stockList.Count;
            Assert.IsTrue(items > 0);
        }

        [TestMethod]
        public void TestReportStocktake()
        {
            List<StockTaking> stockList = _repService.GetStocktaking(2);
            Debug.WriteLine("\n\n STOCKTAKING : ");
            foreach (StockTaking s in stockList) Debug.WriteLine(StocktakingToString(s));
            int items = stockList.Count;
            Assert.IsTrue(items > 0);
        }

        [TestMethod]
        public void TestReportDoctorActivity()
        {
            List<DoctorActivity> activityList = _repService.GetDoctorActivity();
            Debug.WriteLine("\n\n DOCTOR ACTIVITY : ");
            foreach (DoctorActivity a in activityList) Debug.WriteLine(DoctorActivityToString(a));
            int items = activityList.Count;
            Assert.IsTrue(items > 0);
        }

        [TestMethod]
        public void TestReportDistriCentreStock()
        {
            List<DistributionCentreStock> stockList = _repService.GetDistributionCentreStock();
            Debug.WriteLine("\n\n DISTRIBUTION CENTRE STOCK : ");
            foreach (DistributionCentreStock s in stockList) Debug.WriteLine(CentreStockToString(s));
            int items = stockList.Count;
            Assert.IsTrue(items > 0);
        }

        [TestMethod]
        public void TestReportValueInTransit()
        {
            List<ValueInTransit> valueList = _repService.GetValueInTransit();
            Debug.WriteLine("\n\n VALUE IN TRANSIT : ");
            foreach (ValueInTransit vt in valueList) Debug.WriteLine(ValueInTransitToString(vt));
            int items = valueList.Count;
            Assert.IsTrue(items > 0);
        }





        
 public string CentreToString(DistributionCentre c) { return "Id:" + c.CentreId + " / " + c.Name + " / " + c.Address; }

 public string EmployeeToString(Employee e) { return "Id:" + e.EmployeeId + " / " + e.FullName + " (" + e.UserName + ") / " + e.EmailAddress + " / Location:" + e.LocationCentreId; }

 public string PackageToString(Package p) { return "Id:" + p.PackageId + " / Type:" + p.PackageTypeId + " / Code:" + p.BarCode + " / Exp:" + p.ExpirationDate + " / Location:" + p.CurrentLocation; }

 public string PackageTypeToString(StandardPackageType t) { return "Id:" + t.PackageTypeId + " / " + t.Description + " / #:" + t.NumberOfMedications + " / Life:" + t.ShelfLifeUnits + t.ShelfLifeUnitType + " / Val:" + t.Value; }

 public string TransitToString(PackageTransit t) { return "Id:" + t.TransitId + " / PId:" + t.PackageId + " / from:" + t.SenderCentreId + " / to:" + t.ReceiverCentreId; }


 public string LossesToString(DistributionCentreLosses l) { return "C_Id:" + l.DistributionCentreId + " / C_Name:" + l.DistributionCenterName + " / Ratio:" + l.LossRatioNumerator +"/"+l.LossRatioDenominator + " / Value:" + l.TotalLossDiscardedValue; }

 public string CentreStockToString(DistributionCentreStock s) { return "C_Id:" + s.DistributionCentreId + " / CentreName:" + s.DistributionCenterName + " / T_Id:" + s.PackageTypeId + " / TypeDesc:" + s.PackageTypeDescription + " / NumbOfPs:" + s.NumberOfPackages  +" / Value:" + s.TotalValue; }

 public string DoctorActivityToString(DoctorActivity a) { return "E_Id:" + a.DoctorId + " / Name:" + a.DoctorName + " / T_Id:" + a.PackageTypeId + " / TypeDesc:" + a.PackageTypeDescription + " / Count:" + a.PackageCount + " / Value:" + a.TotalPackageValue; }

 public string GlobalStockToString(GlobalStock gs) { return "T_Id:" + gs.PackageTypeId + " / T_Description:" + gs.PackageTypeDescription + " / CostPerP:" + gs.CostPerPackage + " / TotValue:" + gs.TotalValue; }

 public string ValueInTransitToString(ValueInTransit v) { return "SenderId:" + v.SenderDistributionCentreId + " / SenderName:" + v.SenderDistributionCentreName + " / ReceiverId:" + v.ReceiverDistributionCentreId + " / ReceiverName:" + v.RecieverDistributionCentreName + " / NumbOfPs:" + v.TotalPackages + " / Value:" + v.TotalValue; }

 public string StocktakingToString(StockTaking s) { return "P_Id:" + s.PackageId + " / T_Id:" + s.PackageTypeId + " / Descrip:" + s.PackageTypeDescription + " / Expiration:" + s.ExpirationDate + " / CostPerP:" + s.CostPerPackage; }

    }
}
