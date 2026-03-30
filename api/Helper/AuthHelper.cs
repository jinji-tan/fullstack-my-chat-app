using System.Security.Cryptography;
using System.Text;
using api.Data;
using api.DTOs;
using api.Models;
using api.Repositories.interfaces;


namespace api.Helper
{
    public class AuthHelper : IAuthHelper
    {
        private readonly MyChatAppDapperContext _dapper;
        public AuthHelper(MyChatAppDapperContext dapper)
        {
            _dapper = dapper;
        }
        public async Task<bool> Register(UserDto userDto)
        {
            using var hmac = new HMACSHA512();

            byte[] passwordSalt = hmac.Key;
            byte[] passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDto.Password));

            string sql = @"INSERT INTO MyChatAppSchema.Users (Email, FirstName, LastName, PasswordHash, PasswordSalt)
                        VALUES (@Email, @FirstName, @LastName, @PasswordHash, @PasswordSalt)";

            return await _dapper.ExecuteSql(sql, new
            {
                Email = userDto.Email,
                PasswordSalt = passwordSalt,
                PasswordHash = passwordHash,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
            });
        }

        public async Task<bool> UserExists(string email)
        {
            string sql = @"SELECT Email FROM MyChatAppSchema.Users WHERE Email = @Email";

            var user = await _dapper.LoadDataSingle<User>(sql, new { Email = email });

            return user != null;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            string sql = @"SELECT * FROM MyChatAppSchema.Users WHERE Email = @Email";

            return await _dapper.LoadDataSingle<User>(sql, new { Email = email });
        }
    }
}