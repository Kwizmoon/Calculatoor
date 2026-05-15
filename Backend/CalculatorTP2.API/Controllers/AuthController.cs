using CalculatriceLibrary.Data;
using CalculatriceLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace CalculatorTP2.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;

        public AuthController(AppDbContext db)
        {
            _db = db;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            if (_db.Users.Any(u => u.Username == request.Username))
            {
                return BadRequest("Username already exists.");
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // 2. Mapping: Create a clean User object from the Request
            var newUser = new User
            {
                Username = request.Username,
                Password = hashedPassword //  hash this!
            };
            _db.Users.Add(newUser);
            _db.SaveChanges();
            return Ok(new { message = "User registered successfully." });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // 1. Find the user by Username only
            var user = _db.Users.FirstOrDefault(u => u.Username == request.Username);

            // 2. Check if user exists AND if the password matches the hash
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            return Ok(new { userId = user.Id, username = user.Username });
        }
    }
}