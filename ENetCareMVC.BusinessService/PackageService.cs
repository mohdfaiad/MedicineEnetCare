﻿using ENetCareMVC.Repository.Data;
using ENetCareMVC.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENetCareMVC.BusinessService
{
    public class PackageService
    {
        private readonly int zero = 0;
        private IPackageRepository _packageRepository;

        /// <summary>
        /// This constructor simply sets the packageRepository to the one passed as parameter
        /// </summary>
        /// <param name="packageRepository"></param>
        public PackageService(IPackageRepository packageRepository)
        {
            _packageRepository = packageRepository;
        }

        /// <summary>
        /// Calculates Expiration Date based on the Shelf Life settings of the Standard Package Type
        /// </summary>
        /// <param name="packageType"></param>
        /// <param name="startDate"></param>
        /// <returns></returns>
        public DateTime CalculateExpirationDate(StandardPackageType packageType, DateTime startDate)
        {
            if (packageType == null)
                return DateTime.MinValue;

            if (packageType.ShelfLifeUnitType == ShelfLifeUnitType.Month)
                return startDate.AddMonths(packageType.ShelfLifeUnits);
            else
                return startDate.AddDays(packageType.ShelfLifeUnits);
        }

        /// <summary>
        /// Registers and Creates new Packages of a Standard Packahe Type for a given location and
        /// has the specified expiration date. The Barcode is generated from the Package Type Id, Expiration Date and Package Id
        /// </summary>
        /// <param name="packageType"></param>
        /// <param name="location"></param>
        /// <param name="expirationDate"></param>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public Result Register(StandardPackageType packageType, DistributionCentre location, DateTime expirationDate, out string barcode)
        {
            var result = new Result
            {
                Success = true
            };

            barcode = string.Empty;

            if (expirationDate < DateTime.Today)
            {
                result.Success = false;
                result.ErrorMessage = PackageResult.ExpirationDateCannotBeEarlierThanToday;
                return result;
            }

            Package package = new Package
            {
                PackageTypeId = packageType.PackageTypeId,
                CurrentLocationCentreId = location.CentreId,
                CurrentStatus = PackageStatus.InStock,
                ExpirationDate = expirationDate,
                BarCode = "Not Defined"
            };

            int packageId = _packageRepository.Insert(package);
            package.PackageId = packageId;
            barcode = GenerateBarCode(package);
            package.BarCode = barcode;
            _packageRepository.Update(package);
            result.Id = package.PackageId;
            return result;
        }

        /// <summary>
        /// Retrieve a package from its barcode
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public Package Retrieve(string barcode)
        {
            if (string.IsNullOrEmpty(barcode))
                return null;

            return _packageRepository.Get(null, barcode);
        }

        /// <summary>
        /// Returns a List object with all the Standard Package Types in the database
        /// </summary>
        /// <returns> List object </returns>
        public List<StandardPackageType> GetAllStandardPackageTypes()
        {
            return _packageRepository.GetAllStandardPackageTypes();
        }

        /// <summary>
        /// Returns one Standard Package Type from its id
        /// </summary>
        /// <param name="packageId"></param>
        /// <returns></returns>
        public StandardPackageType GetStandardPackageType(int packageId)
        {
            return _packageRepository.GetStandardPackageType(packageId);
        }

        private string GenerateBarCode(Package package)
        {            
            return string.Format("{0:D5}{1:yyMMdd}{2:D5}", package.PackageTypeId, package.ExpirationDate, package.PackageId);
        }

        /// <summary>
        /// Extended Send method which take Sender and Receiver Center and also do more check, 
        /// do some check in package, sender and reciever center and also ckeck the sendDate.
        /// If the checks are passed the set the Package.CurrentStatus = InTransit,
        /// Set the Package.CurrentLocation = Null and Update(Package)
        /// and instantiate new PackageTransit object
        /// </summary>
        /// <param name="barCode"></param>
        /// <param name="senderCentre"></param>
        /// <param name="receiverCentre"></param>
        /// <param name="sendDate"></param>
        /// <returns> Result </returns>
        public Result Send(Package package, DistributionCentre senderCentre, DistributionCentre receiverCentre, DateTime sendDate)
        {                                                          
            Result sendResult = new Result();
            
            if (package == null)                                                  // Case: not found
            {
                sendResult.ErrorMessage = PackageResult.BarCodeNotFound;
                sendResult.Success = false;
                return sendResult;
            }
            if (package.CurrentStatus != PackageStatus.InStock)                   // Case: not in stock 
            {
                sendResult.ErrorMessage = PackageResult.PackageIsNotInStock;
                sendResult.Success = false;
                return sendResult;
            }
            if (package.CurrentLocation.CentreId == null)                         // Cass: Package already in Transit
            {
                sendResult.ErrorMessage = PackageResult.PackageInTransit + " or" + PackageResult.PackageElsewhere;
                sendResult.Success = false;
                return sendResult;
            }
            if (package.CurrentLocation.CentreId != senderCentre.CentreId)        //  Case: not in this centre
            {
                sendResult.ErrorMessage = PackageResult.PackageElsewhere;
                sendResult.Success = false;
                return sendResult;
            }
            if (receiverCentre == null)                                           // Case: Receiver Centre 
            {
                sendResult.ErrorMessage = TransitResult.ReceiverCentreNull;
                sendResult.Success = false;
                return sendResult;
            }
            if (senderCentre.CentreId == receiverCentre.CentreId)
            {
                sendResult.ErrorMessage = TransitResult.PackageAlreadyAtDestination;
                sendResult.Success = false;
                return sendResult;
            }
            
            // System.DateTime.Now.AddDays(-1) -> to get yesterday. might send the package on the same day
            //timeCompare DateTime.Compare(t1,t2) 
            //Less than zero t1 is earlier than t2. | Zero t1 is the same as t2. | Greater than zero t1 is later than t2. 
            int timeCompare = DateTime.Compare(System.DateTime.Now.AddDays(-1), sendDate);
            if (timeCompare > zero)
            {
                sendResult.ErrorMessage = TransitResult.InvalidSendDate;
                sendResult.Success = false;
                return sendResult;
            }
            
            //Update the package
            package.CurrentStatus = PackageStatus.InTransit;        // Proceed to set it as intransit
            package.CurrentLocation = null;                         // Remove current location 
            _packageRepository.Update(package);                     // Update package
            sendResult.Success = true;
            
            //Create new PackageTransit
            PackageTransit packageTransit = new PackageTransit
            {
                Package = package,
                DateSent = sendDate,
                DateReceived = null,
                DateCancelled = null,
                SenderCentre = senderCentre,
                ReceiverCentre = receiverCentre,
            };
            int TransitId = _packageRepository.InsertTransit(packageTransit);
            
            return sendResult;
        }

        /// <summary>
        /// Attempts to update package with "barCode" and its associated transit
        /// sets packages location and status
        /// sets transit's "date received" as today
        /// </summary>
        /// <param name="barCode"></param>
        /// <param name="receiverCentre"></param>
        /// <param name="date"></param>
        /// <returns> Result object according to success or failure </returns>
        public Result Receive(string barCode, DistributionCentre receiverCentre, DateTime date)
        {                                                                 // (P. 24-03-2015)
            Result receiveResult = new Result();
            Package package = _packageRepository.GetPackageWidthBarCode(barCode);
            if (package == null)                         // Case: not found
            {
                receiveResult.ErrorMessage = TransitResult.BarCodeNotFound;
                receiveResult.Success = false;
                return receiveResult;
            }

            PackageTransit activeTransit = _packageRepository.GetTransit(package, null);

            // If there is an active transit set Date Received or Date Cancelled and update
            // Even if there is no transit record the receive should still work
            if (activeTransit != null)
            {
                if (date < activeTransit.DateSent)
                {
                    receiveResult.Success = false;
                    receiveResult.ErrorMessage = PackageResult.ReceiveDateCannotBeEarlierThanSend;
                    return receiveResult;
                }
                
                if (activeTransit.ReceiverCentre.CentreId == receiverCentre.CentreId)
                    activeTransit.DateReceived = date;
                else
                    activeTransit.DateCancelled = date; // something went wrong with the transit so just cancel it

                _packageRepository.UpdateTransit(activeTransit);
            }

            package.CurrentStatus = PackageStatus.InStock;          // set packagestatus
            package.CurrentLocation = receiverCentre;               // set package location
            package.DistributedBy = null;                           // set distributed by employee to null
            _packageRepository.Update(package);                     // update packages DB
            receiveResult.Success = true;
            receiveResult.Id = package.PackageId;
            return receiveResult;
        }

        /// <summary>
        /// Attempts to update Transit with "barCode" and its associated package so as to cancel transit
        /// sets transit's "DateCancelled" as today
        /// </summary>
        /// <param name="barCode"></param>
        /// <param name="dateCancelled"></param>
        /// <returns> Result object according to success or failure </returns>
        public Result CancelTransit(string barCode, DateTime dateCancelled)        // (P. 07-04-2015)
        {
            Result cResult = new Result();
            Package package = _packageRepository.GetPackageWidthBarCode(barCode);
            if (package == null)                         // Case: not found
            {
                cResult.ErrorMessage = TransitResult.BarCodeNotFound;
                cResult.Success = false;
                return cResult;
            }
            List<PackageTransit> activeTransits = _packageRepository.GetActiveTransitsByPackage(package);
            if (activeTransits.Count() == 0)                         // Case: not found
            {
                cResult.ErrorMessage = TransitResult.TransitNotFound;
                cResult.Success = false;
                return cResult;
            }
            PackageTransit transit = activeTransits.ElementAt(0);
            transit.DateCancelled = DateTime.Today;                  // set transit as cancelled
            _packageRepository.UpdateTransit(transit);              // update transits DB
            package.CurrentStatus = PackageStatus.Lost;             // set packagestatus
            _packageRepository.Update(package);                     // update packages DB
            cResult.Success = true;
            return cResult;
        }

        /// <summary>
        /// Attempts to update package with "packageId" so as to distibute it, if currently logged employee is autorized 
        /// </summary>
        /// <param name="barCode"></param>
        /// <param name="distributionCentre"></param>
        /// <param name="employee"></param>
        /// <param name="expirationDate"></param>
        /// <param name="packageType"></param>
        /// <param name="packageId"></param>
        /// <returns> Result object according to success or failure </returns>
        public Result Distribute(string barCode, DistributionCentre distributionCentre, Employee employee, DateTime expirationDate, StandardPackageType packageType, int packageId)
        {
            var result = new Result
            {
                Success = true
            };

            if (employee.EmployeeType == EmployeeType.Manager)
            {
                result.Success = false;
                result.ErrorMessage = PackageResult.EmployeeNotAuthorized;
                return result;
            }

            Package package = _packageRepository.GetPackageWidthBarCode(barCode);
            if(package.CurrentStatus==PackageStatus.Distributed)
            {
                result.Success = false;
                result.ErrorMessage = PackageResult.PackageAlreadyDistributed;
                return result;
            }

            Package package2 = new Package
            {
                PackageType = packageType,
                CurrentLocation = distributionCentre,
                CurrentStatus = PackageStatus.Distributed,
                PackageId = packageId,
                ExpirationDate = expirationDate,
                DistributedBy = employee,
                BarCode = barCode
            };

            _packageRepository.Update(package2);

            result.Id = package.PackageId;

            return result;
        }

        /// <summary>
        /// Attempts to update package with "packageId" to discard it, if currently logged employee is autorized
        /// Sets package's status as discarded
        /// </summary>
        /// <param name="barCode"></param>
        /// <param name="distributionCentre"></param>
        /// <param name="employee"></param>
        /// <param name="expirationDate"></param>
        /// <param name="packageType"></param>
        /// <param name="packageId"></param>
        /// <returns> Result object according to success or failure </returns>
        public Result Discard(string barCode, DistributionCentre distributionCentre, Employee employee, DateTime expirationDate, StandardPackageType packageType, int packageId)
        {
            var result = new Result
            {
                Success = true
            };

            if (employee.EmployeeType == EmployeeType.Manager)
            {
                result.Success = false;
                result.ErrorMessage = PackageResult.EmployeeNotAuthorized;
                return result;
            }

            Package package = new Package
            {
                PackageType = packageType,
                CurrentLocation = distributionCentre,
                CurrentStatus = PackageStatus.Discarded,
                PackageId = packageId,
                ExpirationDate = expirationDate,
                DistributedBy = null,
                BarCode = barCode
            };

            _packageRepository.Update(package);

            result.Id = package.PackageId;

            return result;
        }

        /// <summary>
        /// For a given Standard Package Type and a list of scanned barcodes work out which packages are Lost
        /// and which packages have been found. Tidy up the transit rows for packages for newly found packages.
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="packageType"></param>
        /// <param name="barCodes"></param>
        /// <returns></returns>
        public Result PerformAudit(Employee employee, StandardPackageType packageType, List<string> barCodes)
        {
            Result result = new Result
            {
                Success = true
            };
            int auditId = _packageRepository.InsertAudit(employee, packageType, barCodes);
            result.Id = auditId;

            _packageRepository.UpdateLostFromAudit(auditId, employee.Location, packageType);

            _packageRepository.UpdateInstockFromAudit(auditId, employee.Location, packageType);

            _packageRepository.UpdateTransitReceivedFromAudit(auditId, employee.Location);

            _packageRepository.UpdateTransitCancelledFromAudit(auditId, employee.Location);
            return result;
        }

    }
}
