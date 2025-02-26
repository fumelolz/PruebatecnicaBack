using PruebatecnicaBack.Application.Common.Persistence;
using PruebatecnicaBack.Domain.Common.Errors;
using PruebatecnicaBack.Domain.Entities;
using ErrorOr;
using MediatR;
using BC = BCrypt.Net.BCrypt;

namespace PruebatecnicaBack.Application.Users.Commands.UpdateUsercommand
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ErrorOr<bool>>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<bool>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            if (await _userRepository.GetUserByIdAsync(request.UserId) is not User user)
            {
                return Errors.User.NotFound;
            }

            if (await _userRepository.GetUserByEmailAsync(request.Email) is not null && user.Email != request.Email)
            {
                return Errors.User.DuplicateEmail;
            }

            // Actualizar roles
            user.UpdateRoles(request.Roles);

            if(request.Password != "")
            {
                user.Password = BC.HashPassword(request.Password);
            }

            user.Birthday = request.Birthday;
            user.Email = request.Email;
            user.Name = request.Name;
            user.FirstSurname = request.FirstSurname;
            user.SecondSurname = request.SecondSurname;

            await _userRepository.Update(user);

            return true;
        }
    }
}
