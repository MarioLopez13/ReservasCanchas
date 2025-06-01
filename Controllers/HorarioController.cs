using BackendReservas.Data;
using BackendReservas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendReservas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HorarioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HorarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/horario
        [HttpGet]
        public async Task<IActionResult> GetHorarios()
        {
            var horarios = await _context.Horarios.Include(h => h.Cancha).ToListAsync();
            return Ok(horarios);
        }

        // GET: api/horario/cancha/1
        [HttpGet("cancha/{canchaId}")]
        public async Task<IActionResult> GetHorariosPorCancha(int canchaId)
        {
            var horarios = await _context.Horarios
                .Where(h => h.CanchaId == canchaId)
                .ToListAsync();

            return Ok(horarios);
        }

        // POST: api/horario
        [HttpPost]
        public async Task<IActionResult> CrearHorario([FromBody] Horario horario)
        {
            _context.Horarios.Add(horario);
            await _context.SaveChangesAsync();
            return Ok(horario);
        }

        // PUT: api/horario/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarHorario(int id, [FromBody] Horario horario)
        {
            var existente = await _context.Horarios.FindAsync(id);
            if (existente == null) return NotFound();

            existente.HoraInicio = horario.HoraInicio;
            existente.HoraFin = horario.HoraFin;
            existente.Disponible = horario.Disponible;

            await _context.SaveChangesAsync();
            return Ok(existente);
        }

        // DELETE: api/horario/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarHorario(int id)
        {
            var horario = await _context.Horarios.FindAsync(id);
            if (horario == null) return NotFound();

            _context.Horarios.Remove(horario);
            await _context.SaveChangesAsync();
            return Ok("Horario eliminado");
        }
    }
}
