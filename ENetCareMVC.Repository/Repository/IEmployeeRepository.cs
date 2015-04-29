using ENetCareMVC.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENetCareMVC.Repository.Repository
{
    public interface IEmployeeRepository
    {
        void Update(Employee employee);

        Employee Get(int? employeeId, string username);

        List<DistributionCentre> GetAllDistributionCentres();

        DistributionCentre GetDistributionCentre(int centreid);


    }
}
