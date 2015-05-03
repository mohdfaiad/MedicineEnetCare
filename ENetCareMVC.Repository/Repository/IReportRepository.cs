using ENetCareMVC.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENetCareMVC.Repository.Repository
{
    public interface IReportRepository
    {
        List<DistributionCentreStock> GetDistributionCentreStock();
        List<DistributionCentreLoss> GetDistributionCentreLosses();
        List<DoctorActivity> GetDoctorActivity();
        List<GlobalStock> GetGlobalStock();
        List<ValueInTransit> GetValueInTransit();
        //List<ReconciledPackage> GetReconciledPackages(DistributionCentre currentLocation, StandardPackageType packageType, List<string> barCodeList);
        List<StockTaking> GetStocktaking(int CentreId);
    }
}
