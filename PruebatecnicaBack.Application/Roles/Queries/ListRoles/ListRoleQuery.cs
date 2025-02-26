using PruebatecnicaBack.Contracts.Common;
using PruebatecnicaBack.Domain.Entities;
using MediatR;

namespace PruebatecnicaBack.Application.Roles.Queries.ListRoles
{
    public record ListRoleQuery(
        string? SearchTerm,
        string? SortColumn,
        string? SortOrder,
        int Page,
        int PageSize) : IRequest<PagedList<Role>>;
}
