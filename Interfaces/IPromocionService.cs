using BackendReservas.DTOs;

namespace BackendReservas.Interfaces
{
    public interface IPromocionService
    {
        List<BloqueHorarioDTO> CalcularBloquesHorario(int canchaId);
    }
}