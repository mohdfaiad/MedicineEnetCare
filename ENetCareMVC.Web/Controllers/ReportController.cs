
using ENetCareMVC.BusinessService;
using ENetCareMVC.Repository;
using ENetCareMVC.Repository.Data;
using ENetCareMVC.Repository.Repository;
using ENetCareMVC.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ENetCareMVC.Web.Controllers
{

    public class ReportController : Controller
    {
        static string myConnection = ConfigurationManager.ConnectionStrings["ENetCareLiveAll"].ConnectionString;
        static IReportRepository repRepository = new ReportRepository(myConnection);
        static ReportService reportService = new ReportService(repRepository);



  
        public ActionResult CentreLosses()
        {
            List<DistributionCentreLosses> lossesList = reportService.GetDistributionCentreLosses();
            List<DistributionCentreLosses> mockedLossesList = getMockedLosses();
            return View(mockedLossesList);
        }

        public ActionResult DoctorActivity()
        {
            List<DoctorActivity> activityList = reportService.GetDoctorActivity();
            //List<DoctorActivity> mockedActivityList = getMockedActivity();
            return View(activityList);
        }

        public ActionResult GlobalStock()
        {
            List<GlobalStock> stockList = reportService.GetGlobalStock();
            //List<GlobalStock> mockedStockList = getMockedStock();
            return View(stockList);
        }

        public ActionResult Stocktaking()
        {
            var employee = GetCurrentEmployee();
                        
            List<StockTaking> stocktakingList = reportService.GetStocktaking(employee.LocationCentreId);
            var model = new StocktakingReportViewModel()
            {
                SelectedCentre = employee.Location,
                StocktakingList = stocktakingList
            };
            return View(model);
        }

        public ActionResult ValueInTransit()
        {
            List<ValueInTransit> valueList = reportService.GetValueInTransit();
            //List<ValueInTransit> mockedValueList = getMockedValueInTransit();
            return View(valueList);
        }

        public List<DistributionCentreLosses> getMockedLosses()
        {
            //List<Package> packagesList = MockDataAccess.GetAllPackages();
            List<DistributionCentreLosses> lossesList = new List<DistributionCentreLosses>();

         
            DistributionCentreLosses l1 = new DistributionCentreLosses();
            DistributionCentreLosses l2 = new DistributionCentreLosses();
            DistributionCentreLosses l3 = new DistributionCentreLosses();
            l1.DistributionCenterName = "CentreA";
            l1.DistributionCentreId = 1;
            l1.LossRatioDenominator = 75;
            l1.LossRatioNumerator = 12;
            l1.TotalLossDiscardedValue = 445;
            l2.DistributionCenterName = "CentreB";
            l2.DistributionCentreId = 2;
            l2.LossRatioDenominator = 275;
            l2.LossRatioNumerator = 18;
            l2.TotalLossDiscardedValue = 1445;
            l3.DistributionCenterName = "CentreC";
            l3.DistributionCentreId = 3;
            l3.LossRatioDenominator = 175;
            l3.LossRatioNumerator = 22;
            l3.TotalLossDiscardedValue = 335;

            lossesList.Add(l1); lossesList.Add(l2); lossesList.Add(l3);
            return lossesList;
            
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReportA()
        {
            List<DistributionCentreLosses> losses = reportService.GetDistributionCentreLosses();
            List<DistributionCentreLosses> mockedLosses = getMockedLosses();
            return View(mockedLosses);
        }


        private EmployeeService GetEmployeeService()
        {
            IEmployeeRepository repository = new EmployeeRepository(ConfigurationManager.ConnectionStrings["ENetCareLiveAll"].ConnectionString);
            return new EmployeeService(repository);
        }

        private ReportService GetReportService()
        {
            IReportRepository repository = new ReportRepository(ConfigurationManager.ConnectionStrings["ENetCareLiveAll"].ConnectionString);
            return new ReportService(repository);
        }

        private Employee GetCurrentEmployee()
        {
            string username = User.Identity.Name;
            if (string.IsNullOrEmpty(username))
                return null;

            var employeeService = GetEmployeeService();

            return employeeService.Retrieve(username);
        }

    }
}