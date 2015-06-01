using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ENetCareMVC.BusinessService;
using ENetCareMVC.Web.Controllers;
using System.Web.Mvc;
using System.Web;
using System.Security.Principal;
using ENetCareMVC.Web.Models;
using ENetCareMVC.Repository.Repository;
using ENetCareMVC.Repository.Data;
using System.Collections.Generic;
using ENetCareMVC.UnitTest;

namespace ENetCareMVC.UnitTest.Controllers
{
   
    [TestClass]
    public class PackageTransitControllerTest
    {
        
        IPackageRepository packageRepository = new MockPackageRepository();
        IEmployeeRepository employeeRepository = new MockEmployeeRepository();

        // Test Send Initialized
        [TestMethod]
        public void TestSendPackagePage_Initialized()
        {
            var controller = new PackageTransitController(employeeRepository, packageRepository);
            var result = controller.Send() as ViewResult;
            var model = result.Model as PackageTransitSendViewModel;

            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsNotNull(model.SelectedPackages);
        }

        // Test Receive Initialized
        [TestMethod]
        public void TestReceivePackagePage_Initialized()
        {
            var controller = new PackageTransitController(employeeRepository, packageRepository);
            var result = controller.Receive() as ViewResult;
            var model = result.Model as PackageTransitReceiveViewModel;

            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsNotNull(model.SelectedPackages);
        }
        
        // Test SendPackage [Suceess]
        [TestMethod]
        public void TestSendPackagePage_SendPackage()
        {
            var controller = new PackageTransitController(employeeRepository, packageRepository);
            controller.ControllerContext = new ENetCareMVC.UnitTest.FakeClasses.FakeControllerContext(controller, "fsmith@hotmail.com", new string[] { "Agent" });
            Package package = packageRepository.GetPackageWidthBarCode("12344278431");
            
            DistributionCentre destinationLocation = new DistributionCentre()
            {
                CentreId = 1,
                Name = "mainCentre"
            };

            List<SelectedPackage> spList = new List<SelectedPackage>();

            SelectedPackage sp = new SelectedPackage()
            {
                BarCode = package.BarCode,
                CentreId = package.CurrentLocation.CentreId,
            };
            spList.Add(sp);

            var model = new PackageTransitSendViewModel
            {
                BarCode = package.BarCode,
                DestinationCentreId = destinationLocation.CentreId,
                SendDate = DateTime.Today,
                SelectedPackages = spList
            };
            var result = controller.SendSave(model) as ViewResult;
            var modelReturned = result.Model as PackageTransitSendViewModel;

            Assert.AreEqual("SendComplete", result.ViewName);
            Assert.AreEqual("Successful!", modelReturned.SelectedPackages[0].ProcessResultMessage);
            
        }

        // Test Send [InTransit] package
        [TestMethod]
        public void TestSendPackagePage_InTransit()
        {
            var controller = new PackageTransitController(employeeRepository, packageRepository);
            controller.ControllerContext = new ENetCareMVC.UnitTest.FakeClasses.FakeControllerContext(controller, "fsmith@hotmail.com", new string[] { "Agent" });
            Package package = packageRepository.GetPackageWidthBarCode("13154242431");

            DistributionCentre destinationLocation = new DistributionCentre()
            {
                CentreId = 1,
                Name = "mainCentre"
            };

            List<SelectedPackage> spList = new List<SelectedPackage>();

            SelectedPackage sp = new SelectedPackage()
            {
                BarCode = package.BarCode,
                CentreId = package.CurrentLocation.CentreId,
            };
            spList.Add(sp);

            var model = new PackageTransitSendViewModel()
            {
                SelectedPackages = spList,
                DestinationCentreId = destinationLocation.CentreId,
                BarCode = package.BarCode,
                SendDate = DateTime.Today
            };

            var result = controller.SendSave(model) as ViewResult;

            var modelReturned = result.Model as PackageTransitSendViewModel;
            //TransitResult.PackageAlreadyAtDestination

            Assert.AreEqual("SendComplete", result.ViewName);
            Assert.AreEqual(PackageResult.PackageIsNotInStock, modelReturned.SelectedPackages[0].ProcessResultMessage);
        }

