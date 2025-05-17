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
            using var connection = new SqlConnection(_connectionString);
            const string sql = @"SELECT Id, Name, Description, Price, Stock, CreatedAt, UpdatedAt FROM Products";
            var products = await connection.QueryAsync<Product>(sql);
            return products;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = @"SELECT Id, Name, Description, Price, Stock, CreatedAt, UpdatedAt FROM Products WHERE Id = @Id";
            var product = await connection.QuerySingleOrDefaultAsync<Product>(sql, new { Id = id });
            return product;
        }
         
        public async Task<Product> CreateAsync(Product product)
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = @"
                INSERT INTO Products (Name, Description, Price, Stock)
                OUTPUT INSERTED.Id, INSERTED.Name, INSERTED.Description, INSERTED.Price, INSERTED.Stock, INSERTED.CreatedAt
                VALUES (@Name, @Description, @Price, @Stock );
            ";

            var now = DateTime.UtcNow;
            var createdProduct = await connection.QuerySingleAsync<Product>(sql, new
            {
                 product.Name,
                 product.Description,
                 product.Price,
                 product.Stock
                 
            });

            return createdProduct;
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = @"
                UPDATE Products
                SET Name = @Name,
                    Description = @Description,
                    Price = @Price,
                    Stock = @Stock,
                    UpdatedAt = @UpdatedAt
                WHERE Id = @Id;
            ";

            var updatedAt = product.UpdatedAt ?? DateTime.UtcNow;
            var affectedRows = await connection.ExecuteAsync(sql, new
            {
                 product.Id,
                 product.Name,
                 product.Description,
                 product.Price,
                 product.Stock,
                 updatedAt
            });

            return affectedRows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = @"DELETE FROM Products WHERE Id = @Id";
            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }
       
    }
}
