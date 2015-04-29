using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ENetCareMVC.Repository;
using ENetCareMVC.Repository.Data;
using System.Diagnostics;
using System.Collections.Generic;

namespace ENetCareMVC.UnitTest
{
    [TestClass]
    public class MockRepositoryUnitTest
    {

        public MockRepositoryUnitTest()
        {
            MockDataAccess.LoadMockTables();
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




    }
}
