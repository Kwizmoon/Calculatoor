using CalculatriceLibrary.Data;
using CalculatriceLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace CalculatorTP2.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly AppDbContext _db;

        public AuthController(AppDbContext db)
        {
            _db = db;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            if (_db.Users.Any(u => u.Username == user.Username))
            {
                return BadRequest("Username already exists.");
            }
            _db.Users.Add(user);
            _db.SaveChanges();
            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            var existingUser = _db.Users.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);
            if (existingUser == null)
            {
                return Unauthorized("Invalid username or password.");
            }
            return Ok(new { userId = user.Id, username = user.Username });
        }
}
