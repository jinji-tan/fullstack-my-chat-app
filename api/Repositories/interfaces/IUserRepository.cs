using api.DTOs;

namespace api.Repositories.interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserDto>> GetUsers();
        Task<IEnumerable<UserDto?>> SearchUsers(string searchTerm);
    }
}