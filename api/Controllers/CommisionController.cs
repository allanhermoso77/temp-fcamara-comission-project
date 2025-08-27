using FCamara.CommissionCalculator.Models;
using FCamara.CommissionCalculator.Services;
using Microsoft.AspNetCore.Mvc;

namespace FCamara.CommissionCalculator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommisionController : ControllerBase
    {
        private readonly ICommissionService _commissionService;

        public CommisionController(ICommissionService commissionService)
        {
            _commissionService = commissionService;
        }

        [ProducesResponseType(typeof(CommissionCalculationResponse), 200)]
        [HttpPost]
        public IActionResult Calculate(CommissionCalculationRequest calculationRequest)
        {
            if (calculationRequest == null)
                return BadRequest("Invalid request");

            if (!ModelState.IsValid)
                return BadRequest(ValidationProblem(ModelState));

            var result = _commissionService.Calculate(calculationRequest);
            return Ok(result);
        }
    }    
}
