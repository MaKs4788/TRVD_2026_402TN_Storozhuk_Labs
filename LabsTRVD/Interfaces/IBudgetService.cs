using LabsTRVD.Entities;

namespace LabsTRVD.Services.Interfaces
{
    public interface IBudgetService
    {
        Task SetBudgetAsync(Guid userId, int month, int year, decimal limit);
        Task<decimal> GetMonthlyLimitAsync(Guid userId, int month, int year);
        Task<decimal> GetUsedAmountAsync(Guid userId, int month, int year);
        Task<decimal> GetRemainingBudgetAsync(Guid userId, int month, int year);
        Task<double> GetUsagePercentageAsync(Guid userId, int month, int year);
        Task<bool> IsBudgetExceeded(Guid userId, int month, int year);
    }
}