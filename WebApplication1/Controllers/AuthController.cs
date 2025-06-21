using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Model;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenService _jwtTokenService;

        private readonly List<User> _users = new()
        {
            new User { Username = "admin", Password = "admin123", Role = "Admin" },
            new User { Username = "user", Password = "user123", Role = "User" }
        };
        public AuthController(IJwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _users.SingleOrDefault(u =>
                u.Username == request.Username && u.Password == request.Password);

            if (user == null)
                return Unauthorized("Invalid credentials");

            var token = _jwtTokenService.GenerateToken(user);
            return Ok(new { token });
        }
    }
    }
