using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vinoteca.Entities;
using Vinoteca.Entities.MicrosoftIdentity;

namespace Vinoteca.DataAccess
{
    public class DbDataAccess : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public virtual DbSet<Vino> Vinos { get; set; }
        public virtual DbSet<Bodega> Bodegas { get; set; }
        public virtual DbSet<Provincia> Provincias { get; set; }
        public virtual DbSet<Variedad> Variedades { get; set; }
        public virtual DbSet<BodegasPorProvincias> BodegasPorProvincias { get; set; }
        public virtual DbSet<VinosPorBodegas> VinosPorBodegas { get; set; }
        public virtual DbSet<VinosVariedades> VinosVariedades { get; set; }
        public DbDataAccess(DbContextOptions<DbDataAccess> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.LogTo(Console.WriteLine).EnableDetailedErrors();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
