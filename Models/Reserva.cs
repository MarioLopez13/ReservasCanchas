using System.Text.Json.Serialization;

namespace BackendReservas.Models
{
public class Reserva
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    [JsonIgnore] // <- Añade esto
    public Usuario? Usuario { get; set; } // ahora puede ser null

    public int HorarioId { get; set; }

    [JsonIgnore] // <- Añade esto
    public Horario? Horario { get; set; } // ahora puede ser null

    public DateTime Fecha { get; set; }

    public string Estado { get; set; }

    public bool PromocionAplicada { get; set; }
}

}