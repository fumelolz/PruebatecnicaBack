
using PruebatecnicaBack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace PruebatecnicaBack.Infrastructure.Persistence.Seedding
{
    public class SeedingUsers
    {
        public static void Seed(ModelBuilder modelBuilder)
        {

            var daniel = new User()
            {
                UserId = 1,
                Birthday = DateTime.Now,
                Email = "dany_magadan@hotmail.com",
                FirstSurname = "Magadan",
                IsActive = true,
                Name = "Kevin Daniel",
                Password = "$2a$11$/SkGnWoAMFQH0xEPxRh5tuYf6g.swQqSIfdLDTotaxWG3F8ZFYKpi",
                SecondSurname = "Garcia",
            };

            var juan = new User()
            {
                UserId = 2,
                Name = "Juan",
                FirstSurname = "Perez",
                SecondSurname = "Garcia",
                Email = "juan_perez@hotmail.com",
                Birthday = DateTime.Now,
                IsActive = true,
                Password = "$2a$11$/SkGnWoAMFQH0xEPxRh5tuYf6g.swQqSIfdLDTotaxWG3F8ZFYKpi",
            };


            var userRoleAdminDaniel = new UserRole()
            {
                RoleId = 1,
                UserId = 1,
            };

            var userRoleUserDaniel = new UserRole()
            {
                RoleId = 2,
                UserId = 1,
            };


            var userRolesUserJuan = new UserRole()
            {
                RoleId = 2,
                UserId = 2
            };


            modelBuilder.Entity<User>().HasData(daniel,juan);
            modelBuilder.Entity<UserRole>().HasData(userRoleAdminDaniel,userRoleUserDaniel,userRolesUserJuan);
        }
    }
}
