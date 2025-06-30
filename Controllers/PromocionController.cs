using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendReservas.Data;
using BackendReservas.Models;
using BackendReservas.Services;

namespace BackendReservas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PromocionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PromocionController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPromociones()
        {
            var promociones = await _context.Promociones.Include(p => p.Cancha).ToListAsync();
            return Ok(promociones);
        }

        [HttpPost]
        public async Task<IActionResult> CrearPromocion([FromBody] Promocion promocion)
        {
            _context.Promociones.Add(promocion);
            await _context.SaveChangesAsync();
            return Ok(promocion);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarPromocion(int id, [FromBody] Promocion promocion)
        {
            var existente = await _context.Promociones.FindAsync(id);
            if (existente == null) return NotFound();

            existente.CanchaId = promocion.CanchaId;
            existente.PorcentajeDescuento = promocion.PorcentajeDescuento;
            existente.HoraInicio = promocion.HoraInicio;
            existente.HoraFin = promocion.HoraFin;
            existente.Activo = promocion.Activo;

            await _context.SaveChangesAsync();
            return Ok(existente);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarPromocion(int id)
        {
            var promocion = await _context.Promociones.FindAsync(id);
            if (promocion == null) return NotFound();

            _context.Promociones.Remove(promocion);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("generar-automaticas")]
        public async Task<IActionResult> GenerarPromociones()
        {
            var servicio = PromocionService.Instancia(_context);
            await servicio.GenerarPromocionesAutomaticas();
            return Ok("Promociones generadas automáticamente.");
        }
    }
}