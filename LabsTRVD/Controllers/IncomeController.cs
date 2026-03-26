using AutoMapper;
using LabsTRVD.DTOs;
using LabsTRVD.Entities;
using LabsTRVD.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LabsTRVD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncomeController : ControllerBase
    {
        private readonly IIncomeService _incomeService;
        private readonly IMapper _mapper;

        public IncomeController(IIncomeService incomeService, IMapper mapper)
        {
            _incomeService = incomeService;
            _mapper = mapper;
        }

        // GET: api/Income?userId=...
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IncomeDto>>> GetUserIncomes([FromQuery] Guid userId)
        {
            var incomes = await _incomeService.GetUserIncomesAsync(userId);
            var dtos = _mapper.Map<List<IncomeDto>>(incomes);
            return Ok(dtos);
        }

        // GET: api/Income/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<IncomeDto>> GetIncome(int id)
        {
            var income = await _incomeService.GetByIdAsync(id);
            if (income == null) return NotFound();
            return Ok(_mapper.Map<IncomeDto>(income));
        }

        // POST: api/Income
        [HttpPost]
        public async Task<ActionResult> AddIncome([FromBody] IncomeDto dto)
        {
            try
            {
                var income = _mapper.Map<Income>(dto);
                await _incomeService.AddIncomeAsync(income);
                return CreatedAtAction(nameof(GetIncome), new { id = income.IncomeId }, _mapper.Map<IncomeDto>(income));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/Income/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateIncome(int id, [FromBody] IncomeDto dto)
        {
            try
            {
                var income = _mapper.Map<Income>(dto);
                income.IncomeId = id;
                await _incomeService.UpdateIncomeAsync(income);
                return Ok(_mapper.Map<IncomeDto>(income));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/Income/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteIncome(int id)
        {
            try
            {
                await _incomeService.DeleteIncomeAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // GET: api/Income/total?userId=...&from=...&to=...
        [HttpGet("total")]
        public async Task<ActionResult<decimal>> GetTotalForPeriod(
            [FromQuery] Guid userId,
            [FromQuery] DateTime from,
            [FromQuery] DateTime to)
        {
            var total = await _incomeService.GetTotalForPeriodAsync(userId, from, to);
            return Ok(total);
        }

        // GET: api/Income/total/current-month?userId=...
        [HttpGet("total/current-month")]
        public async Task<ActionResult<decimal>> GetTotalCurrentMonth([FromQuery] Guid userId)
        {
            var total = await _incomeService.GetTotalCurrentMonthAsync(userId);
            return Ok(total);
        }

        // GET: api/Income/total/all?userId=...
        [HttpGet("total/all")]
        public async Task<ActionResult<decimal>> GetTotalAll([FromQuery] Guid userId)
        {
            var total = await _incomeService.GetTotalAsync(userId);
            return Ok(total);
        }
    }
}