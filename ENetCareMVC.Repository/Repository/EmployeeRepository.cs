using ENetCareMVC.Repository.Data;
using ENetCareMVC.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENetCare.Repository.Repository
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
            DataAccess.UpdateEmployee(employee);
            return;
        }

        public Employee Get(int? employeeId, string username)
        {
            Employee employee = null;

                employee = DataAccess.GetEmployee(employeeId, username);
                if (employee != null)
                    employee.Location = DataAccess.GetDistributionCentre(employee.Location.CentreId);

            return employee;
        }

        public List<DistributionCentre> GetAllDistributionCentres()
        {
            List<DistributionCentre> centres = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                centres = DataAccess.GetAllDistributionCentres();
                connection.Close();
            }
            return centres;
        }

        public DistributionCentre GetDistributionCentre(int centreid)
        {
            DistributionCentre centre = null;
                centre = DataAccess.GetDistributionCentre(centreid);
            return centre;
        }

        public List<Employee> getAllEmployees()
        {                                                             //  (P. 04-04-2015)
                List<Employee> eList = DataAccess.GetAllEmployees();
                return eList;
        }

        public DistributionCentre GetHeadOffice()
        {                                                               // (P. 05-04-2015)
                List<DistributionCentre> allCentres = DataAccess.GetAllDistributionCentres();
                foreach (DistributionCentre centre in allCentres)
                    if (centre.IsHeadOffice) return centre;

            return null;
        }


    }
}
