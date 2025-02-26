using PruebatecnicaBack.Application.Authentication.Commands.Common;
using PruebatecnicaBack.Application.Common.Interfaces.Authentication;
using PruebatecnicaBack.Application.Common.Persistence;
using PruebatecnicaBack.Domain.Common.Errors;
using PruebatecnicaBack.Domain.Entities;
using ErrorOr;
using MediatR;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PruebatecnicaBack.Application.Authentication.Commands.RefreshToken
{
    public class RefreshTokenQueryHandler : IRequestHandler<RefreshTokenQuery, ErrorOr<AuthenticationResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public RefreshTokenQueryHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository=userRepository;
            _jwtTokenGenerator=jwtTokenGenerator;
        }

        public async  Task<ErrorOr<AuthenticationResult>> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
        {
            // Check if user already exists
            if (await _userRepository.GetUserByTokenAsync(request.Token) is not User user)
            {
                return Errors.User.NotFound;
            }

            if (user == null || user.RefreshTokenExpiration < DateTime.UtcNow)
            {
                return Errors.Authentication.UnAuthorize;
            }

            // Create Jwt token
            var token = await _jwtTokenGenerator.GenerateToken(user);
            var refreshToken = await _jwtTokenGenerator.GenerateRefreshToken(user);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiration = DateTime.UtcNow.AddDays(7);

            await _userRepository.Update(user);

            var roles = await _userRepository.GetUserRoles(user.UserId);

            var account = new UserResult(
                user.UserId,
                user.Email,
                user.Name,
                user.FirstSurname,
                user.SecondSurname);

            var rolesToAdd = new List<RoleResult>();

            foreach (var role in roles)
            {
                rolesToAdd.Add(new RoleResult(role.RoleId, role.Name));
            }

            return new AuthenticationResult(
                token,
                refreshToken,
                DateTime.UtcNow.AddMinutes(10080),
                DateTime.UtcNow.AddMinutes(60),
                account,
                rolesToAdd);
        }
    }
}
