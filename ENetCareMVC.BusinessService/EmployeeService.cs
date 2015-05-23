using ENetCareMVC.Repository.Data;
using ENetCareMVC.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENetCareMVC.BusinessService
{
    public class EmployeeService
    {
        private IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public Employee Retrieve(string username)
        {
            if (string.IsNullOrEmpty(username))
                return null;

            return _employeeRepository.Get(null, username);
        }

        public Result ChangePassword(string username, string currentPassword, string newPassword, string retryPassword)
        {
            Result result = new Result
            {
                Success = true
            };

            if (string.IsNullOrEmpty(username))
            {
                result.Success = false;
                result.ErrorMessage = EmployeeResult.UserNameCannotByEmpty;
                return result;
            }

            Employee employee = _employeeRepository.Get(null, username);

            if (employee == null)
            {
                result.Success = false;
                result.ErrorMessage = EmployeeResult.UserNameCannotBeFound;
                return result;
            }

            /*
            if (employee.Password != currentPassword)
            {
                result.Success = false;
                result.ErrorMessage = EmployeeResult.CurrentPasswordIsIncorrect;
                return result;
            }
            */
            if (string.IsNullOrEmpty(newPassword))
            {
                result.Success = false;
                result.ErrorMessage = EmployeeResult.NewPasswordCannotBeEmpty;
                return result;
            }

            if (newPassword != retryPassword)
            {
                result.Success = false;
                result.ErrorMessage = EmployeeResult.RetryPasswordNotTheSameAsTheNewPassword;
                return result;
            }

            //employee.Password = newPassword;

            _employeeRepository.Update(employee);

            result.Id = employee.EmployeeId;            
            return result;
        }

        public Result Update(string username, string fullName, string emailAddress, DistributionCentre locationCentre, EmployeeType employeeType)
        {
            Result result = new Result
            {
                Success = true
            };

            if (string.IsNullOrEmpty(username))
            {
                result.Success = false;
                result.ErrorMessage = EmployeeResult.UserNameCannotByEmpty;
                return result;
            }

            Employee employee = _employeeRepository.Get(null, username);

            if (employee == null)
            {
                result.Success = false;
                result.ErrorMessage = EmployeeResult.UserNameCannotBeFound;
                return result;
            }

            if (string.IsNullOrEmpty(fullName))
            {
                result.Success = false;
                result.ErrorMessage = EmployeeResult.FullNameCannotByEmpty;
                return result;
            }

            if (string.IsNullOrEmpty(emailAddress))
            {
                result.Success = false;
                result.ErrorMessage = EmployeeResult.EmailAddressCannotByEmpty;
                return result;
            }

            if (locationCentre == null)
            {
                result.Success = false;
                result.ErrorMessage = EmployeeResult.LocationCentreCannotByEmpty;
                return result;
            }

            employee.FullName = fullName;
            employee.EmailAddress = emailAddress;
            employee.Location = locationCentre;
            employee.LocationCentreId = locationCentre.CentreId;
            employee.EmployeeType = employeeType;

            _employeeRepository.Update(employee);
            result.Id = employee.EmployeeId;
            return result;
        }

        public List<DistributionCentre> GetAllDistributionCentres()
        {
            return _employeeRepository.GetAllDistributionCentres();
        }

        public DistributionCentre GetDistributionCentre(int centreid)
        {
            return _employeeRepository.GetDistributionCentre(centreid);
        }
    }
}
