using api.DTOs;
using api.Models;

namespace api.Repositories.interfaces
{
    public interface IAuthHelper
    {
        Task<bool> Register(RegisterDto registerDto);
        Task<bool> UserExists(string email);
        Task<User?> GetUserById(int id);
        Task<User?> GetUserByEmail(string email);
        Task<bool> UpdateUser(int id, UserUpdateDto user);
        Task<bool> DeleteUser(int id);
        bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt);
    }
}