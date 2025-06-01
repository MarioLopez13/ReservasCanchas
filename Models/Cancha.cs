namespace BackendReservas.Models
{
public class Cancha {
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Tipo { get; set; } // fútbol, vóley, etc.
    public string Ubicacion { get; set; }
}
}