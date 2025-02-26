using PruebatecnicaBack.Application.Common.Persistence;
using PruebatecnicaBack.Domain.Common.Errors;
using PruebatecnicaBack.Domain.Entities;
using ErrorOr;
using MediatR;

namespace PruebatecnicaBack.Application.Roles.Commands.CreateRole
{
    internal class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, ErrorOr<Role>>
    {
        private readonly IRoleRepository _roleRepository;

        public CreateRoleCommandHandler(IRoleRepository roleRepository)
        {
            _roleRepository=roleRepository;
        }

        public async Task<ErrorOr<Role>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            if(await _roleRepository.GetByNameAsync(request.Name) is not null)
            {
                return Errors.Role.AlReadyExist;
            }

            var role = new Role()
            {
                Name = request.Name,
                Description = request.Description
            };

            await _roleRepository.Add(role);

            return role;
        }
    }
}
