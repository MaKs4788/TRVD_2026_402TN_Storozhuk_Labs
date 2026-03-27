using LabsTRVD.Entities;

namespace LabsTRVD.Services.Interfaces
{
    public interface IExpenseService
    {
        Task<IEnumerable<Expense>> GetUserExpensesAsync(Guid userId);

        Task<IEnumerable<Expense>> GetByPeriodAsync(Guid userId, DateTime from, DateTime to);
        Task<Expense?> GetByIdAsync(int id);
        Task AddExpenseAsync(Expense expense);
        Task UpdateExpenseAsync(Expense expense);
        Task DeleteExpenseAsync(int id);
        Task<decimal> GetTotalForPeriodAsync(Guid userId, DateTime from, DateTime to);
        Task<decimal> GetTotalCurrentMonthAsync(Guid userId);
        Task<decimal> GetTotalAsync(Guid userId);
    }
}