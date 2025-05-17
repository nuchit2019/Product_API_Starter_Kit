using Dapper;
using Microsoft.Data.SqlClient;
using core.product.Domain.Entities;
using core.product.Domain.Interfaces;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace core.product.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString; 

        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string 'DefaultConnection' not found.");
        }


        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            // TODO 01 [Repository.GetAllAsync]: Implement ...
            return null;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            // TODO 02 [Repository.GetByIdAsync]: Implement ...
            return null;
        }
         
        public async Task<Product> CreateAsync(Product product)
        {
            // TODO 03 [Repository.CreateAsync]: Implement ...
            return null;
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            // TODO 04 [Repository.UpdateAsync]: Implement ...
            return false;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            // TODO 05 [Repository.DeleteAsync]: Implement ...
            return false;
        }
       
    }
}
