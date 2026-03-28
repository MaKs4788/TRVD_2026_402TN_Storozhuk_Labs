using AutoMapper;
using LabsTRVD.DTOs.ServicesDTOs;
using LabsTRVD.Entities;
using LabsTRVD.Interfaces.Services;

namespace LabsTRVD.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Expense> _expenseRepository;
        private readonly IRepository<Income> _incomeRepository;
        private readonly IMapper _mapper;

        public CategoryService(
            IRepository<Category> categoryRepository,
            IRepository<Expense> expenseRepository,
            IRepository<Income> incomeRepository,
            IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _expenseRepository = expenseRepository;
            _incomeRepository = incomeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDtoResponse>> GetUserCategoriesAsync(Guid userId)
        {
            var categories = await _categoryRepository.FindAsync(c => c.UserId == userId);
            return _mapper.Map<IEnumerable<CategoryDtoResponse>>(categories);
        }

        public async Task<CategoryDtoResponse?> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return category == null ? null : _mapper.Map<CategoryDtoResponse>(category);
        }

        public async Task<CategoryDtoResponse> AddCategoryAsync(CategoryDto categoryDto)
        {
            if (string.IsNullOrWhiteSpace(categoryDto.Name))
                throw new Exception("Назва категорії не може бути порожньою");

            var category = _mapper.Map<Category>(categoryDto);
            await _categoryRepository.AddAsync(category);
            return _mapper.Map<CategoryDtoResponse>(category);
        }

        public async Task<CategoryDtoResponse> UpdateCategoryAsync(int id, CategoryDto categoryDto)
        {
            if (string.IsNullOrWhiteSpace(categoryDto.Name))
                throw new Exception("Назва категорії не може бути порожньою");

            var existing = await _categoryRepository.GetByIdAsync(id);
            if (existing == null)
                throw new Exception("Категорія не знайдена");

            existing.Name = categoryDto.Name;
            await _categoryRepository.UpdateAsync(existing);
            return _mapper.Map<CategoryDtoResponse>(existing);
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