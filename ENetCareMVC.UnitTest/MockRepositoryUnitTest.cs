using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ENetCareMVC.Repository;
using ENetCareMVC.Repository.Data;
using System.Diagnostics;
using System.Collections.Generic;
using ENetCareMVC.Repository.Repository;
using ENetCareMVC.BusinessService;

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


        [TestMethod]
        public void TestMockDb_ShowAllTables()
        {

            List<DistributionCentre> distList = MockDataAccess.GetAllDistibutionCentres();
            Debug.WriteLine("DISTRIBUTION CENTRES : ");
            foreach (DistributionCentre centre in distList) Debug.WriteLine(centre);

            List<Employee> employeeList = MockDataAccess.GetAllEmployees();
            Debug.WriteLine("\n\n EMPLOYEES : ");
            foreach (Employee emp in employeeList) Debug.WriteLine(emp);

            List<StandardPackageType> typeList = MockDataAccess.GetAllPackageTypes();
            Debug.WriteLine("\n\n STANDARD PACKAGE TYPES : ");
            foreach (StandardPackageType t in typeList) Debug.WriteLine(t);

            List<Package> packageList = MockDataAccess.GetAllPackages();
            Debug.WriteLine("\n\n PACKAGES : ");
            foreach (Package p in packageList) Debug.WriteLine(p);

            List<PackageTransit> transitList = MockDataAccess.GetAllPackageTransits();
            Debug.WriteLine("\n\n PACKAGE TRANSITS : ");
            foreach (PackageTransit t in transitList) Debug.WriteLine(t);

            Assert.IsNotNull(employeeList);
        }



    
        [TestMethod]
        public void TestReportDistriCentreLosses()
        {
            List<DistributionCentreLosses> lossesList = _repService.GetDistributionCentreLosses();
            int t=5;
            Assert.IsTrue(t == 4);
        }

        [TestMethod]
        public void TestReportGlobalStock()
        {
            List<GlobalStock> stockList = _repService.GetGlobalStock();
            int t = 5;
            Assert.IsTrue(t == 4);
        }

        [TestMethod]
        public void TestReportStocktake()
        {
            List<StockTaking> stockList = _repService.GetStocktaking(2);
            int t = 5;
            Assert.IsTrue(t == 4);
        }

        [TestMethod]
        public void TestReportDocotrActivity()
        {
            List<DoctorActivity> activityList = _repService.GetDoctorActivity();
            int t = 5;
            Assert.IsTrue(t == 4);
        }

        [TestMethod]
        public void TestReportDistriCentreStock()
        {
            List<DistributionCentreStock> stockList = _repService.GetDistributionCentreStock();
            int t = 5;
            Assert.IsTrue(t == 4);
        }

        [TestMethod]
        public void TestReportValueInTransit()
        {
            List<ValueInTransit> stockList = _repService.GetValueInTransit();
            int t = 5;
            Assert.IsTrue(t == 4);
        }


    }
}
