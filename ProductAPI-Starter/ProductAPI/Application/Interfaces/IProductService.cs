using ProductAPI.Application.DTOs;

namespace ProductAPI.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponseDTO>> GetAllAsync();
        Task<ProductResponseDTO?> GetByIdAsync(int id);
        Task<ProductResponseDTO> CreateAsync(ProductCreateDTO productDto);
        Task<bool> UpdateAsync(ProductUpdateDTO productDto);
        Task<bool> DeleteAsync(int id);
    }
}
