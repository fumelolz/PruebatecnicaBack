using PruebatecnicaBack.Domain.Entities;
using PruebatecnicaBack.Infrastructure.Persistence.Seedding;
using Microsoft.EntityFrameworkCore;

namespace PruebatecnicaBack.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            SeedingUserRoles.Seed(modelBuilder);
            SeedingUsers.Seed(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        // Usuarios
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<ScraperQueue> ScraperQueues { get; set; }
    }
}
