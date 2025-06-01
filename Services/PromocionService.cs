using BackendReservas.Data;
using BackendReservas.Models;

public class PromocionService
{
    private readonly ApplicationDbContext _context;

    public PromocionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task GenerarPromocionesAutomaticas()
    {
        var hoy = DateTime.Today;
        var desde = hoy.AddDays(-30);

        // Cargar datos en memoria para evitar conflictos con MySQL
        var reservas = _context.Reservas
            .Where(r => r.Fecha >= desde && r.Estado == "confirmada")
            .ToList();

        var horarios = _context.Horarios.ToList();
        var canchas = _context.Canchas.ToList(); // ← este era el punto crítico

        var totalDias = (hoy - desde).Days;

        foreach (var cancha in canchas)
        {
            var horariosPorCancha = horarios.Where(h => h.CanchaId == cancha.Id);
            foreach (var horario in horariosPorCancha)
            {
                var reservasEnHorario = reservas.Count(r => r.HorarioId == horario.Id);
                double tasaOcupacion = (double)reservasEnHorario / totalDias;

                if (tasaOcupacion < 0.5)
                {
                    var promocionExistente = _context.Promociones
                        .FirstOrDefault(p =>
                            p.CanchaId == cancha.Id &&
                            p.HoraInicio == horario.HoraInicio &&
                            p.HoraFin == horario.HoraFin);

                    if (promocionExistente == null)
                    {
                        _context.Promociones.Add(new Promocion
                        {
                            CanchaId = cancha.Id,
                            HoraInicio = horario.HoraInicio,
                            HoraFin = horario.HoraFin,
                            PorcentajeDescuento = 20,
                            Activo = true
                        });
                    }
                }
            }
        }

        await _context.SaveChangesAsync();
    }
}
