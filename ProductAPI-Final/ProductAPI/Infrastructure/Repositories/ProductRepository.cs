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
             //throw new Exception("Test Exception ** ProductRepository. GetAllAsync() **"); 
            
            using IDbConnection db = new SqlConnection(_connectionString);
            return await db.QueryAsync<Product>("SELECT * FROM Product2");
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            return await db.QueryFirstOrDefaultAsync<Product>(
                "SELECT * FROM Product2 WHERE Id = @Id", new { Id = id });
        }

        public async Task<int> CreateAsync(Product product)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            var sql = @"INSERT INTO Product2 (Name, Description, Price, Stock) 
                    VALUES (@Name, @Description, @Price, @Stock);
                    SELECT CAST(SCOPE_IDENTITY() as int)";
            return await db.QuerySingleAsync<int>(sql, product);
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            var sql = @"UPDATE Product2 SET 
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
                "DELETE FROM Product2 WHERE Id = @Id", new { Id = id });
            return affected > 0;
        }
    }
}
