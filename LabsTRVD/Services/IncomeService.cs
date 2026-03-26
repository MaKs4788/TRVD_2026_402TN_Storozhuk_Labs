using LabsTRVD.Entities;
using LabsTRVD.Repositories.Interfaces;
using LabsTRVD.Services.Interfaces;

namespace LabsTRVD.Services
{
    public class IncomeService : IIncomeService
    {
        private readonly IRepository<Income> _incomeRepository;

        public IncomeService(IRepository<Income> incomeRepository)
        {
            _incomeRepository = incomeRepository;
        }

        public async Task<IEnumerable<Income>> GetUserIncomesAsync(Guid userId)
        {
            return await _incomeRepository.FindAsync(i => i.UserId == userId);
        }

        public async Task<IEnumerable<Income>> GetByPeriodAsync(Guid userId, DateTime from, DateTime to)
        {
            return await _incomeRepository.FindAsync(i =>
                i.UserId == userId &&
                i.Date >= from &&
                i.Date <= to);
        }

        public async Task<Income?> GetByIdAsync(int id)
        {
            return await _incomeRepository.GetByIdAsync(id);
        }

        public async Task AddIncomeAsync(Income income)
        {
            if (income.Amount <= 0)
                throw new Exception("Сума доходу повинна бути більше 0");

            if (income.UserId == Guid.Empty)
                throw new Exception("UserId не вказаний");

            if (income.Date == default)
                income.Date = DateTime.UtcNow;

            await _incomeRepository.AddAsync(income);
        }

        public async Task UpdateIncomeAsync(Income income)
        {
            if (income.Amount <= 0)
                throw new Exception("Сума доходу повинна бути більше 0");

            var existing = await _incomeRepository.GetByIdAsync(income.IncomeId);
            if (existing == null)
                throw new Exception("Дохід не знайдено");

            existing.Amount = income.Amount;
            existing.Date = income.Date;
            existing.Description = income.Description;
            existing.CategoryId = income.CategoryId;

            await _incomeRepository.UpdateAsync(existing);
        }

        public async Task DeleteIncomeAsync(int id)
        {
            var income = await _incomeRepository.GetByIdAsync(id);
            if (income == null)
                throw new Exception("Дохід не знайдено");

            await _incomeRepository.DeleteAsync(income);
        }

        public async Task<decimal> GetTotalForPeriodAsync(Guid userId, DateTime from, DateTime to)
        {
            var incomes = await GetByPeriodAsync(userId, from, to);
            return incomes.Sum(i => i.Amount);
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
            var incomes = await _incomeRepository.FindAsync(i => i.UserId == userId);
            return incomes.Sum(i => i.Amount);
        }
    }
}