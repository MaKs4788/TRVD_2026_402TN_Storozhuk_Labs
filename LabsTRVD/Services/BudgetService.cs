using LabsTRVD.Entities;
using LabsTRVD.Repositories.Interfaces;
using LabsTRVD.Services.Interfaces;

namespace LabsTRVD.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly IRepository<Budget> _budgetRepository;
        private readonly IRepository<Expense> _expenseRepository;

        public BudgetService(
            IRepository<Budget> budgetRepository,
            IRepository<Expense> expenseRepository)
        {
            _budgetRepository = budgetRepository;
            _expenseRepository = expenseRepository;
        }

        public async Task SetBudgetAsync(Guid userId, int month, int year, decimal limit)
        {
            if (limit <= 0)
                throw new Exception("Ліміт повинен бути більше 0");

            var existing = await _budgetRepository.FindAsync(b =>
                b.UserId == userId &&
                b.Month == month &&
                b.Year == year);

            var budget = existing.FirstOrDefault();

            if (budget == null)
            {
                await _budgetRepository.AddAsync(new Budget
                {
                    UserId = userId,
                    Month = month,
                    Year = year,
                    MonthlyLimit = limit
                });
            }
            else
            {
                budget.MonthlyLimit = limit;
                await _budgetRepository.UpdateAsync(budget);
            }
        }

        public async Task<decimal> GetMonthlyLimitAsync(Guid userId, int month, int year)
        {
            var budget = (await _budgetRepository.FindAsync(b =>
                b.UserId == userId &&
                b.Month == month &&
                b.Year == year)).FirstOrDefault();

            return budget?.MonthlyLimit ?? 0;
        }

        public async Task<decimal> GetUsedAmountAsync(Guid userId, int month, int year)
        {
            var from = new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Utc);
            var to = from.AddMonths(1);

            var expenses = await _expenseRepository.FindAsync(e =>
                e.UserId == userId &&
                e.Date >= from &&
                e.Date < to);

            return expenses.Sum(e => e.Amount);
        }

        public async Task<decimal> GetRemainingBudgetAsync(Guid userId, int month, int year)
        {
            var limit = await GetMonthlyLimitAsync(userId, month, year);
            var used = await GetUsedAmountAsync(userId, month, year);
            return limit - used;
        }

        public async Task<double> GetUsagePercentageAsync(Guid userId, int month, int year)
        {
            var limit = await GetMonthlyLimitAsync(userId, month, year);
            if (limit == 0) return 0;

            var used = await GetUsedAmountAsync(userId, month, year);
            return (double)(used / limit * 100);
        }

        public async Task<bool> IsBudgetExceeded(Guid userId, int month, int year)
        {
            var remaining = await GetRemainingBudgetAsync(userId, month, year);
            return remaining < 0;
        }
    }
}