using FCamara.CommissionCalculator.Configuration;
using FCamara.CommissionCalculator.Models;
using Microsoft.Extensions.Options;

namespace FCamara.CommissionCalculator.Services
{
    public class CommissionService : ICommissionService
    {
        private readonly CommissionRates _rates;

        public CommissionService(IOptions<CommissionRates> options)
        {
            _rates = options.Value;
        }

        public CommissionCalculationResponse Calculate(CommissionCalculationRequest request)
        {
            var fcamaraLocal = request.LocalSalesCount * request.AverageSaleAmount * _rates.FcamaraLocal;
            var fcamaraForeign = request.ForeignSalesCount * request.AverageSaleAmount * _rates.FcamaraForeign;
            var totalFcamara = fcamaraLocal + fcamaraForeign;

            var competitorLocal = request.LocalSalesCount * request.AverageSaleAmount * _rates.CompetitorLocal;
            var competitorForeign = request.ForeignSalesCount * request.AverageSaleAmount * _rates.CompetitorForeign;
            var totalCompetitor = competitorLocal + competitorForeign;

            return new CommissionCalculationResponse
            {
                FCamaraCommissionAmount = totalFcamara,
                CompetitorCommissionAmount = totalCompetitor
            };
        }
    }
}
