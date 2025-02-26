using PruebatecnicaBack.Domain.Entities;
using ErrorOr;
using MediatR;

namespace PruebatecnicaBack.Application.Roles.Queries.RoleById
{
    public record GetRoleByIdQuery(int RoleId) : IRequest<ErrorOr<Role>>;
}
