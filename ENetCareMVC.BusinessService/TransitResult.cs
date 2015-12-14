namespace ENetCareMVC.BusinessService
{
    public class TransitResult
    {

        public const string BarCodeNotFound = "Bar Code not found";
        public const string PackageElsewhere = "Package appears as located elsewhere";
        public const string PackageNotInStock = "Package appears not to be in Stock";
        public const string PackageAlreadyAtDestination = "Package appears as being already at the Destination Centre";
        public const string TransitNotFound = "Transit not found";
        public const string MoreThanOneTransitForPackage = "More than one active transit exists for that package";
        public const string WrongReceiver = "Package was supposed to arrive at a different centre.";
        public const string ReceiverCentreNull = "Please Select the Correct Receiver Centre";
        public const string InvalidSendDate = "Please select the Correct Date to send the Package.";
    }
}
