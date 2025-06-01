using BackendReservas.Data;
using BackendReservas.Models;
using BackendReservas.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendReservas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Usuario user)
        {
            if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
                return BadRequest("Email y contraseña son obligatorios.");

            if (await _context.Usuarios.AnyAsync(u => u.Email == user.Email))
                return BadRequest("Este correo ya está registrado.");

            user.Password = PasswordHasher.HashPassword(user.Password);
            _context.Usuarios.Add(user);
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Usuario registrado" });

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Email y contraseña son obligatorios.");

            var hashed = PasswordHasher.HashPassword(request.Password);

            var existingUser = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == request.Email && u.Password == hashed);

            if (existingUser == null)
                return Unauthorized("Credenciales inválidas");

            return Ok(new
            {
                mensaje = "Inicio de sesión exitoso",
                usuario = new {
                    existingUser.Id,
                    existingUser.Nombre,
                    existingUser.Email,
                    existingUser.Rol
                }
            });
        }
    }
}
