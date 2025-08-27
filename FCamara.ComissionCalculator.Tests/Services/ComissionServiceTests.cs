namespace FCamara.ComissionCalculator.Tests.Services
{
    using FCamara.CommissionCalculator.Configuration;
    using FCamara.CommissionCalculator.Models;
    using FCamara.CommissionCalculator.Services;
    using FluentAssertions;
    using Microsoft.Extensions.Options;
    using Xunit;

    public class CommissionServiceTests
    {
        private static CommissionService CreateService(CommissionRates rates) =>
            new CommissionService(Options.Create(rates));

        [Fact]
        public void Calculate_WithTypicalInputs_ReturnsExpectedTotals()
        {
            // Arrange
            var rates = new CommissionRates
            {
                FcamaraLocal = 0.20m,
                FcamaraForeign = 0.35m,
                CompetitorLocal = 0.02m,
                CompetitorForeign = 0.0755m
            };
            var svc = CreateService(rates);

            var req = new CommissionCalculationRequest
            {
                LocalSalesCount = 10,
                ForeignSalesCount = 10,
                AverageSaleAmount = 100m
            };

            // Act
            var res = svc.Calculate(req);

            // Assert
            res.FCamaraCommissionAmount.Should().Be(550m);      // 20%*10*100 + 35%*10*100
            res.CompetitorCommissionAmount.Should().Be(95.5m);  // 2%*10*100 + 7.55%*10*100
        }

        [Fact]
        public void Calculate_WithZeroCounts_ReturnsZero()
        {
            var rates = new CommissionRates
            {
                FcamaraLocal = 0.20m,
                FcamaraForeign = 0.35m,
                CompetitorLocal = 0.02m,
                CompetitorForeign = 0.0755m
            };
            var svc = CreateService(rates);

            var req = new CommissionCalculationRequest
            {
                LocalSalesCount = 0,
                ForeignSalesCount = 0,
                AverageSaleAmount = 100m
            };

            var res = svc.Calculate(req);

            res.FCamaraCommissionAmount.Should().Be(0m);
            res.CompetitorCommissionAmount.Should().Be(0m);
        }

        [Fact]
        public void Calculate_RespectsConfiguredRates_DifferentNumbers()
        {
            // Different rates to ensure we’re not hardcoding math
            var rates = new CommissionRates
            {
                FcamaraLocal = 0.10m,
                FcamaraForeign = 0.30m,
                CompetitorLocal = 0.05m,
                CompetitorForeign = 0.06m
            };
            var svc = CreateService(rates);

            var req = new CommissionCalculationRequest
            {
                LocalSalesCount = 3,
                ForeignSalesCount = 2,
                AverageSaleAmount = 250m
            };

            // Expected:
            // FCamara = (3*250*0.10) + (2*250*0.30) = 75 + 150 = 225
            // Competitor = (3*250*0.05) + (2*250*0.06) = 37.5 + 30 = 67.5
            var res = svc.Calculate(req);

            res.FCamaraCommissionAmount.Should().Be(225m);
            res.CompetitorCommissionAmount.Should().Be(67.5m);
        }

        [Fact]
        public void Calculate_DecimalPrecision_IsStable()
        {
            var rates = new CommissionRates
            {
                FcamaraLocal = 0.1234m,
                FcamaraForeign = 0.3456m,
                CompetitorLocal = 0.0101m,
                CompetitorForeign = 0.0707m
            };
            var svc = CreateService(rates);

            var req = new CommissionCalculationRequest
            {
                LocalSalesCount = 7,
                ForeignSalesCount = 5,
                AverageSaleAmount = 19.99m
            };

            var res = svc.Calculate(req);

            // Compute expected with decimals (not doubles)
            decimal fLocal = 7 * 19.99m * 0.1234m;
            decimal fForeign = 5 * 19.99m * 0.3456m;
            decimal cLocal = 7 * 19.99m * 0.0101m;
            decimal cForeign = 5 * 19.99m * 0.0707m;

            res.FCamaraCommissionAmount.Should().Be(fLocal + fForeign);
            res.CompetitorCommissionAmount.Should().Be(cLocal + cForeign);
        }
    }
}
