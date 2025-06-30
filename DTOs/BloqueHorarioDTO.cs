namespace BackendReservas.DTOs
{
    public class BloqueHorarioDTO
    {
        public int Id { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }
        public string Estado { get; set; }
        public int Descuento { get; set; }
        public string? MotivoDescuento { get; set; }
    }
}