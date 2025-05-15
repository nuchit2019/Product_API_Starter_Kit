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

            // throw new Exception("Test Exception ProductService---------------");
            return products.Select(p => MapToDTO(p));
        }

        public async Task<ProductResponseDTO?> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product != null ? MapToDTO(product) : null;
        }

        public async Task<ProductResponseDTO> CreateAsync(ProductCreateDTO productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Stock = productDto.Stock
            };

            product.Id = await _productRepository.CreateAsync(product);
            return MapToDTO(product);
        }

        public async Task<bool> UpdateAsync(ProductUpdateDTO productDto)
        {
            var product = new Product
            {
                Id = productDto.Id,
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Stock = productDto.Stock
            };

            return await _productRepository.UpdateAsync(product);
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
                product.Stock);
        }
    }
}