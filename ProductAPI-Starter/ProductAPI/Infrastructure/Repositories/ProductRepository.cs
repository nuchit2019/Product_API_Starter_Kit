using Dapper;
using Microsoft.Data.SqlClient;
using ProductAPI.Domain.Entities;
using ProductAPI.Domain.Interfaces;
using System.Data;

namespace ProductAPI.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString; 

        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")  ?? throw new ArgumentNullException(nameof(configuration), "Connection string 'DefaultConnection' not found.");
        }


        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            // TODO : Implement ...
            return null;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            // TODO : Implement ...
            return null;
        }

        public async Task<int> CreateAsync(Product product)
        {
            // TODO : Implement ...
            return 0;
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            // TODO : Implement ...
            return false;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            // TODO : Implement ...
            return false;
        }
    }
}
