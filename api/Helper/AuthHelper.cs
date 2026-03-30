using System.Security.Cryptography;
using System.Text;
using api.Data;
using api.DTOs;
using api.Models;
using api.Repositories.interfaces;
using Dapper;


namespace api.Helper
{
    public class AuthHelper : IAuthHelper
    {
        private readonly MyChatAppDapperContext _dapper;
        public AuthHelper(MyChatAppDapperContext dapper)
        {
            _dapper = dapper;
        }
        public async Task<bool> Register(RegisterDto registerDto)
        {
            using var hmac = new HMACSHA512();

            byte[] passwordSalt = hmac.Key;
            byte[] passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));

            string sql = @"INSERT INTO MyChatAppSchema.Users (Email, FirstName, LastName, PasswordHash, PasswordSalt)
                        VALUES (@Email, @FirstName, @LastName, @PasswordHash, @PasswordSalt)";

            return await _dapper.ExecuteSql(sql, new
            {
                Email = registerDto.Email,
                PasswordSalt = passwordSalt,
                PasswordHash = passwordHash,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
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
        
        public bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(storedHash);
        }

        public async Task<User?> GetUserById(int id)
        {
            string sql = @"SELECT * FROM MyChatAppSchema.Users WHERE Id = @Id";

            return await _dapper.LoadDataSingle<User>(sql, new { Id = id });
        }
        public async Task<bool> UpdateUser(int id, UserUpdateDto user)
        {
            var existingUser = await GetUserById(id);

            if (existingUser == null) return false;

            byte[] passwordHash = existingUser.PasswordHash;
            byte[] passwordSalt = existingUser.PasswordSalt;

            if (!string.IsNullOrWhiteSpace(user.Password))
            {
                using var hmac = new HMACSHA512();
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
            }

            string sql = @"UPDATE MyChatAppSchema.Users 
                    SET Email = @Email,
                        FirstName = @FirstName,
                        LastName = @LastName,
                        PasswordHash = @PasswordHash,
                        PasswordSalt = @PasswordSalt
                    WHERE Id = @Id";

            return await _dapper.ExecuteSql(sql, new
            {
                Id = id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PasswordSalt = passwordSalt,
                PasswordHash = passwordHash
            });
        }

        public async Task<bool> DeleteUser(int id)
        {
            string sql = @"DELETE FROM MyChatAppSchema.Users
                            WHERE Id = @Id";

            return await _dapper.ExecuteSql(sql, new { Id = id });
        }
    }
}