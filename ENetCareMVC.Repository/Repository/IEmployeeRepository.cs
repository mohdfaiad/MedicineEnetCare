using System.Collections.Generic;
using ENetCareMVC.Repository.Data;

namespace ENetCareMVC.Repository.Repository
{
    public interface IEmployeeRepository
    {
        void Update(Employee employee);
        Employee Get(int? employeeId, string username);
        List<DistributionCentre> GetAllDistributionCentres();
        List<Employee> GetAllEmployees();
        DistributionCentre GetDistributionCentre(int centreid);
    }
}
