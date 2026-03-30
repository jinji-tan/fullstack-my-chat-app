using System.Data;
using System.Data.Common;
using Dapper;
using Microsoft.Data.SqlClient;

namespace api.Data
{
    public class MyChatAppDapperContext
    {
        private readonly IConfiguration _config;
        public MyChatAppDapperContext(IConfiguration config)
        {
            _config = config;
        }

        private IDbConnection CreateConnection() => new SqlConnection(_config.GetConnectionString("DefaultConnection"));

        public async Task<IEnumerable<T>> LoadData<T>(string sql, object? parameter = null)
        {
            using var connection = CreateConnection();

            return await connection.QueryAsync<T>(sql, parameter);
        }

        public async Task<T?> LoadDataSingle<T>(string sql, object? parameter = null)
        {
            using var connection = CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<T>(sql, parameter);
        }

        public async Task<bool> ExecuteSql(string sql, object? parameter = null)
        {
            using var connection = CreateConnection();

            return await connection.ExecuteAsync(sql, parameter) > 0;
        }
    
    }
}