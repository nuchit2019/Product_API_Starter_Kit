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
            // TODO 01 [Service]: Implement ...
            return null;
        }

        public async Task<ProductResponseDTO?> GetByIdAsync(int id)
        {
            // TODO 02 [Service]: Implement ...
            return null;
        }

        public async Task<ProductResponseDTO> CreateAsync(ProductCreateDTO productDto)
        {
            // TODO 03 [Service]: Implement ...
            return null;

        }

        public async Task<bool> UpdateAsync(ProductUpdateDTO updateProductDto)
        {
            // TODO 04 [Service]: Implement ...
            return false ;

        }

        public async Task<bool> DeleteAsync(int id)
        {
            // TODO 05 [Service]: Implement ...
            return false ;
        }

        private static ProductResponseDTO MapToDTO(Product product)
        {
            // TODO 06 [Service]: Implement ...
            return null;
        }
    }
}
