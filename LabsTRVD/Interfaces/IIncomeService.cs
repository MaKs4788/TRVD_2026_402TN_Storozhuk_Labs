using LabsTRVD.Entities;

namespace LabsTRVD.Services.Interfaces
{
    public interface IIncomeService
    {
        Task<IEnumerable<Income>> GetUserIncomesAsync(Guid userId);
        Task<IEnumerable<Income>> GetByPeriodAsync(Guid userId, DateTime from, DateTime to);
        Task<Income?> GetByIdAsync(int id);
        Task AddIncomeAsync(Income income);
        Task UpdateIncomeAsync(Income income);
        Task DeleteIncomeAsync(int id);
        Task<decimal> GetTotalForPeriodAsync(Guid userId, DateTime from, DateTime to);
        Task<decimal> GetTotalCurrentMonthAsync(Guid userId);
        Task<decimal> GetTotalAsync(Guid userId);
    }
}