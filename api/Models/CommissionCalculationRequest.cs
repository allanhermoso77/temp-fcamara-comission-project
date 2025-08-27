using System.ComponentModel.DataAnnotations;

namespace FCamara.CommissionCalculator.Models
{
    public class CommissionCalculationRequest
    {
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "LocalSalesCount must be >= 0")]
        public int LocalSalesCount { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "ForeignSalesCount must be >= 0")]
        public int ForeignSalesCount { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "AverageSaleAmount must be > 0")]
        public decimal AverageSaleAmount { get; set; }
    }
}
