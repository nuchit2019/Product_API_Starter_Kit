using core.category.Application.DTOs;
using core.category.Domain.Entities;
using core.category.Domain.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace core.category.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly string _connectionString;

        public CategoryRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string 'DefaultConnection' not found.");
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = "SELECT * FROM Category";
            return await connection.QueryAsync<Category>(sql);
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = "SELECT * FROM Category WHERE CategoryId = @Id";
            return await connection.QuerySingleOrDefaultAsync<Category>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(Category category)
        {
            using var connection = new SqlConnection(_connectionString);
            // Check for duplicate Name
            const string checkSql = "SELECT COUNT(1) FROM Category WHERE Name = @Name";
            var exists = await connection.ExecuteScalarAsync<int>(checkSql, new { category.Name });
            if (exists > 0)
            {
                throw new InvalidOperationException($"A category with the name '{category.Name}' already exists.");
            }

            const string sql = @"
                    INSERT INTO Category (Name, Description, IsActive)
                    VALUES (@Name, @Description, @IsActive);
                    SELECT CAST(SCOPE_IDENTITY() as int);";
            return await connection.ExecuteScalarAsync<int>(sql, category);
        }

        public async Task<bool> UpdateAsync(Category category)
        {
            using var connection = new SqlConnection(_connectionString);
            var updates = new List<string>();
            var parameters = new DynamicParameters();
            parameters.Add("@Id", category.CategoryId);

            if (!string.IsNullOrEmpty(category.Name) )
            {
                updates.Add("Name = @Name");
                parameters.Add("@Name", category.Name);
            }
            if (!string.IsNullOrEmpty(category.Description))
            {
                updates.Add("Description = @Description");
                parameters.Add("@Description", category.Description);
            }

            if (category.IsActive != null)
            {

                updates.Add("IsActive = @IsActive");
                parameters.Add("@IsActive", category.IsActive);
            }

            if (updates.Count == 0)
                return false;

            var sql = $@"
                UPDATE Category
                SET {string.Join(", ", updates)}
                WHERE CategoryId = @Id";

            var affected = await connection.ExecuteAsync(sql, parameters);
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = "DELETE FROM Category WHERE CategoryId = @Id";
            var affected = await connection.ExecuteAsync(sql, new { Id = id });
            return affected > 0;
        }
        
    }
}
