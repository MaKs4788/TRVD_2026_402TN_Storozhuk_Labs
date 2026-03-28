using LabsTRVD.DTOs.ServicesDTOs;

namespace LabsTRVD.Interfaces.Services
{
    public interface IExpenseService
    {
        Task<IEnumerable<ExpenseDtoResponse>> GetUserExpensesAsync(Guid userId);

        Task<IEnumerable<ExpenseDtoResponse>> GetByPeriodAsync(Guid userId, DateTime from, DateTime to);
        Task<ExpenseDtoResponse?> GetByIdAsync(int id);
        Task<ExpenseDtoResponse> AddExpenseAsync(ExpenseDto expenseDto);
        Task<ExpenseDtoResponse> UpdateExpenseAsync(int id, ExpenseDto expenseDto);
        Task DeleteExpenseAsync(int id);
        Task<decimal> GetTotalForPeriodAsync(Guid userId, DateTime from, DateTime to);
        Task<decimal> GetTotalCurrentMonthAsync(Guid userId);
        Task<decimal> GetTotalAsync(Guid userId);
    }
}