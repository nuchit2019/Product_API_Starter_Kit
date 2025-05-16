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
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string 'DefaultConnection' not found.");
        }


        public async Task<IEnumerable<Product>> GetAllAsync()
        { 
            //throw new SqlException("Simulated database error Repository Layer...");

            using IDbConnection db = new SqlConnection(_connectionString);
            return await db.QueryAsync<Product>("SELECT * FROM Products");
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            return await db.QueryFirstOrDefaultAsync<Product>(
                "SELECT * FROM Products WHERE Id = @Id", new { Id = id });
        }
         
        public async Task<Product> CreateAsync(Product product)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            var sql = @"INSERT INTO Products (Name, Description, Price, Stock) 
                    VALUES (@Name, @Description, @Price, @Stock);
                    SELECT CAST(SCOPE_IDENTITY() as int)";
             
            var newId = await db.QuerySingleAsync<int>(sql, product);
            product.Id = newId; 
            return product;
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            var sql = @"UPDATE Products SET 
                    Name = @Name, 
                    Description = @Description, 
                    Price = @Price, 
                    Stock = @Stock 
                    WHERE Id = @Id";
            var affected = await db.ExecuteAsync(sql, product);
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            var affected = await db.ExecuteAsync(
                "DELETE FROM Products WHERE Id = @Id", new { Id = id });
            return affected > 0;
        }
    }
}
