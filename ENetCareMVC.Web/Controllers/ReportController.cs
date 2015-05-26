﻿
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
            return View(reportService.GetDistributionCentreLosses());
        }

        public ActionResult DoctorActivity()
        {
             return View(reportService.GetDoctorActivity());
        }

        public ActionResult GlobalStock()
        {
              return View(reportService.GetGlobalStock());
        }

        public ActionResult DistributionCentreStock()
        {
             return View(reportService.GetDistributionCentreStock());
        }

        public ActionResult ValueInTransit()
        {
            return View(reportService.GetValueInTransit());
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
            if (string.IsNullOrEmpty(username)) return null;
            var employeeService = GetEmployeeService();
            return employeeService.Retrieve(username);
        }


        // **********************************************************************

        public ActionResult CentreLossesTwo()
        {
            return View(MockDataAccess.getMockedLosses());
        }

        public ActionResult DoctorActivityTwo()
        {
            return View(MockDataAccess.getMockedActivity());
        }

        public ActionResult GlobalStockTwo()
        {
            return View(MockDataAccess.getMockedGlobalStock());
        }

        public ActionResult DistributionCentreStockTwo()
        {
            return View(MockDataAccess.getMockedDistributionCentreStock());
        }
        public ActionResult StocktakingTwo()
        {
            return View(MockDataAccess.getMockedStocktaking());
        }

        public ActionResult ValueInTransitTwo()
        {
            return View(MockDataAccess.getMockedValueInTransit());
        }

     
    }
}