using System.Text.Json.Serialization;

namespace BackendReservas.Models
{
public class Reserva
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }

    public Usuario? Usuario { get; set; }  // ✅ sin JsonIgnore
    public int HorarioId { get; set; }

    public Horario? Horario { get; set; }  // ✅ sin JsonIgnore

    public DateTime Fecha { get; set; }
    public string Estado { get; set; } = string.Empty;
    public bool PromocionAplicada { get; set; }
}


}