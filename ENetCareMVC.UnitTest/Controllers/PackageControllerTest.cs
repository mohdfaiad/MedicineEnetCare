using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ENetCareMVC.Web.Controllers;
using System.Web.Mvc;
using ENetCareMVC.Web.Models;
using ENetCareMVC.Repository.Repository;

namespace ENetCareMVC.UnitTest.Controllers
{
    [TestClass]
    public class PackageControllerTest
    {
        [TestInitialize]
        public void TestInitialize()
        {           
        }
        
        [TestMethod]
        public void TestInitialRegisterPage()
        {
            IPackageRepository packageRepository = new MockPackageRepository();
            IEmployeeRepository employeeRepository = new MockEmployeeRepository();
            var controller = new PackageController(employeeRepository, packageRepository); 

            var result = controller.Register() as ViewResult;

            var model = result.Model as PackageRegisterViewModel;

            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsTrue(model.StandardPackageTypes.Any());
        }

        [TestMethod]
        public void TestPostBackRegisterPage()
        {
            IPackageRepository packageRepository = new MockPackageRepository();
            IEmployeeRepository employeeRepository = new MockEmployeeRepository();
            var controller = new PackageController(employeeRepository, packageRepository);
            var model = new PackageRegisterViewModel()
            {
                StandardPackageTypeId = 3,
                LocationCentreId = 1,
                ExpirationDate = new DateTime(2015, 6, 20)
            };

            var result = controller.Register(model) as ViewResult;

            var modelReturned = result.Model as PackageRegisterViewModel;

            string compareStartOfBarCode = string.Format("{0:D5}{1:yyMMdd}", model.StandardPackageTypeId, model.ExpirationDate);

            Assert.AreEqual("RegisterComplete", result.ViewName);
            Assert.IsTrue(modelReturned.BarCode.StartsWith(compareStartOfBarCode));
        }
    }
}
