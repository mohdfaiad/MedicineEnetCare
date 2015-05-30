using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ENetCareMVC.Web.Controllers;
using System.Web.Mvc;
using ENetCareMVC.Web.Models;
using ENetCareMVC.Repository.Repository;
using ENetCareMVC.Repository.Data;
using System.Collections.Generic;

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

        [TestMethod]
        public void TestDistributeEmployeeNotAuthorized()
        {
            IPackageRepository packageRepository = new MockPackageRepository();
            IEmployeeRepository employeeRepository = new MockEmployeeRepository();
            var controller = new PackageController(employeeRepository, packageRepository);

            Package package = packageRepository.GetPackageWidthBarCode("04867393563");

            List<SelectedPackage> spList = new List<SelectedPackage>();

            SelectedPackage sp = new SelectedPackage();
            sp.BarCode = package.BarCode;
            sp.CentreId = package.CurrentLocation.CentreId;
            sp.ExpirationDate = package.ExpirationDate;
            sp.PackageId = package.PackageId;
            sp.PackageTypeId = package.PackageType.PackageTypeId;
            sp.CurrentEmployeeUserName = "rsmith@hotmail.com";

            spList.Add(sp);

            var model = new PackageDistributeViewModel()
            {
                SelectedPackages = spList,
            };

            var result = controller.DistributeSave(model) as ViewResult;

            var modelReturned = result.Model as PackageDistributeViewModel;

            Assert.AreEqual("DistributeComplete", result.ViewName);
            Assert.AreEqual("You are not authorized to distribute packages", modelReturned.SelectedPackages[0].ProcessResultMessage);
        }

        [TestMethod]
        public void TestDistributeExpiredPackages()
        {
            IPackageRepository packageRepository = new MockPackageRepository();
            IEmployeeRepository employeeRepository = new MockEmployeeRepository();
            var controller = new PackageController(employeeRepository, packageRepository);

            Package package = packageRepository.GetPackageWidthBarCode("65985438786");

            List<SelectedPackage> spList = new List<SelectedPackage>();

            SelectedPackage sp = new SelectedPackage();
            sp.BarCode = package.BarCode;
            sp.CentreId = package.CurrentLocation.CentreId;
            sp.ExpirationDate = package.ExpirationDate;
            sp.PackageId = package.PackageId;
            sp.PackageTypeId = package.PackageType.PackageTypeId;
            sp.CurrentEmployeeUserName = "ihab@enetcare.com";

            spList.Add(sp);

            var model = new PackageDistributeViewModel()
            {
                SelectedPackages = spList,
            };

            var result = controller.DistributeSave(model) as ViewResult;

            var modelReturned = result.Model as PackageDistributeViewModel;

            Assert.AreEqual("DistributeComplete", result.ViewName);
            Assert.AreEqual("That package has expired, cannot distribute an expired package", modelReturned.SelectedPackages[0].ProcessResultMessage);
        }


        [TestMethod]
        public void TestDiscardPackageNotInStock()
        {
            IPackageRepository packageRepository = new MockPackageRepository();
            IEmployeeRepository employeeRepository = new MockEmployeeRepository();
            var controller = new PackageController(employeeRepository, packageRepository);

            Package package = packageRepository.GetPackageWidthBarCode("01298475141");

            List<SelectedPackage> spList = new List<SelectedPackage>();

            SelectedPackage sp = new SelectedPackage();
            sp.BarCode = package.BarCode;
            sp.CentreId = package.CurrentLocation.CentreId;
            sp.ExpirationDate = package.ExpirationDate;
            sp.PackageId = package.PackageId;
            sp.PackageTypeId = package.PackageType.PackageTypeId;
            sp.CurrentEmployeeUserName = "ihab@enetcare.com";

            spList.Add(sp);

            var model = new PackageDiscardViewModel()
            {
                SelectedPackages = spList,
            };

            var result = controller.DiscardSave(model) as ViewResult;

            var modelReturned = result.Model as PackageDiscardViewModel;

            Assert.AreEqual("DiscardComplete", result.ViewName);
            Assert.AreEqual("That Package is Not in Stock", modelReturned.SelectedPackages[0].ProcessResultMessage);
        }

        [TestMethod]
        public void TestDiscardPackageExpired()
        {
            IPackageRepository packageRepository = new MockPackageRepository();
            IEmployeeRepository employeeRepository = new MockEmployeeRepository();
            var controller = new PackageController(employeeRepository, packageRepository);

            Package package = packageRepository.GetPackageWidthBarCode("65985438786");

            List<SelectedPackage> spList = new List<SelectedPackage>();

            SelectedPackage sp = new SelectedPackage();
            sp.BarCode = package.BarCode;
            sp.CentreId = package.CurrentLocation.CentreId;
            sp.ExpirationDate = package.ExpirationDate;
            sp.PackageId = package.PackageId;
            sp.PackageTypeId = package.PackageType.PackageTypeId;
            sp.CurrentEmployeeUserName = "ihab@enetcare.com";

            spList.Add(sp);

            var model = new PackageDiscardViewModel()
            {
                SelectedPackages = spList,
            };

            var result = controller.DiscardSave(model) as ViewResult;

            var modelReturned = result.Model as PackageDiscardViewModel;

            Assert.AreEqual("DiscardComplete", result.ViewName);
            Assert.AreEqual("Succeeded", modelReturned.SelectedPackages[0].ProcessResultMessage);
        }
    }
}
