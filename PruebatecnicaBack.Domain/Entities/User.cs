using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PruebatecnicaBack.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstSurname { get; set; }
        public string SecondSurname { get; set; }
        public DateTime Birthday { get; set; }
        public bool IsActive { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
        public List<UserRole> UserRoles { get; set; } = new List<UserRole>();
        

        // Datos adicionales
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        // Seguridad
        public DateTime? LastLogin { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public void AddRole(int roleId)
        {
            if (!UserRoles.Any(ur => ur.RoleId == roleId))
            {
                UserRoles.Add(new UserRole { UserId = UserId, RoleId = roleId });
            }
        }

        public void RemoveRole(int roleId)
        {
            var userRole = UserRoles.FirstOrDefault(ur => ur.RoleId == roleId);
            if (userRole != null)
            {
                UserRoles.Remove(userRole);
            }
        }

        public void UpdateRoles(List<int> newRoleIds)
        {
            UserRoles ??= new List<UserRole>();

            var currentRoleIds = UserRoles.Select(ur => ur.RoleId).ToList();

            var rolesToAdd = newRoleIds.Except(currentRoleIds)
                                       .Select(roleId => new UserRole { UserId = UserId, RoleId = roleId })
                                       .ToList();

            var rolesToRemove = UserRoles.Where(ur => !newRoleIds.Contains(ur.RoleId)).ToList();

            if(rolesToRemove.Count > 0)
            {
                foreach (var role in rolesToRemove)
                {
                    UserRoles.Remove(role);
                }
            }

            if(rolesToAdd.Count > 0)
            {
                UserRoles.AddRange(rolesToAdd);
            }
        }

    }
}
