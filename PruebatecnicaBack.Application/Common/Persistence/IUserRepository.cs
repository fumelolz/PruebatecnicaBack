using PruebatecnicaBack.Contracts.Common;
using PruebatecnicaBack.Domain.Entities;

namespace PruebatecnicaBack.Application.Common.Persistence
{
    public interface IUserRepository
    {
        Task<PagedList<User>> GetAllAsync(
            string? roleName,
            string? searchTerm,
            string? sortColumn,
            string? sortOrder,
            int page,
            int pageSize);
        Task<User?> GetAsync(int id);
        Task<User?> GetUserByIdAsync(int? id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByTokenAsync(string token);
        Task<bool> CheckIfEmailExist(string username);
        Task<List<Role>> GetUserRoles(int id);
        Task Add(User user);
        Task Update(User user);
        Task Delete(User user);
    }
}
