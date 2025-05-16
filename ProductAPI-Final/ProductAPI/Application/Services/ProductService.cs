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
            var products = await _productRepository.GetAllAsync();

            //throw new InvalidOperationException("Simulated exception in Service Layer");
            return products.Select(p => MapToDTO(p));
        }

        public async Task<ProductResponseDTO?> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product != null ? MapToDTO(product) : null;
        }

        public async Task<ProductResponseDTO> CreateAsync(ProductCreateDTO productDto)
        { 
            var entity = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Stock = productDto.Stock,
                CreatedAt = DateTime.UtcNow
            };
             
            var newProduct = await _productRepository.CreateAsync(entity);
            return MapToDTO(newProduct);

        }

        public async Task<bool> UpdateAsync(ProductUpdateDTO updateProductDto)
        { 
            var existingProduct = await _productRepository.GetByIdAsync(updateProductDto.Id);
            if (existingProduct is null)
                return false;

            existingProduct.Name = updateProductDto.Name ?? existingProduct.Name;
            existingProduct.Description = updateProductDto.Description ?? existingProduct.Description;
            existingProduct.Price = updateProductDto.Price ?? existingProduct.Price;
            existingProduct.Stock = updateProductDto.Stock ?? existingProduct.Stock;
            existingProduct.UpdatedAt = DateTime.UtcNow;

            return await _productRepository.UpdateAsync(existingProduct);

        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _productRepository.DeleteAsync(id);
        }

        private static ProductResponseDTO MapToDTO(Product product)
        {
            return new ProductResponseDTO(
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
