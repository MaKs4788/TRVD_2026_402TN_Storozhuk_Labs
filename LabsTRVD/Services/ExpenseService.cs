using AutoMapper;
using LabsTRVD.DTOs.ServicesDTOs;
using LabsTRVD.Entities;
using LabsTRVD.Interfaces.Repositories;
using LabsTRVD.Interfaces.Services;

namespace LabsTRVD.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IMapper _mapper;

        public ExpenseService(IExpenseRepository expenseRepository, IMapper mapper)
        {
            _expenseRepository = expenseRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ExpenseDtoResponse>> GetUserExpensesAsync(Guid userId)
        {
            var expenses = await _expenseRepository.GetByUserAsync(userId);
            return _mapper.Map<IEnumerable<ExpenseDtoResponse>>(expenses);
        }

        public async Task<IEnumerable<ExpenseDtoResponse>> GetByPeriodAsync(Guid userId, DateTime from, DateTime to)
        {
            var expenses = await _expenseRepository.FindAsync(e =>
                e.UserId == userId &&
                e.Date >= from &&
                e.Date <= to);
            return _mapper.Map<IEnumerable<ExpenseDtoResponse>>(expenses);
        }

        public async Task<ExpenseDtoResponse?> GetByIdAsync(int id)
        {
            var expense = await _expenseRepository.GetByIdAsync(id);
            return expense == null ? null : _mapper.Map<ExpenseDtoResponse>(expense);
        }

        public async Task<ExpenseDtoResponse> AddExpenseAsync(ExpenseDto expenseDto)
        {
            if (expenseDto.Amount <= 0)
                throw new Exception("Сума повинна бути більше 0");

            if (expenseDto.UserId == Guid.Empty)
                throw new Exception("UserId не вказаний");

            var expense = _mapper.Map<Expense>(expenseDto);
            if (expense.Date == default)
                expense.Date = DateTime.Now;

            if (expense.CategoryId == 0)
                expense.CategoryId = null;

            await _expenseRepository.AddAsync(expense);
            return _mapper.Map<ExpenseDtoResponse>(expense);
        }

        public async Task<ExpenseDtoResponse> UpdateExpenseAsync(int id, ExpenseDto expenseDto)
        {
            if (expenseDto.Amount <= 0)
                throw new Exception("Сума повинна бути більше 0");

            var existing = await _expenseRepository.GetByIdAsync(id);
            if (existing == null)
                throw new Exception("Витрата не знайдена");

            existing.Amount = expenseDto.Amount;
            existing.Date = expenseDto.Date;
            existing.Description = expenseDto.Description;

            // Трактуємо CategoryId = 0 як null (без категорії)
            existing.CategoryId = expenseDto.CategoryId == 0 ? null : expenseDto.CategoryId;

            await _expenseRepository.UpdateAsync(existing);
            return _mapper.Map<ExpenseDtoResponse>(existing);
        }

        public async Task DeleteExpenseAsync(int id)
        {
            var expense = await _expenseRepository.GetByIdAsync(id);
            if (expense == null)
                throw new Exception("Витрата не знайдена");

            await _expenseRepository.DeleteAsync(expense);
        }

        public async Task<decimal> GetTotalForPeriodAsync(Guid userId, DateTime from, DateTime to)
        {
            var expenses = await _expenseRepository.FindAsync(e =>
                e.UserId == userId &&
                e.Date >= from &&
                e.Date <= to);
            return expenses.Sum(e => e.Amount);
        }

        public async Task<decimal> GetTotalCurrentMonthAsync(Guid userId)
        {
            var now = DateTime.Now;
            var from = new DateTime(now.Year, now.Month, 1);
            var to = from.AddMonths(1).AddTicks(-1);

            return await GetTotalForPeriodAsync(userId, from, to);
        }

        public async Task<decimal> GetTotalAsync(Guid userId)
        {
            var expenses = await _expenseRepository.GetByUserAsync(userId);
            return expenses.Sum(e => e.Amount);
        }
    }
}