        // Test Send Package with the same [destination and sender]
        [TestMethod]
        public void TestSendPackagePage_ToWrongDestination()
        {
            var controller = new PackageTransitController(employeeRepository, packageRepository);
            controller.ControllerContext = new ENetCareMVC.UnitTest.FakeClasses.FakeControllerContext(controller, "fsmith@hotmail.com", new string[] { "Agent" });

            Package package = packageRepository.GetPackageWidthBarCode("12344278431");
            DistributionCentre destinationLocation = new DistributionCentre()
            {
                CentreId = package.CurrentLocation.CentreId,
            };

            List<SelectedPackage> spList = new List<SelectedPackage>();

            SelectedPackage sp = new SelectedPackage()
            {
                BarCode = package.BarCode,
                CentreId = package.CurrentLocation.CentreId,
            };

            spList.Add(sp);

            var model = new PackageTransitSendViewModel()
            {
                SelectedPackages = spList,
                DestinationCentreId = destinationLocation.CentreId,
                BarCode = package.BarCode,
                SendDate = DateTime.Today
            };

            var result = controller.SendSave(model) as ViewResult;

            var modelReturned = result.Model as PackageTransitSendViewModel;

            Assert.AreEqual("SendComplete", result.ViewName);
            Assert.AreEqual(TransitResult.PackageAlreadyAtDestination, modelReturned.SelectedPackages[0].ProcessResultMessage);
        }

        // Test Send [Null Barcode] Package 
        [TestMethod]
        public void TestSendPackagePage_NullPackage()
        {
            var controller = new PackageTransitController(employeeRepository, packageRepository);
            controller.ControllerContext = new ENetCareMVC.UnitTest.FakeClasses.FakeControllerContext(controller, "fsmith@hotmail.com", new string[] { "Agent" });

            Package package = packageRepository.GetPackageWidthBarCode("12344278431");
            package.BarCode = null;
            DistributionCentre destinationLocation = new DistributionCentre()
            {
                CentreId = package.CurrentLocation.CentreId,
            };

            List<SelectedPackage> spList = new List<SelectedPackage>();

            SelectedPackage sp = new SelectedPackage()
            {
                BarCode = package.BarCode,
                CentreId = package.CurrentLocation.CentreId,
            };
            spList.Add(sp);

            var model = new PackageTransitSendViewModel()
            {
                SelectedPackages = spList,
                DestinationCentreId = destinationLocation.CentreId,
                BarCode = sp.BarCode,
                SendDate = DateTime.Today
            };

            var result = controller.SendSave(model) as ViewResult;

            var modelReturned = result.Model as PackageTransitSendViewModel;

            Assert.AreEqual("SendComplete", result.ViewName);
            Assert.AreEqual(PackageResult.BarCodeNotFound, modelReturned.SelectedPackages[0].ProcessResultMessage);
        }

        //Test Receive [Lost] Package - successful
        [TestMethod]
        public void TestReceivedPackagePage_Lost()
        {
            var controller = new PackageTransitController(employeeRepository, packageRepository);
            controller.ControllerContext = new ENetCareMVC.UnitTest.FakeClasses.FakeControllerContext(controller, "fsmith@hotmail.com", new string[] { "Agent" });

            Package package = packageRepository.GetPackageWidthBarCode("04983238778");
            package.CurrentStatus = PackageStatus.Lost;

            List<SelectedPackage> spList = new List<SelectedPackage>();

            SelectedPackage sp = new SelectedPackage()
            {
                BarCode = package.BarCode,
                CentreId = package.CurrentLocation.CentreId,
            };
            spList.Add(sp);

            var model = new PackageTransitReceiveViewModel()
            {
                SelectedPackages = spList,
                BarCode = package.BarCode,
                ReceiveDate = DateTime.Today
            };

            var result = controller.ReceiveSave(model) as ViewResult;
            var modelReturned = result.Model as PackageTransitReceiveViewModel;

            Assert.AreEqual("ReceiveComplete", result.ViewName);
            Assert.AreEqual("Successful!", modelReturned.SelectedPackages[0].ProcessResultMessage);
        }

