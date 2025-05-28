using core.category.Application.DTOs;
using core.category.Domain.Entities;

namespace core.category.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<int> AddCategoryAsync(CategoryCreateDTO category);
        Task<CategoryResponseDTO?> GetCategoryByIdAsync(int id);
        Task<IEnumerable<CategoryResponseDTO>> GetAllCategoriesAsync();
        Task<bool> UpdateCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(int id);
    }
}