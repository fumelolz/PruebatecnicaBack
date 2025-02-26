using ErrorOr;
using MediatR;

namespace PruebatecnicaBack.Application.Users.Commands.UpdateUsercommand
{
    public record UpdateUserCommand(
        int UserId,
        string Name,
        string FirstSurname,
        string SecondSurname,
        string Email,
        string Password,
        DateTime Birthday,
        List<int> Roles) : IRequest<ErrorOr<bool>>;
}