        //Test Receive [Discarded] Package - successful
        [TestMethod]
        public void TestReceivedPackagePage_Discarded()
        {
            var controller = new PackageTransitController(employeeRepository, packageRepository);
            controller.ControllerContext = new ENetCareMVC.UnitTest.FakeClasses.FakeControllerContext(controller, "fsmith@hotmail.com", new string[] { "Agent" });

            Package package = packageRepository.GetPackageWidthBarCode("04983238779");

            List<SelectedPackage> spList = new List<SelectedPackage>();

            SelectedPackage sp = new SelectedPackage()
            {
                BarCode = package.BarCode,
                CentreId = package.CurrentLocation.CentreId,
            };
            spList.Add(sp);

            var model = new PackageTransitReceiveViewModel()
            {
                SelectedPackages = spList,
                BarCode = package.BarCode,
                ReceiveDate = DateTime.Today
            };

            var result = controller.ReceiveSave(model) as ViewResult;
            var modelReturned = result.Model as PackageTransitReceiveViewModel;

            Assert.AreEqual("ReceiveComplete", result.ViewName);
            Assert.AreEqual("Successful!", modelReturned.SelectedPackages[0].ProcessResultMessage);
        }

        //Test Receive [InStock] Package - successful
        [TestMethod]
        public void TestReceivedPackagePage_InStock()
        {
            var controller = new PackageTransitController(employeeRepository, packageRepository);
            controller.ControllerContext = new ENetCareMVC.UnitTest.FakeClasses.FakeControllerContext(controller, "fsmith@hotmail.com", new string[] { "Agent" });

            Package package = packageRepository.GetPackageWidthBarCode("04983238437");

            List<SelectedPackage> spList = new List<SelectedPackage>();

            SelectedPackage sp = new SelectedPackage()
            {
                BarCode = package.BarCode
            };
            spList.Add(sp);

            var model = new PackageTransitReceiveViewModel()
            {
                SelectedPackages = spList,
                BarCode = package.BarCode,
                ReceiveDate = DateTime.Today
            };

            var result = controller.ReceiveSave(model) as ViewResult;
            var modelReturned = result.Model as PackageTransitReceiveViewModel;

            Assert.AreEqual("ReceiveComplete", result.ViewName);
            Assert.AreEqual("Successful!", modelReturned.SelectedPackages[0].ProcessResultMessage);
        }

        //Test Receive [Distributed] Package  - successful
        [TestMethod]
        public void TestReceivedPackagePage_Distributed()
        {
            var controller = new PackageTransitController(employeeRepository, packageRepository);
            controller.ControllerContext = new ENetCareMVC.UnitTest.FakeClasses.FakeControllerContext(controller, "fsmith@hotmail.com", new string[] { "Agent" });

            Package package = packageRepository.GetPackageWidthBarCode("11623542734");

            List<SelectedPackage> spList = new List<SelectedPackage>();

            SelectedPackage sp = new SelectedPackage()
            {
                BarCode = package.BarCode
            };
            spList.Add(sp);

            var model = new PackageTransitReceiveViewModel()
            {
                SelectedPackages = spList,
                BarCode = package.BarCode,
                ReceiveDate = DateTime.Today
            };

            var result = controller.ReceiveSave(model) as ViewResult;
            var modelReturned = result.Model as PackageTransitReceiveViewModel;

            Assert.AreEqual("ReceiveComplete", result.ViewName);
            Assert.AreEqual("Successful!", modelReturned.SelectedPackages[0].ProcessResultMessage);
        }
    }
}
