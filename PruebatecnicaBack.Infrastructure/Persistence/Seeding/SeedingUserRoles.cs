
using PruebatecnicaBack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace PruebatecnicaBack.Infrastructure.Persistence.Seedding
{
    public class SeedingUserRoles
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            var admin = new Role()
            {
                RoleId = 1,
                Description = "Rol de administrador",
                Name = "Administrador",
            };

            var user = new Role()
            {
                RoleId = 2,
                Description = "Rol de usuario",
                Name = "Usuario",
            };

            modelBuilder.Entity<Role>().HasData(admin, user);

        }
    }
}
