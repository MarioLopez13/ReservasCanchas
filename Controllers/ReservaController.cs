using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendReservas.Data;
using BackendReservas.Models;

namespace BackendReservas.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class ReservaController : ControllerBase {
        private readonly ApplicationDbContext _context;

        public ReservaController(ApplicationDbContext context) {
            _context = context;
        }

        // POST api/Reserva
        [HttpPost]
        public async Task<IActionResult> CrearReserva([FromBody] Reserva reserva) {
            // Validar duplicidad
            var existe = await _context.Reservas.AnyAsync(r =>
                r.UsuarioId == reserva.UsuarioId &&
                r.HorarioId == reserva.HorarioId &&
                r.Fecha.Date == reserva.Fecha.Date);

            if (existe) return BadRequest("Ya existe una reserva para ese horario.");

            // Validar máximo 2 reservas al día
            var cantidad = await _context.Reservas.CountAsync(r =>
                r.UsuarioId == reserva.UsuarioId &&
                r.Fecha.Date == reserva.Fecha.Date);

            if (cantidad >= 2) return BadRequest("Máximo 2 reservas por día.");

            reserva.Estado = "confirmada";
            reserva.PromocionAplicada = false;

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            return Ok(reserva);
        }

        // GET api/Reserva/usuario/5
        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> Historial(int usuarioId) {
            var historial = await _context.Reservas
                .Include(r => r.Horario)
                .Include(r => r.Horario.Cancha)
                .Where(r => r.UsuarioId == usuarioId)
                .ToListAsync();

            return Ok(historial);
        }

        // PUT api/Reserva/cancelar/5
        [HttpPut("cancelar/{id}")]
        public async Task<IActionResult> Cancelar(int id) {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null) return NotFound("No se encontró la reserva.");

            reserva.Estado = "cancelada";
            await _context.SaveChangesAsync();

            return Ok(reserva);
        }
    }
}
