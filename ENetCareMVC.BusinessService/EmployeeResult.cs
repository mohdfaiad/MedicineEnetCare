using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENetCareMVC.BusinessService
{
    /// <summary>
    /// This class holds constants to be used by the "Result" Class
    /// </summary>
    public static class EmployeeResult
    {
        public const string UserNameCannotByEmpty = "User Name cannot be empty";
        public const string UserNameCannotBeFound = "User Name cannot be found";
        public const string CurrentPasswordIsIncorrect = "Current password is incorrect";
        public const string NewPasswordCannotBeEmpty = "New password cannot be empty";
        public const string RetryPasswordNotTheSameAsTheNewPassword = "Retry password not the same as the new password";

        public const string FullNameCannotByEmpty = "Full Name cannot be empty";
        public const string EmailAddressCannotByEmpty = "Email Address cannot be empty";
        public const string LocationCentreCannotByEmpty = "Location Centre cannot be empty";
    }
}
