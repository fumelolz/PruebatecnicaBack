using PruebatecnicaBack.Application.Common.Persistence;
using PruebatecnicaBack.Contracts.Common;
using PruebatecnicaBack.Domain.Entities;
using MediatR;

namespace PruebatecnicaBack.Application.Users.Queries.ListUsersQuery
{
    public class ListUsersQueryHandler : IRequestHandler<ListUsersQuery, PagedList<User>>
    {
        private readonly IUserRepository _userRepository;

        public ListUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository=userRepository;
        }

        public async Task<PagedList<User>> Handle(ListUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync(
                request.roleName,
                request.SearchTerm,
                request.SortColumn,
                request.SortOrder,
                request.Page,
                request.PageSize);
            return users;
        }
    }
}
