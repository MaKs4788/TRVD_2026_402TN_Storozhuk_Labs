using LabsTRVD.Entities;

namespace LabsTRVD.Repositories.Interfaces
{
    public interface IExpenseRepository: IRepository<Expense>
    {
        Task<IEnumerable<Expense>> GetByUserAsync(Guid userId);
    }
}
