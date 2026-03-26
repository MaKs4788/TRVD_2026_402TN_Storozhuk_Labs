using LabsTRVD.Entities;

namespace LabsTRVD.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetUserCategoriesAsync(Guid userId);
        Task<Category?> GetByIdAsync(int id);
        Task AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);
        Task<bool> IsCategoryUsedAsync(int categoryId);
    }
}