namespace BackendReservas.Models
{
public class Promocion {
    public int Id { get; set; }
    public int CanchaId { get; set; }
    public Cancha? Cancha { get; set; }
    public double PorcentajeDescuento { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFin { get; set; }
    public bool Activo { get; set; }
}
}