using core.product.Application.DTOs;
using core.product.Application.Interfaces;
using core.product.Domain.Entities;
using core.product.Domain.Interfaces;

namespace core.product.Application.Services
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
            var products = await _productRepository.GetAllAsync();
            return products.Select(MapToDTO);
        }

        public async Task<ProductResponseDTO?> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return null;
            return MapToDTO(product);
        }

        public async Task<ProductResponseDTO> CreateAsync(ProductCreateDTO productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Stock = productDto.Stock,
                CreatedAt = DateTime.UtcNow
            };

            var createdProduct = await _productRepository.CreateAsync(product);
            return MapToDTO(createdProduct);
        }

        public async Task<bool> UpdateAsync(ProductUpdateDTO updateProductDto)
        {
            var existingProduct = await _productRepository.GetByIdAsync(updateProductDto.Id);
            if (existingProduct == null)
                return false;

            existingProduct.Name = updateProductDto.Name;
            existingProduct.Description = updateProductDto.Description;
            existingProduct.Price = updateProductDto.Price;
            existingProduct.Stock = updateProductDto.Stock;
            existingProduct.UpdatedAt = DateTime.UtcNow;

            return await _productRepository.UpdateAsync(existingProduct);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
                return false;

            return await _productRepository.DeleteAsync(id);
        }

        private static ProductResponseDTO MapToDTO(Product product)
        {
            return new ProductResponseDTO
                (
                    product.Id,
                    product.Name,
                    product.Description,
                    product.Price,
                    product.Stock,
                    product.CreatedAt,
                    product.UpdatedAt
                 );
        }
    }
}
