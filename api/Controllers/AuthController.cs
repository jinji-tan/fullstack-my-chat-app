using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using api.DTOs;
using api.Repositories.interfaces;
using api.Service.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _token;
        private readonly IAuthHelper _auth;
        public AuthController(ITokenService token, IAuthHelper auth)
        {
            _token = token;
            _auth = auth;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (await _auth.UserExists(registerDto.Email))
                return BadRequest("Email already exists.");

            bool response = await _auth.Register(registerDto);
            if (!response) return BadRequest("Failed to register.");

            var user = await _auth.GetUserByEmail(registerDto.Email);
            if (user == null) return BadRequest("User was created but could not be retrieved.");

            return Ok(new
            {
                userId = user.Id,
                message = "Successfully registered.",
                token = _token.CreateToken(user.Id, user.Email)
            });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthDto authDto)
        {
            var user = await _auth.GetUserByEmail(authDto.Email);

            if (user == null || !_auth.VerifyPassword(authDto.Password, user.PasswordHash, user.PasswordSalt))
                return Unauthorized("Invalid email or password.");

            return Ok(new
            {
                userId = user.Id,
                token = _token.CreateToken(user.Id, user.Email),
                fullName = user.FirstName + " " + user.LastName
            });
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserUpdateDto user)
        {
            var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var userToUpdate = await _auth.GetUserById(id);

            if (userToUpdate == null) return NotFound("User not found.");

            if (string.IsNullOrEmpty(userIdFromToken) || id.ToString() != userIdFromToken)
                return Unauthorized("You can only update your own profile.");

            bool response = await _auth.UpdateUser(id, user);

            if (!response)
                return BadRequest("Failed to update user.");

            return Ok(user);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var userToDelete = await _auth.GetUserById(id);

            if (userToDelete == null) return NotFound("User not found.");

            if (id.ToString() != userIdFromToken)
                return Unauthorized("You can only delete your own account.");

            bool response = await _auth.DeleteUser(id);

            if (!response)
                return BadRequest("Failed to delete user.");

            return Ok(new { message = $"Successfully deleted user {id}" });
        }
    }

}