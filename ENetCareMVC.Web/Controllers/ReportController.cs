
using ENetCareMVC.BusinessService;
using ENetCareMVC.Repository.Data;
using ENetCareMVC.Repository.Repository;
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
        private string myConnection = ConfigurationManager.ConnectionStrings["ENetCareLiveAll"].ConnectionString;
        // private string myConnection = ConfigurationManager.ConnectionStrings["ENetCareMVC"].ConnectionString;
        //ConfigurationManager.ConnectionStrings;  //"";

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ReportA()
        {
            IReportRepository repRepository = new ReportRepository(myConnection);
            ReportService repService = new ReportService(repRepository);
            List<DistributionCentreLosses> losses = repService.GetDistributionCentreLosses();
            return View(losses);
        }

        public ActionResult ReportB()
        {
            return View();
        }

        public ActionResult ReportC()
        {
            return View();
        }

        public ActionResult ReportD()
        {
            return View();
        }

        public ActionResult ReportE()
        {
            return View();
        }

    }
}