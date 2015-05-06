using ENetCareMVC.Repository.Data;
using ENetCareMVC.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENetCareMVC.BusinessService
{
    public class ReportService
    {
        private IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }
        
        /// <summary>
        /// Report Service for Distribution Centre Stock Report
        /// </summary>
        /// <returns></returns>
        public List<DistributionCentreStock> GetDistributionCentreStock()
        {
            return _reportRepository.GetDistributionCentreStock();
        }

        /// <summary>
        /// Report Service for Distribution Centre Losses Report
        /// </summary>
        /// <returns></returns>
        public List<DistributionCentreLosses> GetDistributionCentreLosses()
        {
            return _reportRepository.GetDistributionCentreLosses();
        }

        /// <summary>
        /// Report Service for Doctor Activity Report
        /// </summary>
        /// <returns></returns>
        public List<DoctorActivity> GetDoctorActivity()
        {
            return _reportRepository.GetDoctorActivity();
        }

        /// <summary>
        /// Report Service for Global Stock Report
        /// </summary>
        /// <returns></returns>
        public List<GlobalStock> GetGlobalStock()
        {
            return _reportRepository.GetGlobalStock();
        }

        /// <summary>
        /// Report Service for Stocktaking Report
        /// </summary>
        /// <param name="centreId"></param>
        /// <returns></returns>
        public List<StockTaking> GetStocktaking(int centreId)
        {
            return _reportRepository.GetStocktaking(centreId);
        }
  
        /// <summary>
        /// Report Service for Value In Transit Report
        /// </summary>
        /// <returns></returns>
        public List<ValueInTransit> GetValueInTransit()
        {
            return _reportRepository.GetValueInTransit();
        }

        /// <summary>
        /// Report Service for Reconciled Package screen in Package Audit Wizard
        /// </summary>
        /// <param name="currentLocation"></param>
        /// <param name="packageType"></param>
        /// <param name="barCodeList"></param>
        /// <returns></returns>
        public List<ReconciledPackage> GetReconciledPackages(DistributionCentre currentLocation, StandardPackageType packageType, List<string> barCodeList)
        {
            return _reportRepository.GetReconciledPackages(currentLocation, packageType, barCodeList);
        }
    }
}
