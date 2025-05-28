using core.api.Common;
using core.category.Application.DTOs;
using core.category.Application.Interfaces;
using core.category.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace core.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(ICategoryService categoryService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponseDTO>>> GetAll()
        {
            var categories = await categoryService.GetAllCategoriesAsync();             


            if (categories == null || !categories.Any())
            {
                return NotFound(ApiResponse<IEnumerable<CategoryResponseDTO>>.FailResponse("No Category found.", 404));
            }
            return Ok(ApiResponse<IEnumerable<CategoryResponseDTO>>.SuccessResponse(categories, "Category retrieved successfully."));

        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryResponseDTO>> GetById(int id)
        {
            var category = await categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound(ApiResponse<CategoryResponseDTO>.FailResponse("Category not found.", 404));
            return Ok(ApiResponse<CategoryResponseDTO>.SuccessResponse(category, "Category retrieved successfully."));
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CategoryCreateDTO category)
        {
            var id = await categoryService.AddCategoryAsync(category);
            return CreatedAtAction(nameof(GetById), new { id }, ApiResponse<int>.SuccessResponse(id, "Category created successfully."));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Category category)
        {
            if (id != category.CategoryId)
                return BadRequest(ApiResponse<string>.FailResponse("Category ID mismatch.", 400));

            var updated = await categoryService.UpdateCategoryAsync(category);
            if (!updated)
                return NotFound(ApiResponse<string>.FailResponse("Category not found.", 404));

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await categoryService.DeleteCategoryAsync(id);
            if (!deleted)
                return NotFound(ApiResponse<string>.FailResponse("Category not found.", 404));

            return NoContent();
        }
    }
}
