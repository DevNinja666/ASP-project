using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Invoicer.Data;
using Invoicer.Models;
using Invoicer.DTOs.User;

namespace Invoicer.Controllers
{
//Надир мяллим это контроллер пользователя перед использованием
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto dto)
        {
            var exists = await _context.Users.AnyAsync(x => x.Email == dto.Email);

            if (exists)
                return BadRequest("User with this email already exists");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = dto.Password,
                Address = dto.Address,
                PhoneNumber = dto.PhoneNumber,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == dto.Email && x.Password == dto.Password);

            if (user == null)
                return Unauthorized("Invalid email or password");

            return Ok(user);
        }

        
        [HttpPut("profile/{id}")]
        public async Task<IActionResult> UpdateProfile(int id, UpdateProfileDto dto)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            user.Name = dto.Name;
            user.Email = dto.Email;
            user.Address = dto.Address;
            user.PhoneNumber = dto.PhoneNumber;
            user.UpdatedAt = DateTimeOffset.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPatch("password/{id}")]
        public async Task<IActionResult> ChangePassword(int id, ChangePasswordDto dto)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            if (user.Password != dto.CurrentPassword)
                return BadRequest("Current password incorrect");

            user.Password = dto.NewPassword;
            user.UpdatedAt = DateTimeOffset.UtcNow;

            await _context.SaveChangesAsync();

            return Ok("Password updated");
        }
    }
}
