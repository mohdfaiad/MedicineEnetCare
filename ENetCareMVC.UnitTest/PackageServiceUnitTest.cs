using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ENetCareMVC.Repository.Repository;
using ENetCareMVC.BusinessService;
using ENetCareMVC.Repository.Data;
using System.Diagnostics;
using ENetCareMVC.Repository;
using System.Configuration;
using ENetCareMVC.UnitTest;

namespace ENetCare.UnitTest
{
    [TestClass]
    public class PackageServiceUnitTest
    {

        public PackageServiceUnitTest()
        {
            MockDataAccess.LoadMockTables();
        }

        [TestMethod]
        public void TestCalculateExpirationDate()
        {
            IPackageRepository packageRepository = new MockPackageRepository();
            PackageService packageService = new PackageService(packageRepository);
            StandardPackageType packageType = MockDataAccess.GetPackageType(3);
            DateTime todaysDate = DateTime.Today;
            DateTime expirationDate = packageService.CalculateExpirationDate(packageType, todaysDate);
            Assert.AreEqual<DateTime>(todaysDate.AddMonths(packageType.ShelfLifeUnits), expirationDate);
        }

        [TestMethod]
        public void TestCalculateExpirationDate2()
        {
            IPackageRepository packageRepository = new MockPackageRepository();
            PackageService packageService = new PackageService(packageRepository);
            StandardPackageType packageType = MockDataAccess.GetPackageType(5);
            packageType.ShelfLifeUnitType = ShelfLifeUnitType.Day;
            DateTime todaysDate = DateTime.Today;
            DateTime expirationDate = packageService.CalculateExpirationDate(packageType, todaysDate);
            Assert.AreEqual<DateTime>(todaysDate.AddDays(packageType.ShelfLifeUnits), expirationDate);
        }

        [TestMethod]
        public void TestRegisterPackage()
        {            
            IPackageRepository packageRepository = new MockPackageRepository();
            PackageService packageService = new PackageService(packageRepository);
            StandardPackageType packageType = MockDataAccess.GetPackageType(3);
            DistributionCentre location = MockDataAccess.GetDistributionCentre(2);
            DateTime expirationDate = DateTime.Today.AddMonths(2);
            string barCode;
            var result = packageService.Register(packageType, location, expirationDate, out barCode);
            int newPackageId = result.Id;
            string compareBarCode = string.Format("{0:D5}{1:yyMMdd}{2:D5}", packageType.PackageTypeId, expirationDate, newPackageId);
            Assert.AreEqual<string>(compareBarCode, barCode);
        }

        [TestMethod]
        public void TestRegisterPackageExpirationDateTooEarly()
        {
            IPackageRepository packageRepository = new MockPackageRepository();
            PackageService packageService = new PackageService(packageRepository);
            StandardPackageType packageType = MockDataAccess.GetPackageType(3);
            DistributionCentre location = MockDataAccess.GetDistributionCentre(2);
            DateTime expirationDate = DateTime.Today.AddDays(-1);
            string barCode;
            var result = packageService.Register(packageType, location, expirationDate, out barCode);
            int newPackageId = result.Id;

            Assert.AreEqual<bool>(result.Success, false);
            Assert.AreEqual<string>(result.ErrorMessage, PackageResult.ExpirationDateCannotBeEarlierThanToday);
        }

        private Result DiscardPackage(int currentCentreId, string userName, string barCode)
        {
            MockPackageRepository packageRepository = new MockPackageRepository();
            PackageService _packageService = new PackageService(packageRepository);
            Employee employee = MockDataAccess.GetEmployee(userName);
            Package package = MockDataAccess.GetPackage(barCode);
            package.CurrentLocation = MockDataAccess.GetDistributionCentre(currentCentreId);
            StandardPackageType spt2 = _packageService.GetStandardPackageType(package.PackageType.PackageTypeId);
            return _packageService.Discard(package.BarCode, employee.Location, employee, package.ExpirationDate, spt2, package.PackageId);
        }

