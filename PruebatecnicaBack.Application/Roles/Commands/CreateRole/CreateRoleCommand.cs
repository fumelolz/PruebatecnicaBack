using PruebatecnicaBack.Domain.Entities;
using ErrorOr;
using MediatR;


namespace PruebatecnicaBack.Application.Roles.Commands.CreateRole
{
    public record CreateRoleCommand(string Name, string Description) : IRequest<ErrorOr<Role>>;
}
