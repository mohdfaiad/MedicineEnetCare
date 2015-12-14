using System.Linq;
using System.Diagnostics;
using System.Configuration;
using System.Collections.Generic;
using ENetCareMVC.BusinessService;
using ENetCareMVC.Repository.Data;
using ENetCareMVC.Repository.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ENetCareMVC.UnitTest                            
{

    [TestClass]
    public class DBUnitTest
    {

        //static string myConnection = ConfigurationManager.ConnectionStrings["ENetCareLiveAll"].ConnectionString;
        //static IReportRepository repRepository = new ReportRepository(_connectionString);
        //static ReportService reportService = new ReportService(repRepository);

        //  OLD static string _connectionString = ConfigurationManager.ConnectionStrings["EnetCare"].ConnectionString;
        // <add name="ENetCare" connectionString="Data Source=(LocalDb)\v11.0;AttachDbFilename=|DataDirectory|\ENetCareMVC.mdf;Initial Catalog=ENetCareMVC;Integrated Security=True" providerName="System.Data.SqlClient" />
        // <add name="ENetCareLiveAll" connectionString="metadata=res://*;provider=System.Data.SqlClient;provider connection string='Data Source=(LocalDb)\v11.0;AttachDbFilename=|DataDirectory|\ENetCareMVC.mdf;Initial Catalog=ENetCareMVC;Integrated Security=True'" providerName="System.Data.SqlClient" />
        //                                             ="metadata=res://*;provider=System.Data.SqlClient;provider connection string='Data Source=(LocalDb)\v11.0;AttachDbFilename=|DataDirectory|\ENetCareMVC.mdf;Initial Catalog=ENetCareMVC;Integrated Security=True'";
        
        //static string _connectionString = "metadata=res://*;provider=System.Data.SqlClient;provider connection string='Data Source=(LocalDb)\\v11.0;AttachDbFilename=|DataDirectory|\\ENetCareMVCDiff.mdf;Initial Catalog=ENetCareMVCDiff;Integrated Security=True'";
        static string _connectionString = ConfigurationManager.ConnectionStrings["ENetCareLiveAll"].ConnectionString;
        //SqlConnection enetConnection = new SqlConnection(_connectionString);



 /*
        [TestMethod]
        public void TestDbAccess_Connection()        // Assertion is true if connection was opened 
        {
            enetConnection.Open();
            System.Data.ConnectionState state = enetConnection.State;
            enetConnection.Close();
            CheckIfItsPopulated();
            Assert.IsTrue(state == System.Data.ConnectionState.Open);
        }

        [TestMethod]
        public void TestDbAccess_GetDistCentres()                          
        {
            //enetConnection.Open();
            List<DistributionCentre> centresList = DataAccess.GetAllDistributionCentres(_connectionString);
            //enetConnection.Close();
            int howMany = centresList.Count();
            Debug.WriteLine(howMany + " centres found.");
            Assert.IsTrue(howMany > 0);
        }

        [TestMethod]
        public void TestDbAccess_GetEmployeesZ()                   
        {
            //enetConnection.Open();
            List<Employee> employeeList = DataAccess.GetAllEmployees(_connectionString);
            //enetConnection.Close();
            int howMany = employeeList.Count();
            Debug.WriteLine(howMany + " employees found.");
            Assert.IsTrue(howMany > 0);
        }
*/

        [TestMethod]
        public void TestDbAccess_GetEmployees()                   
        {
            IEmployeeRepository empRepository = new EmployeeRepository(_connectionString);
            EmployeeService empService = new EmployeeService(empRepository);
            List<Employee> empList = empService.GetAllEmployees();  

            int howMany = empList.Count();
            Debug.WriteLine(howMany + " employees found.");
            Assert.IsTrue(howMany > 0);
        }

 /* 
 
        [TestMethod]
        public void TestDb_ShowAllTables()
        {
            //enetConnection.Open();
            List<DistributionCentre> distList = DataAccess.GetAllDistributionCentres(_connectionString);
            Debug.WriteLine("DISTRIBUTION CENTRES : ");
            foreach (DistributionCentre centre in distList) Debug.WriteLine(centre);

            List<Employee> employeeList = DataAccess.GetAllEmployees(_connectionString);
            Debug.WriteLine("\n\n EMPLOYEES : ");
            foreach (Employee emp in employeeList) Debug.WriteLine(emp);

            List<StandardPackageType> typeList = DataAccess.GetAllStandardPackageTypes(_connectionString);
            Debug.WriteLine("\n\n STANDARD PACKAGE TYPES : ");
            foreach (StandardPackageType t in typeList) Debug.WriteLine(t);

            List<Package> packageList = DataAccess.GetAllPackages(_connectionString);
            Debug.WriteLine("\n\n PACKAGES : ");
            foreach (Package p in packageList) Debug.WriteLine(p);

            List<PackageTransit> transitList = DataAccess.GetAllPackageTransits(_connectionString);
            Debug.WriteLine("\n\n PACKAGE TRANSITS : ");
            foreach (PackageTransit t in transitList) Debug.WriteLine(t);
            //enetConnection.Close();
            Assert.IsNotNull(employeeList);
        }

/*

        [TestMethod]
        public void TestDbAccess_GetPackages()                           
        {
            enetConnection.Open();
            List<Package> packageList = DataAccess.GetAllPackages(_connectionString);
            enetConnection.Close();
            int howMany = packageList.Count();
            Debug.WriteLine(howMany + " packages found.");
            Assert.IsTrue(howMany > 0);
        }


      
         

        [TestMethod]
        public void TestDbAccess_GetPackageTypes()                             
        {
            enetConnection.Open();
            List<StandardPackageType> typeList = DataAccess.GetAllStandardPackageTypes(_connectionString);
            enetConnection.Close();
            int howMany = typeList.Count();
            Debug.WriteLine(howMany + " packTypes found.");
            Assert.IsTrue(howMany > 0);
        }

        [TestMethod]
        public void TestRepository_GetEmployees()                  
        {
            EmployeeRepository eRep = new EmployeeRepository(_connectionString);
            List<Employee> employeeList = eRep.getAllEmployees();
            int howMany = employeeList.Count();
            Debug.WriteLine(howMany + " Employees found via repository.");
            Assert.IsTrue(howMany > 0);
        }

        [TestMethod]
        public void TestRepository_GetPackageTypes()       
        {
            PackageRepository pRep = new PackageRepository(_connectionString);
            List<StandardPackageType> typeList = pRep.GetAllStandardPackageTypes();
            int howMany = typeList.Count();
            Debug.WriteLine(howMany + " packages found via repository.");
            Assert.IsTrue(howMany > 0);
        }


        public void CheckIfItsPopulated()      // Populates Database if it hasnt been poped yet
        {                                       //               (P. 04-04-2015)
            enetConnection.Open();
            List<Employee> employeeList = DataAccess.GetAllEmployees(_connectionString);
            enetConnection.Close();
            if (employeeList.Count() < 8)         // if there curr are less than 8 employees
            {
                //Populator myPopulator = new Populator();
                //myPopulator.Run(enetConnection);
                Debug.WriteLine(" Sample Items were added to Database");
            }
        }

 */ 

    }
}