        [TestMethod]
        public void TestDiscard_HandleCentreNullReference()
        {
            var result = DiscardPackage(4, "rsmith@hotmail.com", "1232655456");
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestDiscard_PackageNotExpired()
        {
            //"ihab" works in centre 4 and package "04983238436" has not expired
            var result = DiscardPackage(4, "ihab@enetcare.com", "04983238436");
            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void TestDiscard_DiscardByManager()
        {
            //"rsmith" is a manager who works in centre 4 and so he should not be able to discard packages
            var result = DiscardPackage(4, "rsmith@hotmail.com", "96854278434");
            Assert.AreEqual("You are not authorized to discard packages", result.ErrorMessage);
        }

        [TestMethod]
        public void TestDiscard_PackageDistributedButTimeExpired()
        {
            //"ihab" works in centre 4 and package "01298475141" has been distributed already but the date has passed long after distribution
            var result = DiscardPackage(4, "ihab@enetcare.com", "01298475141");
            Assert.IsFalse(result.Success);
        }
        
        private Result DistributePackage(int currentCentreId, string userName, string barCode)
        {
            MockPackageRepository packageRepository = new MockPackageRepository();
            PackageService _packageService = new PackageService(packageRepository);
            Employee employee = MockDataAccess.GetEmployee(userName);
            Package package = MockDataAccess.GetPackage(barCode);
            package.CurrentLocation = MockDataAccess.GetDistributionCentre(currentCentreId);
            StandardPackageType spt2 = _packageService.GetStandardPackageType(package.PackageType.PackageTypeId);
            return _packageService.Distribute(package.BarCode, employee.Location, employee, package.ExpirationDate, spt2, package.PackageId);
        }

        [TestMethod]
        public void TestDistribute_EmployeeNotAuthorizedError()
        {
            //"rsmith" is a manager who works in centre 4 and so he should not be able to distribute
            var result = DistributePackage(4, "rsmith@hotmail.com", "96854278434");
            Assert.AreEqual("You are not authorized to distribute packages", result.ErrorMessage);
        }

        [TestMethod]
        public void TestDistribute_PackageAlreadyDistributed()
        {
            //"ihab" works at centre 4 and the package "04334278430" has already been distributed
            var result = DistributePackage(4, "ihab@enetcare.com", "04334278430");
            Assert.AreEqual("That Package has already been distributed", result.ErrorMessage);
        }

        [TestMethod]
        public void TestDistribute_DistributeExpiredPackages()
        {
            //"ihab" works in centre 4 and the package "65985438786" has expired
            var result = DistributePackage(4, "ihab@enetcare.com", "65985438786");
            Assert.AreEqual("That package has expired, cannot distribute an expired package", result.ErrorMessage);
        }

        [TestMethod]
        public void TestDistribute_InStockCurrentLocationUpdate()
        {
            //"ihab" works in centre 4 and the package "12344278431" is also in centre 4
            var result = DistributePackage(4, "ihab@enetcare.com", "04983238436");
            Assert.AreEqual(null, result.ErrorMessage);
        }

        [TestMethod]
        public void TestDistribute_DistributePackageFromDifferentLocation()
        {
            //"ihab" works in centre 4 and the package "96854278434" is in centre 2
            var result = DistributePackage(2, "ihab@enetcare.com", "96854278434");
            Assert.AreEqual(false, result.Success);
        }

        [TestMethod]
        public void TestReceivePackage_Successfully()
        {
            MockPackageRepository myMockPackageRepo = new MockPackageRepository();
            PackageService packageService = new PackageService(myMockPackageRepo);
            Package package1 = MockDataAccess.GetPackage(3);
            DistributionCentre myReceiverCentre = MockDataAccess.GetDistributionCentre(3);
            int newTransitId = InsertMockTransit(package1, 2, 3);                                       // insert transit
            packageService.Receive(package1.BarCode, myReceiverCentre, DateTime.Today);
            PackageTransit finishedTransit = MockDataAccess.GetPackageTransit(newTransitId);
            Debug.WriteLine(finishedTransit.ToString());
            Assert.IsTrue(finishedTransit.ReceiverCentre==myReceiverCentre);
        }

        [TestMethod]
        public void TestReceivePackage_BarcodeNotFound()
        {
            MockPackageRepository myMockPackageRepo = new MockPackageRepository();
            PackageService packageService = new PackageService(myMockPackageRepo);
            DistributionCentre myReceiverCentre = MockDataAccess.GetDistributionCentre(3);
            string bCode = "0001015042500004";        // package1.BarCode; //
            Result res = packageService.Receive(bCode, myReceiverCentre, DateTime.Today);
            Assert.AreEqual<bool>(res.Success, false);
            Assert.AreEqual<string>(res.ErrorMessage, TransitResult.BarCodeNotFound);
        }

        [TestMethod]
        public void TestReceivePackage_CancelTransit()
        {
            MockPackageRepository myMockPackageRepo = new MockPackageRepository();
            PackageService packageService = new PackageService(myMockPackageRepo);
            Package package1 = MockDataAccess.GetPackage(3);
            DistributionCentre myReceiverCentre = MockDataAccess.GetDistributionCentre(3);
            int newTransitId = InsertMockTransit(package1, 2, 3);                                       // insert transit
            Result res = packageService.CancelTransit(package1.BarCode, DateTime.Today);                // cancel transit
            int foundTransits = myMockPackageRepo.GetActiveTransitsByPackage(package1).Count;
            Assert.IsTrue(res.Success && foundTransits == 0);
        }

        [TestMethod]
        public void TestReceivePackage_ReceiveDateTooEarly()
        {
            MockPackageRepository myMockPackageRepo = new MockPackageRepository();
            PackageService packageService = new PackageService(myMockPackageRepo);
            Package package1 = MockDataAccess.GetPackage(3);
            DistributionCentre myReceiverCentre = MockDataAccess.GetDistributionCentre(3);
            int newTransitId = InsertMockTransit(package1, 2, 3);                                       // insert transit
            Result res = packageService.Receive(package1.BarCode, myReceiverCentre, DateTime.Today.AddMonths(-1));                // receive            
            Assert.AreEqual<bool>(res.Success, false);
            Assert.AreEqual<string>(res.ErrorMessage, PackageResult.ReceiveDateCannotBeEarlierThanSend);
        }

        public int InsertMockTransit(Package Package, int SenderId, int ReceiverId)
        {
            DistributionCentre mySenderCentre = MockDataAccess.GetDistributionCentre(SenderId);
            DistributionCentre myReceiverCentre = MockDataAccess.GetDistributionCentre(ReceiverId);
            PackageTransit newTransit = new PackageTransit();
            newTransit.Package = Package;
            newTransit.DateSent = DateTime.Today.AddDays(-2);
            newTransit.SenderCentre = mySenderCentre;
            newTransit.ReceiverCentre = myReceiverCentre;
            int newTransitId = MockDataAccess.InsertPackageTransit(newTransit);
            return newTransitId;
        }

        private Result SendPackage(int packageId, int senderCenterId, int receiverCenterId)
        {
            IPackageRepository _mockPackageRepository = new MockPackageRepository();
            PackageService _packageServices = new PackageService(_mockPackageRepository);
            Package _package = MockDataAccess.GetPackage(packageId);
            DistributionCentre _senderLocation = MockDataAccess.GetDistributionCentre(senderCenterId);
            DistributionCentre _destinationLocation = MockDataAccess.GetDistributionCentre(receiverCenterId);
            return _packageServices.Send(_package, _senderLocation, _destinationLocation , DateTime.Today);
        }
        [TestMethod]
        // Try to send Package into the Sender Centre location
        public void TestSendPackage_SameLocation()
        {
            IPackageRepository _mockPackageRepository = new MockPackageRepository();
            PackageService _packageServices = new PackageService(_mockPackageRepository);
            Package _package = MockDataAccess.GetPackage(3);
            DistributionCentre _senderLocation = MockDataAccess.GetDistributionCentre(2);
            DateTime _sendDate = DateTime.Today;
            var _result = _packageServices.Send(_package, _senderLocation, _senderLocation, _sendDate);
            Assert.AreEqual<bool>(false, _result.Success);
        }

        [TestMethod]
        // Try to send Package which is not abvailable
        public void TestSendPackage_PackageNotFond()
        {
            IPackageRepository _mockPackageRepository = new MockPackageRepository();
            PackageService _packageServices = new PackageService(_mockPackageRepository);
            DistributionCentre _senderLocation = MockDataAccess.GetDistributionCentre(1);
            DistributionCentre _destinationLocation = MockDataAccess.GetDistributionCentre(2);
            DateTime _sendDate = DateTime.Today;
            Package _package = null;
            var _result = _packageServices.Send(_package, _senderLocation, _destinationLocation, _sendDate);
            Assert.AreEqual<string>(_result.ErrorMessage, "Bar Code not found");
        }
        
        [TestMethod]
        // Try to send package to null destination centre
        public void TestSendPackage_SendToNullCentre()
        {
            IPackageRepository _mockPackageRepository = new MockPackageRepository();
            PackageService _packageServices = new PackageService(_mockPackageRepository);
            DistributionCentre _senderLocation = MockDataAccess.GetDistributionCentre(1);
            DistributionCentre _destinationLocation = MockDataAccess.GetDistributionCentre(2);
            DateTime _sendDate = DateTime.Today;
            Package _package = MockDataAccess.GetPackage(15);
            var _result = _packageServices.Send(_package, _package.CurrentLocation, null, _sendDate);
            Assert.AreEqual<string>("Please Select the Correct Receiver Centre", _result.ErrorMessage);
        }
        
        [TestMethod]
        // Try to send package which already is in destination centre
        public void TestSendPackage_SendToSenderCenter()
        {
            var _result = SendPackage(15, 4, 4);
            Assert.AreEqual<string>("Package appears as being already at the Destination Centre", _result.ErrorMessage);
        }
        
        [TestMethod]
        // Try to send Discarded Package
        public void TestSendPackage_SendNotInStockPackage()
        {
            var _result = SendPackage(14, 2, 2);
            Assert.AreEqual<string>("That Package is Not in Stock", _result.ErrorMessage);
        }
        
        [TestMethod]
        // try to send InTransit Packagepubl
        public void TestSendPackage_SendInTransitPackage()
        {
            var _result = SendPackage(13, 2, 1);
            Assert.AreEqual<string>("That Package is Not in Stock", _result.ErrorMessage);
        }
        
        [TestMethod]
        //try to send package that located in somewhere else
        public void TestSendPackage_AnotherCenter()
        {
            var _result = SendPackage(12, 3, 1);
            Assert.AreEqual<string>("That Package is NOT located in this distribution centre", _result.ErrorMessage);
        }

    }
}
