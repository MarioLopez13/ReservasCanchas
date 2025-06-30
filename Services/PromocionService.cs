using BackendReservas.Data;
using BackendReservas.DTOs;
using BackendReservas.Factories;
using BackendReservas.Interfaces;
using BackendReservas.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendReservas.Services
{
    public class PromocionService : IPromocionService
    {
        private readonly ApplicationDbContext _context;

        private static PromocionService? _instancia;
        private static readonly object _lock = new();

        private PromocionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public static PromocionService Instancia(ApplicationDbContext context)
        {
            lock (_lock)
            {
                _instancia ??= new PromocionService(context);
                return _instancia;
            }
        }

        public List<BloqueHorarioDTO> CalcularBloquesHorario(int canchaId)
        {
            var horaInicio = new TimeSpan(7, 0, 0);
            var horaFin = new TimeSpan(23, 0, 0);
            var bloques = new List<BloqueHorarioDTO>();

            var horarios = _context.Horarios
                .Where(h => h.CanchaId == canchaId)
                .ToList();

            var reservasHoy = _context.Reservas
                .Include(r => r.Horario)
                .Where(r =>
                    r.Horario != null &&
                    r.Horario.CanchaId == canchaId &&
                    r.Fecha.Date == DateTime.Today &&
                    r.Estado == "confirmada")
                .ToList();

            int reservasTotalesHoy = reservasHoy.Count;
            var ahora = DateTime.Now.TimeOfDay;

            for (var hora = horaInicio; hora < horaFin; hora = hora.Add(TimeSpan.FromHours(1)))
            {
                var fin = hora.Add(TimeSpan.FromHours(1));
                var horarioDb = horarios.FirstOrDefault(h => h.HoraInicio == hora);

                bool ocupado = reservasHoy.Any(r => r.Horario?.HoraInicio == hora);
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

                if (!ocupado && horarioDb?.Disponible == true && hora > ahora)
                {
                    var fueLiberado = _context.Reservas.Any(r =>
                        r.HorarioId == horarioDb.Id &&
                        r.Estado == "cancelada" &&
                        r.Fecha.Date == DateTime.Today &&
                        r.Fecha.TimeOfDay >= ahora.Subtract(TimeSpan.FromMinutes(5)));

                    if (fueLiberado)
                    {
                        descuento = Math.Max(descuento, 50);
                        razones.Add("Horario liberado por cancelación reciente");
                    }
                }

                bloques.Add(BloqueHorarioFactory.Crear(
                    horarioDb?.Id ?? 0,
                    hora,
                    fin,
                    ocupado,
                    descuento,
                    razones.Count > 0 ? string.Join(" + ", razones) : null
                ));
            }

            return bloques;
        }

        public async Task GenerarPromocionesAutomaticas()
        {
            var promociones = new List<Promocion>();

            var canchas = await _context.Canchas.ToListAsync();
            foreach (var cancha in canchas)
            {
                promociones.Add(new Promocion
                {
                    CanchaId = cancha.Id,
                    PorcentajeDescuento = 15,
                    HoraInicio = new TimeSpan(13, 0, 0),
                    HoraFin = new TimeSpan(15, 0, 0),
                    Activo = true
                });
            }

            _context.Promociones.AddRange(promociones);
            await _context.SaveChangesAsync();
        }
    }
}
