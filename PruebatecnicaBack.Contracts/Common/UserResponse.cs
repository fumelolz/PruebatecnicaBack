using PruebatecnicaBack.Contracts.Roles;

namespace PruebatecnicaBack.Contracts.Common
{
    public record UserResponse(int UserId, string Email, string Name, string FirstSurname, string SecondSurname, DateTime Birthday, DateTime CreationDate, DateTime UpdatedDate, List<RoleSimplifiedResponse> Roles);
}
