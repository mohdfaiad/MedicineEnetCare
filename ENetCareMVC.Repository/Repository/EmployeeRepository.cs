using ENetCareMVC.Repository.Data;
using ENetCareMVC.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENetCareMVC.Repository.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private string _connectionString;

        public EmployeeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void Update(Employee employee)
        {
            DataAccess.UpdateEmployee(_connectionString, employee);
            return;
        }

        public Employee Get(int? employeeId, string username)
        {
            Employee employee = null;

            employee = DataAccess.GetEmployee(_connectionString, employeeId, username);
            return employee;
        }

        public List<DistributionCentre> GetAllDistributionCentres()
        {
            List<DistributionCentre> centres = null;
            centres = DataAccess.GetAllDistributionCentres(_connectionString);

            return centres;
        }

        public DistributionCentre GetDistributionCentre(int centreid)
        {
            DistributionCentre centre = null;
            centre = DataAccess.GetDistributionCentre(_connectionString, centreid);
            return centre;
        }

        public List<Employee> getAllEmployees()
        {                                                             //  (P. 04-04-2015)
            List<Employee> eList = DataAccess.GetAllEmployees(_connectionString);
            return eList;
        }

        public DistributionCentre GetHeadOffice()
        {                                                               // (P. 05-04-2015)
            List<DistributionCentre> allCentres = DataAccess.GetAllDistributionCentres(_connectionString);
            foreach (DistributionCentre centre in allCentres)
                if (centre.IsHeadOffice) return centre;

            return null;
        }


    }
}
