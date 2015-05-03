using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENetCare.Repository.Repository
{
        public class PackageTransitRepository    // PackageTransitRepository
        {

            /*


            private string _connectionString;

            public PackageTransitRepository(string connectionString)
            {
                _connectionString = connectionString;
            }
            public void Update(PackageTransit transit)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    DataAccess.UpdatePackageTransit(connection, transit);
                }
                return;
            }

            
            public List<PackageTransit> GetAllPackageTransits()
            {                                                                        //   Added by Pablo on 23-03-15
                List<PackageTransit> allTransits = null;
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    allTransits = DataAccess.getAllPackageTransits(connection);
                }
                return allTransits;
            }
            
             
            public List<PackageTransit> GetTransitsByPackage(Package xPackage)     // Added by Pablo on 23-03-15
            {
                List<PackageTransit> allTransits = this.GetAllPackageTransits();
                List<PackageTransit> myTransits = new List<PackageTransit>();        // create empty list
                foreach (PackageTransit p in allTransits)
                { if (p.Package == xPackage) myTransits.Add(p); }
                return myTransits;
            }

            public List<PackageTransit> GetActiveTransitsByPackage(Package xPackage)     // Added by Pablo on 23-03-15
            {
                List<PackageTransit> allTransits = this.GetAllPackageTransits();
                List<PackageTransit> myTransits = new List<PackageTransit>();        // create empty list
                foreach (PackageTransit t in allTransits)
                    {
                    if (t.Package == xPackage && t.DateReceived == null && t.DateCancelled == null) myTransits.Add(t);
                    }
                return myTransits;
            }

            public PackageTransit getTransitById(int id)
            {                                                                       // Added by Pablo on 23-03-15
                List<PackageTransit> allTransits = this.GetAllPackageTransits();
                foreach (PackageTransit p in allTransits) { if (p.TransitId == id) return p; }
                return null;
            }

            public List<PackageTransit> getTransitsBySender(DistributionCentre senderCentre)
            {                                                                 // Added by Pablo on 23-03-15
                List<PackageTransit> allTransits = this.GetAllPackageTransits();     // get all transits
                List<PackageTransit> myTransits = new List<PackageTransit>();        // create empty list 
                foreach (PackageTransit p in allTransits)
                { if (p.SenderCentre.CentreId == senderCentre.CentreId) myTransits.Add(p); }
                return myTransits;                                          // return subset of transits
            }

            public List<PackageTransit> getTransitsByReceiver(DistributionCentre receiverCentre)
            {                                                                 // Added by Pablo on 23-03-15
                List<PackageTransit> allTransits = this.GetAllPackageTransits();     // get all transits
                List<PackageTransit> myTransits = new List<PackageTransit>();        // create empty list 
                foreach (PackageTransit p in allTransits)
                { if (p.ReceiverCentre.CentreId == receiverCentre.CentreId) myTransits.Add(p); }
                return myTransits;                                          // return subset of transits
            }


            */

        
    }
}
