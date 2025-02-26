using PruebatecnicaBack.Application.Common.Persistence;
using PruebatecnicaBack.Contracts.Common;
using PruebatecnicaBack.Domain.Entities;
using MediatR;

namespace PruebatecnicaBack.Application.Roles.Queries.ListRoles
{
    internal class ListRoleQueryHandler : IRequestHandler<ListRoleQuery, PagedList<Role>>
    {
        private readonly IRoleRepository _roleRepository;
        public ListRoleQueryHandler(IRoleRepository roleRepository)
        {
            this._roleRepository=roleRepository;
        }

        public async Task<PagedList<Role>> Handle(ListRoleQuery request, CancellationToken cancellationToken)
        {
            return await _roleRepository.GetAllAsync(
                request.SearchTerm,
                request.SortColumn,
                request.SortOrder,
                request.Page,
                request.PageSize);
        }
    }
}
