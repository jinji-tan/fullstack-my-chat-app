using api.Data;
using api.DTOs;
using api.Models;
using api.Repositories.interfaces;
using Dapper;

namespace api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MyChatAppDapperContext _dapper;
        public UserRepository(MyChatAppDapperContext context)
        {
            _dapper = context;
        }

        public async Task<IEnumerable<UserDto>> GetUsers()
        {
            string sql = @"SELECT Id,
                            Email,
                            FirstName,
                            LastName
                            FROM MyChatAppSchema.Users
                            ORDER BY FirstName ASC";

            var users = await _dapper.LoadData<UserDto>(sql);

            return users.ToList();
        }

        public async Task<IEnumerable<UserDto?>> SearchUsers(string searchTerm)
        {
            string sql = @"SELECT Id, Email, FirstName, LastName
                    FROM MyChatAppSchema.Users
                    WHERE Email LIKE @Search 
                       OR FirstName LIKE @Search 
                       OR LastName LIKE @Search";

            var parameters = new { Search = $"%{searchTerm}%" };

            var users = await _dapper.LoadData<UserDto>(sql, parameters);
            return users.ToList();
        }

    }
}