using Microsoft.EntityFrameworkCore;
using BackendReservas.Models;

namespace BackendReservas.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cancha> Canchas { get; set; }
        public DbSet<Horario> Horarios { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Promocion> Promociones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reserva>()
                .HasIndex(r => new { r.UsuarioId, r.Fecha })
                .HasDatabaseName("IX_Reserva_Usuario_Fecha");
        }
    }
}
