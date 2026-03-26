using LabsTRVD.Entities;
using LabsTRVD.Repositories.Interfaces;
using LabsTRVD.Services.Interfaces;

namespace LabsTRVD.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;

        public ExpenseService(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public async Task<IEnumerable<Expense>> GetUserExpensesAsync(Guid userId)
        {
            return await _expenseRepository.GetByUserAsync(userId);
        }

        public async Task<IEnumerable<Expense>> GetByPeriodAsync(Guid userId, DateTime from, DateTime to)
        {
            return await _expenseRepository.FindAsync(e =>
                e.UserId == userId &&
                e.Date >= from &&
                e.Date <= to);
        }

        public async Task<Expense?> GetByIdAsync(int id)
        {
            return await _expenseRepository.GetByIdAsync(id);
        }

        public async Task AddExpenseAsync(Expense expense)
        {
            if (expense.Amount <= 0)
                throw new Exception("Сума повинна бути більше 0");

            if (expense.UserId == Guid.Empty)
                throw new Exception("UserId не вказаний");

            if (expense.Date == default)
                expense.Date = DateTime.UtcNow;

            await _expenseRepository.AddAsync(expense);
        }

        public async Task UpdateExpenseAsync(Expense expense)
        {
            if (expense.Amount <= 0)
                throw new Exception("Сума повинна бути більше 0");

            var existing = await _expenseRepository.GetByIdAsync(expense.ExpenseId);
            if (existing == null)
                throw new Exception("Витрата не знайдена");

            existing.Amount = expense.Amount;
            existing.CategoryId = expense.CategoryId;
            existing.Date = expense.Date;
            existing.Description = expense.Description;

            await _expenseRepository.UpdateAsync(existing);
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
            var expenses = await GetByPeriodAsync(userId, from, to);
            return expenses.Sum(e => e.Amount);
        }
        public async Task<decimal> GetTotalCurrentMonthAsync(Guid userId)
        {
            var now = DateTime.UtcNow;
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