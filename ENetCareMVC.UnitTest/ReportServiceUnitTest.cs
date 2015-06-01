using ENetCareMVC.BusinessService;
using ENetCareMVC.Repository;
using ENetCareMVC.Repository.Data;
using ENetCareMVC.Repository.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Web.UI;
//using System.Web.UI.WebControls;
namespace ENetCareMVC.UnitTest
{
    [TestClass]
    public class ReportServiceUnitTest
    {
        public ReportServiceUnitTest()
        {
            MockDataAccess.LoadMockTables();
        }

        [TestMethod]
        public void TestReport_Mock_GetDistributionCentreStock()
        {
            IReportRepository reportRepository = new MockReportRepository();
            ReportService reportService = new ReportService(reportRepository);
            var stockList = reportService.GetDistributionCentreStock();

            foreach (DistributionCentreStock s in stockList) Debug.WriteLine(s.ToString());
            Debug.WriteLine("Number of items: " + stockList.Count());
            Assert.AreEqual<int>(1, stockList.Count());
            Assert.AreEqual<decimal>(10, stockList[0].CostPerPackage);
        }

        [TestMethod]
        public void TestReport_Mock_GetDistributionCentreLosses()
        {
            IReportRepository reportRepository = new MockReportRepository();
            ReportService reportService = new ReportService(reportRepository);
            var lossesList = reportService.GetDistributionCentreLosses();

            foreach (DistributionCentreLosses l in lossesList) Debug.WriteLine(l.ToString());
            Debug.WriteLine("Number of items: " + lossesList.Count());
            Assert.AreEqual<int>(1, lossesList.Count());
            Assert.AreEqual<int>(15, lossesList[0].LossRatioDenominator.Value);
        }

        /*
        [TestMethod]
        public void TestReport_Mock_GetStocktaking()
        {
            IReportRepository reportRepository = new MockReportRepository();
            ReportService reportService = new ReportService(reportRepository);
            var spList = reportService.GetStocktaking(1); 

            //MockReportRepository repo = new MockReportRepository();
            //ReportService _reportService = new ReportService(repo);
            //List<StockTaking> spList = _reportService.GetStocktaking(4);

            foreach (StockTaking p in spList) Debug.WriteLine(p.ToString());
            Debug.WriteLine("Number of items: " + spList.Count());
            Assert.IsTrue(spList.Count() > 0); 
        }
         
         

        [TestMethod]
        public void TestReportValueInTransitView()
        {
            ReportController controller = new ReportController();
            ViewResult result = controller.ValueInTransit() as ViewResult;
            Assert.AreEqual("ValueInTransit", result.ViewBag.Message);
        }

        [TestMethod]
        public void TestReportDistriCentreLossesView()
        {
            ReportController controller = new ReportController();
            ViewResult result = controller.CentreLosses() as ViewResult;
            Assert.AreEqual("CentreLosses", result.ViewBag.Message);
        }

        [TestMethod]
        public void TestReportDistriCentreStockView()
        {
            ReportController controller = new ReportController();
            ViewResult result = controller.DistributionCentreStock() as ViewResult;
            Assert.AreEqual("ValueInTransit", result.ViewBag.Message);
        }

        [TestMethod]
        public void TestReportDoctorActivityView()
        {
            ReportController controller = new ReportController();
            ViewResult result = controller.DoctorActivity() as ViewResult;
            Assert.AreEqual("DoctorsActivity", result.ViewBag.Message);
        }

        [TestMethod]
        public void TestReportGlobalStockView()
        {
            ReportController controller = new ReportController();
            ViewResult result = controller.GlobalStock() as ViewResult;
            Assert.AreEqual("GlobalStock", result.ViewBag.Message);
        }

        [TestMethod]
        public void TestReportStocktakingView()
        {
            ReportController controller = new ReportController();
            ViewResult result = controller.Stocktaking() as ViewResult;
            Assert.AreEqual("Stocktaking", result.ViewBag.Message);
        }

         
        */



    }
}
