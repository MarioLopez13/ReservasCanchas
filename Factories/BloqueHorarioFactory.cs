using BackendReservas.DTOs;

namespace BackendReservas.Factories
{
    public static class BloqueHorarioFactory
    {
        public static BloqueHorarioDTO Crear(
            int id,
            TimeSpan inicio,
            TimeSpan fin,
            bool ocupado,
            int descuento,
            string? motivo)
        {
            return new BloqueHorarioDTO
            {
                Id = id,
                HoraInicio = inicio.ToString(@"hh\:mm"),
                HoraFin = fin.ToString(@"hh\:mm"),
                Estado = ocupado ? "ocupado" : "disponible",
                Descuento = descuento,
                MotivoDescuento = motivo
            };
        }
    }
}