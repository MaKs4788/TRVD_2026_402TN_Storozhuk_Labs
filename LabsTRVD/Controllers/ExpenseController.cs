using AutoMapper;
using LabsTRVD.DTOs;
using LabsTRVD.Entities;
using LabsTRVD.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LabsTRVD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        private readonly IMapper _mapper;

        public ExpenseController(IExpenseService expenseService, IMapper mapper)
        {
            _expenseService = expenseService;
            _mapper = mapper;
        }

        // GET: api/Expense/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserExpenses(Guid userId)
        {
            var expenses = await _expenseService.GetUserExpensesAsync(userId);
            var dtos = _mapper.Map<List<ExpenseDto>>(expenses);
            return Ok(dtos);
        }

        // GET: api/Expense/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var expense = await _expenseService.GetByIdAsync(id);
            if (expense == null) return NotFound();

            var dto = _mapper.Map<ExpenseDto>(expense);
            return Ok(dto);
        }

        // POST: api/Expense
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExpenseDto dto)
        {
            try
            {
                var expense = _mapper.Map<Expense>(dto);
                await _expenseService.AddExpenseAsync(expense);

                var resultDto = _mapper.Map<ExpenseDto>(expense);
                return CreatedAtAction(nameof(GetById), new { id = expense.ExpenseId }, resultDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/Expense/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ExpenseDto dto)
        {
            try
            {
                var expense = _mapper.Map<Expense>(dto);
                expense.ExpenseId = id;

                await _expenseService.UpdateExpenseAsync(expense);

                var resultDto = _mapper.Map<ExpenseDto>(expense);
                return Ok(resultDto);
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