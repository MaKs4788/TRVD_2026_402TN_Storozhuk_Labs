using LabsTRVD.Entities;
using LabsTRVD.Repositories.Interfaces;
using LabsTRVD.Services.Interfaces;

namespace LabsTRVD.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Expense> _expenseRepository;
        private readonly IRepository<Income> _incomeRepository;

        public CategoryService(
            IRepository<Category> categoryRepository,
            IRepository<Expense> expenseRepository,
            IRepository<Income> incomeRepository)
        {
            _categoryRepository = categoryRepository;
            _expenseRepository = expenseRepository;
            _incomeRepository = incomeRepository;
        }

        public async Task<IEnumerable<Category>> GetUserCategoriesAsync(Guid userId)
        {
            return await _categoryRepository.FindAsync(c => c.UserId == userId);
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task AddCategoryAsync(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.Name))
                throw new Exception("Назва категорії не може бути порожньою");

            await _categoryRepository.AddAsync(category);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.Name))
                throw new Exception("Назва категорії не може бути порожньою");

            var existing = await _categoryRepository.GetByIdAsync(category.CategoryId);
            if (existing == null)
                throw new Exception("Категорія не знайдена");

            existing.Name = category.Name;
            await _categoryRepository.UpdateAsync(existing);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                throw new Exception("Категорія не знайдена");

            bool used = await IsCategoryUsedAsync(id);
            if (used)
                throw new Exception("Категорія використовується у доходах або витратах");

            await _categoryRepository.DeleteAsync(category);
        }

        public async Task<bool> IsCategoryUsedAsync(int categoryId)
        {
            var expenses = await _expenseRepository.FindAsync(e => e.CategoryId == categoryId);
            if (expenses.Any()) return true;

            var incomes = await _incomeRepository.FindAsync(i => i.CategoryId == categoryId);
            return incomes.Any();
        }
    }
}