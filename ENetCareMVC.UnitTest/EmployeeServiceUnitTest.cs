using ENetCareMVC.BusinessService;
using ENetCareMVC.Repository;
using ENetCareMVC.Repository.Data;
using ENetCareMVC.Repository.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENetCareMVC.UnitTest
{

    [TestClass]
    public class EmployeeServiceUnitTest
    {

        public EmployeeServiceUnitTest()
        {
            MockDataAccess.LoadMockTables();
        }

        [TestMethod]
        public void TestUpdate()
        {
            IEmployeeRepository employeeRepository = new MockEmployeeRepository();
            EmployeeService employeeService = new EmployeeService(employeeRepository);

            DistributionCentre locationCentre = new DistributionCentre
            {
                CentreId = 1,
                Name = "North Centre"
            };

            var result = employeeService.Update("fsmith@hotmail.com", "Fred Smith", "fsmith@a.com", locationCentre, EmployeeType.Doctor);

            Assert.AreEqual<bool>(true, result.Success);
        }

        [TestMethod]
        public void TestUpdate_UsernameEmpty()
        {
            IEmployeeRepository employeeRepository = new MockEmployeeRepository();
            EmployeeService employeeService = new EmployeeService(employeeRepository);

            DistributionCentre locationCentre = new DistributionCentre
            {
                CentreId = 1,
                Name = "North Centre"
            };

            var result = employeeService.Update(string.Empty, "Fred Smith", "fsmith@a.com", locationCentre, EmployeeType.Doctor);

            Assert.AreEqual<bool>(false, result.Success);
            Assert.AreEqual<string>(EmployeeResult.UserNameCannotByEmpty, result.ErrorMessage);
        }

        [TestMethod]
        public void TestUpdate_UsernameNotFound()
        {
            IEmployeeRepository employeeRepository = new MockEmployeeRepository();
            EmployeeService employeeService = new EmployeeService(employeeRepository);

            DistributionCentre locationCentre = new DistributionCentre
            {
                CentreId = 1,
                Name = "North Centre"
            };

            var result = employeeService.Update("badusername", "Fred Smith", "fsmith@a.com", locationCentre, EmployeeType.Doctor);

            Assert.AreEqual<bool>(false, result.Success);
            Assert.AreEqual<string>(EmployeeResult.UserNameCannotBeFound, result.ErrorMessage);
        }

        [TestMethod]
        public void TestUpdate_FullNameEmpty()
        {
            IEmployeeRepository employeeRepository = new MockEmployeeRepository();
            EmployeeService employeeService = new EmployeeService(employeeRepository);

            DistributionCentre locationCentre = new DistributionCentre
            {
                CentreId = 1,
                Name = "North Centre"
            };

            var result = employeeService.Update("fsmith@hotmail.com", string.Empty, "fsmith@a.com", locationCentre, EmployeeType.Doctor);

            Assert.AreEqual<bool>(false, result.Success);
            Assert.AreEqual<string>(EmployeeResult.FullNameCannotByEmpty, result.ErrorMessage);
        }

        [TestMethod]
        public void TestUpdate_EmailAddressEmpty()
        {
            IEmployeeRepository employeeRepository = new MockEmployeeRepository();
            EmployeeService employeeService = new EmployeeService(employeeRepository);

            DistributionCentre locationCentre = new DistributionCentre
            {
                CentreId = 1,
                Name = "North Centre"
            };

            var result = employeeService.Update("fsmith@hotmail.com", "Fred Smith", string.Empty, locationCentre, EmployeeType.Doctor);

            Assert.AreEqual<bool>(false, result.Success);
            Assert.AreEqual<string>(EmployeeResult.EmailAddressCannotByEmpty, result.ErrorMessage);
        }

        [TestMethod]
        public void TestUpdate_LocationCentreEmpty()
        {
            IEmployeeRepository employeeRepository = new MockEmployeeRepository();
            EmployeeService employeeService = new EmployeeService(employeeRepository);

            DistributionCentre locationCentre = new DistributionCentre
            {
                CentreId = 1,
                Name = "North Centre"
            };

            var result = employeeService.Update("fsmith@hotmail.com", "Fred Smith", "fsmith@a.com", null, EmployeeType.Doctor);

            Assert.AreEqual<bool>(false, result.Success);
            Assert.AreEqual<string>(EmployeeResult.LocationCentreCannotByEmpty, result.ErrorMessage);
        }
    }
}
