using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENetCareMVC.BusinessService
{
    public class PackageResult
    {
        public const string BarCodeNotFound = "Bar Code not found";
        public const string NoBarCodesSelected = "There are no Bar Codes selected";
        public const string PackageElsewhere = "That Package is NOT located in this distribution centre";
        public const string PackageIsNotInStock = "That Package is Not in Stock";
        public const string PackageAlreadyDistributed = "That Package has already been distributed";
        public const string PackageInTransit = "That Package is in Transit";
        public const string PackageAlreadyDiscarded = "That Package has been Discarded";
        public const string PackageNotExpired = "That package cannot be discarded until it has expired on ";
        public const string PackageIsLost = "Unfortunately That package has been lost";
        public const string PackageHasExpired = "That package has expired, cannot distribute an expired package";
        public const string EmployeeNotAuthorized = "You are not authorized to distribute packages";
        public const string EmployeeNotAuthorizedDiscard = "You are not authorized to discard packages";
        public const string EmployeeInDifferentLocation = "That Package is NOT located in this distribution centre";
        public const string ExpirationDateCannotBeEarlierThanToday = "The expiration date cannot be earlier than today";
        public const string ReceiveDateCannotBeEarlierThanSend = "The receive date cannot be earlier than the send date";
    }
}
