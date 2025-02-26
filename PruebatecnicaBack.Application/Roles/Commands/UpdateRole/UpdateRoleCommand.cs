using PruebatecnicaBack.Domain.Entities;
using ErrorOr;
using MediatR;

namespace PruebatecnicaBack.Application.Roles.Commands.UpdateRole
{
    public record UpdateRoleCommand(int RoleId, string Name, string Description) : IRequest<ErrorOr<Role>>;
}
