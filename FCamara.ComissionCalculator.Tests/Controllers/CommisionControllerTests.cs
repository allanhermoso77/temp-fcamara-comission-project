using FCamara.CommissionCalculator.Controllers;
using FCamara.CommissionCalculator.Models;
using FCamara.CommissionCalculator.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FCamara.ComissionCalculator.Tests.Controllers
{
    public class CommisionControllerTests
    {
        [Fact]
        public void Post_ReturnsOk_WithCalculatedResponse()
        {
            // Arrange
            var request = new CommissionCalculationRequest
            {
                LocalSalesCount = 10,
                ForeignSalesCount = 10,
                AverageSaleAmount = 100m
            };

            var expected = new CommissionCalculationResponse
            {
                FCamaraCommissionAmount = 550m,
                CompetitorCommissionAmount = 95.5m
            };

            var service = new Mock<ICommissionService>();
            service.Setup(s => s.Calculate(request)).Returns(expected);

            var controller = new CommisionController(service.Object);

            // Act
            var result = controller.Calculate(request);

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.StatusCode.Should().Be(200);
            ok.Value.Should().BeEquivalentTo(expected);

            service.Verify(s => s.Calculate(request), Times.Once);
        }

        [Fact]
        public void Post_ReturnsBadRequest_WhenRequestIsNull()
        {
            // Arrange
            var service = new Mock<ICommissionService>();
            var controller = new CommisionController(service.Object);

            // Act
            var result = controller.Calculate(null!);

            // Assert
            var bad = result as BadRequestObjectResult;
            bad.Should().NotBeNull();
            bad!.StatusCode.Should().Be(400);
            service.Verify(s => s.Calculate(It.IsAny<CommissionCalculationRequest>()), Times.Never);
        }

        [Fact]
        public void Post_ReturnsBadRequest_WhenModelStateInvalid()
        {
            // Arrange
            var mock = new Mock<ICommissionService>();
            var controller = new CommisionController(mock.Object);

            // Make ModelState invalid (as if data annotations failed)
            controller.ModelState.AddModelError(nameof(CommissionCalculationRequest.AverageSaleAmount), "AverageSaleAmount must be > 0");

            var req = new CommissionCalculationRequest
            {
                LocalSalesCount = 10,
                ForeignSalesCount = 10,
                AverageSaleAmount = 0
            };

            // Act
            var action = controller.Calculate(req);

            // Assert
            var bad = action as ObjectResult;
            bad.Should().NotBeNull();
            bad!.StatusCode.Should().Be(400);

            // Service should not be called on invalid model
            mock.Verify(s => s.Calculate(It.IsAny<CommissionCalculationRequest>()), Times.Never);
        }
    }
}
