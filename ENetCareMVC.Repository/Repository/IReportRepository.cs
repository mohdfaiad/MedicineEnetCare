using System.Collections.Generic;
using ENetCareMVC.Repository.Data;

namespace ENetCareMVC.Repository.Repository
{
    public interface IReportRepository
    {
        List<DistributionCentreStock> GetDistributionCentreStock();
        List<DistributionCentreLosses> GetDistributionCentreLosses();
        List<DoctorActivity> GetDoctorActivity();
        List<GlobalStock> GetGlobalStock();
        List<ValueInTransit> GetValueInTransit();
        List<ReconciledPackage> GetReconciledPackages(DistributionCentre currentLocation, StandardPackageType packageType, List<string> barCodeList);
        List<StockTaking> GetStocktaking(int CentreId);
    }
}
