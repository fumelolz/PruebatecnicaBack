using PruebatecnicaBack.Application.Common.Persistence;
using PruebatecnicaBack.Domain.Entities;
using MediatR;
using PruebatecnicaBack.Domain.Common.Errors;
using ErrorOr;

namespace PruebatecnicaBack.Application.Roles.Commands.UpdateRole
{
    
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, ErrorOr<Role>>
    {
        private readonly IRoleRepository _roleRepository;

        public UpdateRoleCommandHandler(IRoleRepository roleRepository)
        {
            _roleRepository=roleRepository;
        }
        public async Task<ErrorOr<Role>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetByIdAsync(request.RoleId);

            if(role == null)
            {
                return Errors.Role.NotFound;
            }

            if (await _roleRepository.GetByNameAsync(request.Name) is not null && role.Name != request.Name)
            {
                return Errors.Role.AlReadyExist;
            }

            role.Name = request.Name;
            role.Description = request.Description;

            await _roleRepository.Update(role);

            return role;
        }
    }
}
