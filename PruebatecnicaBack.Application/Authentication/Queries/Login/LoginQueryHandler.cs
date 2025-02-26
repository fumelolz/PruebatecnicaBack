using PruebatecnicaBack.Application.Common.Interfaces.Authentication;
using PruebatecnicaBack.Application.Common.Persistence;
using ErrorOr;
using MediatR;
using PruebatecnicaBack.Domain.Common.Errors;
using PruebatecnicaBack.Domain.Entities;
using PruebatecnicaBack.Application.Authentication.Commands.Login;
using PruebatecnicaBack.Application.Authentication.Commands.Common;
using BC = BCrypt.Net.BCrypt;

namespace PruebatecnicaBack.Application.Authentication.Commands.Register
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginQueryHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var errors = new List<Error>();
            // Validate user exist
            if(await _userRepository.GetUserByEmailAsync(request.Email) is not User user)
            {
                errors.Add(Errors.Authentication.InvalidCredentials);
                return errors;
            }

            // Validate password
            if (BC.Verify(request.Password, user.Password) is false)
            {
                errors.Add(Errors.Authentication.InvalidCredentials);
                return errors;
            }

            var roles = await _userRepository.GetUserRoles(user.UserId);

            var token =  await _jwtTokenGenerator.GenerateToken(user);
            var refreshToken = await _jwtTokenGenerator.GenerateRefreshToken(user);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiration = DateTime.UtcNow.AddMinutes(10080);

            await _userRepository.Update(user);

            var account = new UserResult(
                user.UserId,
                user.Email,
                user.Name,
                user.FirstSurname,
                user.SecondSurname);


            var rolesToAdd = new List<RoleResult>();

            foreach (var role in roles)
            {
                rolesToAdd.Add(new RoleResult(role.RoleId,role.Name));
            }

            return new AuthenticationResult(
                token,
                refreshToken,
                DateTime.Now.AddDays(7),
                DateTime.Now.AddMinutes(60),
                account,
                rolesToAdd); ;

        }
    }
}
