using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendReservas.Data;
using BackendReservas.Models;

namespace BackendReservas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReservaController(ApplicationDbContext context)
        {
            _context = context;
        }

         // ==========================
        // CREAR UNA NUEVA RESERVA
        // POST api/Reserva
        // ==========================
        [HttpPost]
        public async Task<IActionResult> CrearReserva([FromBody] Reserva reserva)
        {
            // Cargar horario
            var horario = await _context.Horarios.FindAsync(reserva.HorarioId);
            if (horario == null)
                return BadRequest("Horario no encontrado.");

            // Validar si el horario ya fue reservado
            var yaReservado = await _context.Reservas.AnyAsync(r =>
                r.HorarioId == reserva.HorarioId &&
                r.Estado == "confirmada"
            );

            if (yaReservado)
                return BadRequest("Este horario ya ha sido reservado por otro usuario.");

            // Validar si el usuario ya tiene reserva para ese horario
            var existe = await _context.Reservas.AnyAsync(r =>
                r.UsuarioId == reserva.UsuarioId &&
                r.HorarioId == reserva.HorarioId &&
                r.Fecha.Date == reserva.Fecha.Date
            );

            if (existe)
                return BadRequest("Ya existe una reserva para ese horario.");

            //  Obtener todos los horarios de esa cancha
            var horariosCancha = await _context.Horarios
                .Where(h => h.CanchaId == horario.CanchaId)
                .Select(h => h.Id)
                .ToListAsync();

            // ✅ Validar máximo 2 reservas por cancha y por día
            var cantidad = await _context.Reservas.CountAsync(r =>
                r.UsuarioId == reserva.UsuarioId &&
                horariosCancha.Contains(r.HorarioId) &&
                r.Fecha.Date == reserva.Fecha.Date &&
                r.Estado == "confirmada"
            );

            if (cantidad >= 2)
                return BadRequest("Máximo 2 reservas por día para esta cancha.");

            // Actualizar estado del horario a no disponible
            horario.Disponible = false;

            reserva.Estado = "confirmada";
            reserva.PromocionAplicada = reserva.PromocionAplicada;

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            return Ok(reserva);
        }

        // GET api/Reserva/usuario/5
        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> Historial(int usuarioId)
        {
            var historial = await _context.Reservas
                .Include(r => r.Horario)
                .ThenInclude(h => h.Cancha)
                .Where(r => r.UsuarioId == usuarioId)
                .ToListAsync();

            return Ok(historial);
        }

        // PUT api/Reserva/cancelar/5
        [HttpPut("cancelar/{id}")]
        public async Task<IActionResult> Cancelar(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
                return NotFound("No se encontró la reserva.");

            reserva.Estado = "cancelada";

            var horario = await _context.Horarios.FindAsync(reserva.HorarioId);
            if (horario != null)
                horario.Disponible = true;

            await _context.SaveChangesAsync();
            return Ok(reserva);
        }
    }
}
