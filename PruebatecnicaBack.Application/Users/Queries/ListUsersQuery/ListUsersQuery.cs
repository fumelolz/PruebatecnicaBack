using PruebatecnicaBack.Contracts.Common;
using PruebatecnicaBack.Domain.Entities;
using MediatR;

namespace PruebatecnicaBack.Application.Users.Queries.ListUsersQuery
{
    public record ListUsersQuery(
        string? roleName,
        string? SearchTerm,
        string? SortColumn,
        string? SortOrder,
        int Page,
        int PageSize) : IRequest<PagedList<User>>;
}
