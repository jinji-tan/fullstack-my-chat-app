using api.DTOs;
using api.Repositories.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _user;
        public UserController(IUserRepository user)
        {
            _user = user;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _user.GetUsers();

            if (users == null)
                return NoContent();

            return Ok(users);
        }

        [HttpGet("{searchTerm}")]
        public async Task<IActionResult> SearchUsers(string searchTerm)
        {
            var user = await _user.SearchUsers(searchTerm);
            if (user == null)
                return NoContent();

            return Ok(user);
        }
    }
    
}