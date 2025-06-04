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

        [HttpGet("bloques/cancha/{canchaId}")]
        public IActionResult GetBloquesHorarioConDescuentos(int canchaId)
        {
            var horaInicio = new TimeSpan(7, 0, 0);
            var horaFin = new TimeSpan(23, 0, 0);
            var bloques = new List<object>();

// Obtener todos los horarios existentes para esa cancha
            var horarios = _context.Horarios
                .Where(h => h.CanchaId == canchaId)
                .ToList();
// Obtener todas las reservas de hoy para esa cancha
            var reservasHoy = _context.Reservas
                .Include(r => r.Horario)
                .Where(r =>
                    r.Horario != null &&
                    r.Horario.CanchaId == canchaId &&
                    r.Fecha.Date == DateTime.Today &&
                    r.Estado == "confirmada"
                ).ToList();

            int reservasTotalesHoy = reservasHoy.Count;
            var ahora = DateTime.Now.TimeOfDay;

            for (var hora = horaInicio; hora < horaFin; hora = hora.Add(TimeSpan.FromHours(1)))
            {
                var fin = hora.Add(TimeSpan.FromHours(1));
                var horarioDb = horarios.FirstOrDefault(h => h.HoraInicio == hora);

                bool ocupado = reservasHoy.Any(r => r.Horario != null && r.Horario.HoraInicio == hora);

                int descuento = 0;
                List<string> razones = new();

                int totalBloques = (int)(horaFin - horaInicio).TotalHours;
                if (reservasTotalesHoy < totalBloques * 0.2)
                {
                    descuento += 10;
                    razones.Add("Baja ocupación general del día (<20%)");
                }

                if (reservasTotalesHoy < 4)
                {
                    descuento += 10;
                    razones.Add("Menos de 4 reservas en el día");
                }

                if (hora - ahora <= TimeSpan.FromHours(2) && hora > ahora)
                {
                    descuento += 10;
                    razones.Add("Reserva anticipada (menos de 2h)");
                }

                if (hora == new TimeSpan(14, 0, 0))
                {
                    descuento += 15;
                    razones.Add("Horario históricamente con baja demanda");
                }

                bloques.Add(new
                {
                    id = horarioDb?.Id ?? 0,
                    horaInicio = hora.ToString(@"hh\:mm"),
                    horaFin = fin.ToString(@"hh\:mm"),
                    estado = ocupado ? "ocupado" : "disponible",
                    descuento = descuento,
                    motivoDescuento = razones.Count > 0 ? string.Join(" + ", razones) : null
                });
            }

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
