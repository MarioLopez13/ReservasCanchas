using BackendReservas.Data;
using BackendReservas.Models;
using BackendReservas.Services;
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

        [HttpGet("bloques/cancha/{canchaId}")]
        public IActionResult GetBloquesHorarioConDescuentos(int canchaId)
        {
            var bloques = PromocionService.Instancia(_context).CalcularBloquesHorario(canchaId);
            return Ok(bloques);
        }

        [HttpPost("generar/{canchaId}")]
        public async Task<IActionResult> GenerarHorariosParaCancha(int canchaId)
        {
            var yaExisten = await _context.Horarios.AnyAsync(h => h.CanchaId == canchaId);
            if (yaExisten)
                return BadRequest("Los horarios ya existen para esta cancha.");

            var horarios = new List<Horario>();
            var horaInicio = new TimeSpan(7, 0, 0);
            var horaFin = new TimeSpan(23, 0, 0);

            for (var hora = horaInicio; hora < horaFin; hora = hora.Add(TimeSpan.FromHours(1)))
            {
                horarios.Add(new Horario
                {
                    CanchaId = canchaId,
                    HoraInicio = hora,
                    HoraFin = hora.Add(TimeSpan.FromHours(1)),
                    Disponible = true
                });
            }

            _context.Horarios.AddRange(horarios);
            await _context.SaveChangesAsync();

            return Ok($"Horarios generados para la cancha con ID: {canchaId}");
        }
    }
}