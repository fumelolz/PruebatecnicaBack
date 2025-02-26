using PruebatecnicaBack.Domain.Entities;
using ErrorOr;
using MediatR;

namespace PruebatecnicaBack.Application.Users.Queries.GetUserByIdQuery
{
    public record GetUserByIdQuery(int UserId) : IRequest<ErrorOr<User>>;
}
