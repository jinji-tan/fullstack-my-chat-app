using System.Security.Cryptography;
using System.Text;
using api.DTOs;
using api.Repositories.interfaces;
using api.Service.interfaces;
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
        public async Task<IActionResult> Register(UserDto userDto)
        {
            if (await _auth.UserExists(userDto.Email))
                return BadRequest("Email already exists.");

            bool response = await _auth.Register(userDto);

            if (!response)
                return BadRequest("Failed to register.");

            return Ok(new { message = "Successfully registered." });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthDto authDto)
        {
            var user = await _auth.GetUserByEmail(authDto.Email);

            if (user == null)
                return Unauthorized("Invalid email or password.");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(authDto.Password));

            if (!computedHash.SequenceEqual(user.PasswordHash))
                return Unauthorized("Invalid email or password.");

            return Ok(new
            {
                token = _token.CreateToken(user.Email),
                fullName = user.FirstName + " " + user.LastName
            });
        }
    }
}