using LabsTRVD.DTOs.ServicesDTOs;
using LabsTRVD.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LabsTRVD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        // GET: api/Expense/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserExpenses(Guid userId)
        {
            var dtos = await _expenseService.GetUserExpensesAsync(userId);
            return Ok(dtos);
        }

        // GET: api/Expense/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var expense = await _expenseService.GetByIdAsync(id);
            if (expense == null) return NotFound();

            return Ok(expense);
        }

        // POST: api/Expense
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExpenseDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _expenseService.AddExpenseAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.ExpenseId }, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message, error = "Validation Error" });
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new 
                { 
                    message = "Помилка при збереженні в базі даних",
                    details = ex.InnerException?.Message ?? ex.Message,
                    error = "Database Error"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new 
                { 
                    message = ex.Message,
                    error = ex.GetType().Name,
                    stackTrace = ex.StackTrace
                });
            }
        }

        // PUT: api/Expense/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ExpenseDto dto)
        {
            try
            {
                var result = await _expenseService.UpdateExpenseAsync(id, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("не знайдена"))
                    return NotFound(new { message = ex.Message });

                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/Expense/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _expenseService.DeleteExpenseAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("не знайдена"))
                    return NotFound(new { message = ex.Message });

                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET: api/Expense/user/{userId}/total?from=...&to=...
        [HttpGet("user/{userId}/total")]
        public async Task<IActionResult> GetTotalForPeriod(Guid userId, DateTime from, DateTime to)
        {
            var total = await _expenseService.GetTotalForPeriodAsync(userId, from, to);
            return Ok(new { total });
        }

        // GET: api/Expense/user/{userId}/total/current-month
        [HttpGet("user/{userId}/total/current-month")]
        public async Task<IActionResult> GetTotalCurrentMonth(Guid userId)
        {
            var total = await _expenseService.GetTotalCurrentMonthAsync(userId);
            return Ok(new { total });
        }

        // GET: api/Expense/user/{userId}/total/all
        [HttpGet("user/{userId}/total/all")]
        public async Task<IActionResult> GetTotal(Guid userId)
        {
            var total = await _expenseService.GetTotalAsync(userId);
            return Ok(new { total });
        }
    }
}