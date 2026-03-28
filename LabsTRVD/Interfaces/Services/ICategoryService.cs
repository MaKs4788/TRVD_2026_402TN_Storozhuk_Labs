using LabsTRVD.DTOs.ServicesDTOs;

namespace LabsTRVD.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDtoResponse>> GetUserCategoriesAsync(Guid userId);
        Task<CategoryDtoResponse?> GetByIdAsync(int id);
        Task<CategoryDtoResponse> AddCategoryAsync(CategoryDto categoryDto);
        Task<CategoryDtoResponse> UpdateCategoryAsync(int id, CategoryDto categoryDto);
        Task DeleteCategoryAsync(int id);
        Task<bool> IsCategoryUsedAsync(int categoryId);
    }
}