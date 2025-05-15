using ProductAPI.Application.DTOs;
using ProductAPI.Application.Interfaces;
using ProductAPI.Domain.Entities;
using ProductAPI.Domain.Interfaces;

namespace ProductAPI.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductResponseDTO>> GetAllAsync()
        {
            // TODO : Implement ...
            return null;
        }

        public async Task<ProductResponseDTO?> GetByIdAsync(int id)
        {
            // TODO : Implement ...
            return null;
        }

        public async Task<ProductResponseDTO> CreateAsync(ProductCreateDTO productDto)
        {
            // TODO : Implement ...
            return null;
        }

        public async Task<bool> UpdateAsync(ProductUpdateDTO productDto)
        {
            // TODO : Implement ...
            return false;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            // TODO : Implement ...
            return false ;
        }

        private static ProductResponseDTO MapToDTO(Product product)
        {
            // TODO : Implement ...
            return null;
        }
    }
}