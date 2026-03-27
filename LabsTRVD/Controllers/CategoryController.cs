using AutoMapper;
using LabsTRVD.DTOs;
using LabsTRVD.Entities;
using LabsTRVD.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LabsTRVD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        // GET: api/Category?userId=...
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetUserCategories([FromQuery] Guid userId)
        {
            var categories = await _categoryService.GetUserCategoriesAsync(userId);
            var dtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            return Ok(dtos);
        }

        // GET: api/Category/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null) return NotFound();

            var dto = _mapper.Map<CategoryDto>(category);
            return Ok(dto);
        }

        // POST: api/Category
        [HttpPost]
        public async Task<ActionResult> AddCategory([FromBody] CategoryDto dto)
        {
            try
            {
                var category = _mapper.Map<Category>(dto);
                await _categoryService.AddCategoryAsync(category);
                return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryId }, dto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/Category/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(int id, [FromBody] CategoryDto dto)
        {
            try
            {
                var category = _mapper.Map<Category>(dto);
                category.CategoryId = id;
                await _categoryService.UpdateCategoryAsync(category);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/Category/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}