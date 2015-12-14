using ENetCareMVC.Repository;
using System.Collections.Generic;
using ENetCareMVC.Repository.Data;
using ENetCareMVC.Repository.Repository;

namespace ENetCareMVC.UnitTest
{
    public class MockEmployeeRepository : IEmployeeRepository
    {

        public MockEmployeeRepository()          // Constructor              (P. 05-04-2015)
        {
            MockDataAccess.LoadMockTables();
        }

        public void Update(Employee employee)                                 // (P. 04-04-2015)
        {
            MockDataAccess.UpdateEmployee(employee);    
        }

        public List<Repository.Data.DistributionCentre> GetAllDistributionCentres()     // (P. 04-04-2015)
        {
            return MockDataAccess.GetAllDistibutionCentres();
        }

        public List<Employee> GetAllEmployees()     // (P. 28-05-2015)
        {
            return MockDataAccess.GetAllEmployees();
        }


        public Repository.Data.DistributionCentre GetDistributionCentre(int centreId)       // (P. 04-04-2015)
        {
            return MockDataAccess.GetDistributionCentre(centreId);
        }

        public Employee Get(int? employeeId, string username)
        {
            List<Employee> employees = MockDataAccess.GetAllEmployees();

            for (int i = 0; i < employees.Count; i++)
            {
                if (employees[i].UserName == username)
                {
                    return employees[i];
                }
            }

            return null;
        }     
    }
}
