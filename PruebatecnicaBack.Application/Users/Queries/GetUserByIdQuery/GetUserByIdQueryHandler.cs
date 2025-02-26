using PruebatecnicaBack.Application.Common.Persistence;
using PruebatecnicaBack.Domain.Common.Errors;
using PruebatecnicaBack.Domain.Entities;
using ErrorOr;
using MediatR;

namespace PruebatecnicaBack.Application.Users.Queries.GetUserByIdQuery
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ErrorOr<User>>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository=userRepository;
        }

        public async Task<ErrorOr<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            if(await _userRepository.GetUserByIdAsync(request.UserId) is not User user)
            {
                return Errors.User.NotFound;
            }

            return user;
        }
    }
}
