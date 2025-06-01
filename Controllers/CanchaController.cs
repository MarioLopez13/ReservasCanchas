using BackendReservas.Data;
using BackendReservas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendReservas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CanchaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CanchaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/cancha
        [HttpGet]
        public async Task<IActionResult> GetCanchas()
        {
            var canchas = await _context.Canchas.ToListAsync();
            return Ok(canchas);
        }

        // GET: api/cancha/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCancha(int id)
        {
            var cancha = await _context.Canchas.FindAsync(id);
            if (cancha == null) return NotFound();
            return Ok(cancha);
        }

        // POST: api/cancha
        [HttpPost]
        public async Task<IActionResult> CreateCancha([FromBody] Cancha cancha)
        {
            _context.Canchas.Add(cancha);
            await _context.SaveChangesAsync();
            return Ok(cancha);
        }

        // PUT: api/cancha/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCancha(int id, [FromBody] Cancha cancha)
        {
            var existing = await _context.Canchas.FindAsync(id);
            if (existing == null) return NotFound();

            existing.Nombre = cancha.Nombre;
            existing.Tipo = cancha.Tipo;
            existing.Ubicacion = cancha.Ubicacion;

            await _context.SaveChangesAsync();
            return Ok(existing);
        }

        // DELETE: api/cancha/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCancha(int id)
        {
            var cancha = await _context.Canchas.FindAsync(id);
            if (cancha == null) return NotFound();

            _context.Canchas.Remove(cancha);
            await _context.SaveChangesAsync();
            return Ok("Cancha eliminada");
        }
    }
}
