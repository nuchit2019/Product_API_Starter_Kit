using core.category.Application.DTOs;
using core.category.Domain.Entities;

namespace core.category.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task<int> AddAsync(Category category);

        Task<Category?> GetByIdAsync(int id);

        Task<IEnumerable<Category>> GetAllAsync();

        Task<bool> UpdateAsync(Category category);


        Task<bool> DeleteAsync(int id);
    }
}