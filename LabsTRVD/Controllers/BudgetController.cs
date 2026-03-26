using LabsTRVD.DTOs;
using LabsTRVD.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LabsTRVD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetService _budgetService;

        public BudgetController(IBudgetService budgetService)
        {
            _budgetService = budgetService;
        }

        // POST: api/Budget
        [HttpPost]
        public async Task<IActionResult> SetBudget([FromBody] BudgetDto dto)
        {
            try
            {
                await _budgetService.SetBudgetAsync(dto.UserId, dto.Month, dto.Year, dto.MonthlyLimit);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/Budget/{userId}/summary?month=3&year=2026
        [HttpGet("{userId}/summary")]
        public async Task<IActionResult> GetBudgetSummary(Guid userId, [FromQuery] int month, [FromQuery] int year)
        {
            try
            {
                var summary = new BudgetSummaryDto
                {
                    Limit = await _budgetService.GetMonthlyLimitAsync(userId, month, year),
                    Used = await _budgetService.GetUsedAmountAsync(userId, month, year),
                    Remaining = await _budgetService.GetRemainingBudgetAsync(userId, month, year),
                    UsagePercentage = await _budgetService.GetUsagePercentageAsync(userId, month, year),
                    Exceeded = await _budgetService.IsBudgetExceeded(userId, month, year)
                };

                return Ok(summary);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}