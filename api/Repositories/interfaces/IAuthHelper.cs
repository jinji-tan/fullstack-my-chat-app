using api.DTOs;
using api.Models;

namespace api.Repositories.interfaces
{
    public interface IAuthHelper
    {
        Task<bool> Register(UserDto userDto);
        Task<bool> UserExists(string email);
        Task<User?> GetUserByEmail(string email);
    }
}