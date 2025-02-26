using PruebatecnicaBack.Application.Common.Interfaces.Authentication;
using PruebatecnicaBack.Application.Common.Persistence;
using ErrorOr;
using MediatR;
using PruebatecnicaBack.Domain.Common.Errors;
using PruebatecnicaBack.Domain.Entities;
using PruebatecnicaBack.Application.Authentication.Commands.Common;
using BC = BCrypt.Net.BCrypt;

namespace PruebatecnicaBack.Application.Users.Commands.Register
{
    public class CreateUserCommandHandler :
        IRequestHandler<CreateUserCommand, ErrorOr<AuthenticationResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public CreateUserCommandHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Verificar si el usuario ya existe
            if (await _userRepository.GetUserByEmailAsync(request.Email) is not null)
            {
                return Errors.User.DuplicateEmail;
            }

            // Crear el usuario
            var user = new User
            {
                Name = request.Name,
                FirstSurname = request.FirstSurname,
                SecondSurname = request.SecondSurname,
                Email = request.Email,
                Password = BC.HashPassword(request.Password),
                Birthday = request.Birthday
            };

            // Asignar roles al usuario antes de guardarlo en la BD
            user.UpdateRoles(request.Roles);

            // Guardar usuario en la BD
            await _userRepository.Add(user);

            // Generar token
            var token = await _jwtTokenGenerator.GenerateToken(user);
            var refreshToken = await _jwtTokenGenerator.GenerateRefreshToken(user);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiration = DateTime.UtcNow.AddDays(7);

            await _userRepository.Update(user);

            var userRoles = await _roleRepository.GetAllUserRolesAsync(user.UserId);

            return new AuthenticationResult(
                token,
                refreshToken,
                DateTime.UtcNow.AddDays(7),
                DateTime.UtcNow.AddHours(1),
                new UserResult(user.UserId, user.Email, user.Name, user.FirstSurname, user.SecondSurname),
                userRoles.Select(role => new RoleResult(role.RoleId, role.Name))
                        .ToList()
            );
        }
    }
}
