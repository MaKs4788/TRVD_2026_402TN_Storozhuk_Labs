using LabsTRVD.DTOs.ServicesDTOs;

namespace LabsTRVD.Interfaces.Services
{
    public interface IIncomeService
    {
        Task<IEnumerable<IncomeDtoResponse>> GetUserIncomesAsync(Guid userId);
        Task<IEnumerable<IncomeDtoResponse>> GetByPeriodAsync(Guid userId, DateTime from, DateTime to);
        Task<IncomeDtoResponse?> GetByIdAsync(int id);
        Task<IncomeDtoResponse> AddIncomeAsync(IncomeDto incomeDtoDto);
        Task<IncomeDtoResponse> UpdateIncomeAsync(int id, IncomeDto incomeDto);
        Task DeleteIncomeAsync(int id);
        Task<decimal> GetTotalForPeriodAsync(Guid userId, DateTime from, DateTime to);
        Task<decimal> GetTotalCurrentMonthAsync(Guid userId);
        Task<decimal> GetTotalAsync(Guid userId);
    }
}