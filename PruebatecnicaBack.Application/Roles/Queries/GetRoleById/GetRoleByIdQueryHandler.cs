
using PruebatecnicaBack.Application.Common.Persistence;
using PruebatecnicaBack.Domain.Common.Errors;
using PruebatecnicaBack.Domain.Entities;
using ErrorOr;
using MediatR;

namespace PruebatecnicaBack.Application.Roles.Queries.RoleById
{
    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, ErrorOr<Role>>
    {
        private readonly IRoleRepository _roleRepository;

        public GetRoleByIdQueryHandler(IRoleRepository roleRepository)
        {
            _roleRepository=roleRepository;
        }

        public async Task<ErrorOr<Role>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            if (await _roleRepository.GetByIdAsync(request.RoleId) is not Role role)
            {
                return Errors.Role.NotFound;
            }

            return role;
        }
    }
}
