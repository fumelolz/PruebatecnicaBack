using PruebatecnicaBack.Application.Authentication.Commands.Common;
using ErrorOr;
using MediatR;

namespace PruebatecnicaBack.Application.Authentication.Commands.RefreshToken
{
    public record RefreshTokenQuery(string Token) : IRequest<ErrorOr<AuthenticationResult>>;
}
