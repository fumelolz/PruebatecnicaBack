using PruebatecnicaBack.Contracts.Common;
using PruebatecnicaBack.Domain.Entities;

namespace PruebatecnicaBack.Application.Common.Persistence
{
    public interface IRoleRepository
    {
        Task<PagedList<Role>> GetAllAsync(
            string? searchTerm,
            string? sortColumn,
            string? sortOrder,
            int page,
            int pageSize);
        Task<List<Role>> GetAllUserRolesAsync(int userId);
        Task<Role> GetByIdAsync(int id);

        Task<Role> GetByNameAsync(string name);
        Task Add(Role role);
        Task Update(Role role);
        Task Delete(Role role);
        Task DeleteRolesByIdAsync(List<UserRole> roles);

        Task<bool> CheckIfUserHasRoleAsync(int roleId, int userId);
        Task DeleteRoleByIdAsync(int roleId);
    }
}
