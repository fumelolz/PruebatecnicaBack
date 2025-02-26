using PruebatecnicaBack.Application.Authentication.Commands.Common;
using PruebatecnicaBack.Application.Common.Responses;
using ErrorOr;
using MediatR;

namespace PruebatecnicaBack.Application.Users.Commands.Register
{
    public record CreateUserCommand(
        string Name,
        string FirstSurname,
        string SecondSurname,
        string Email,
        string Password,
        DateTime Birthday,
        List<int> Roles) : IRequest<ErrorOr<AuthenticationResult>>;
}
