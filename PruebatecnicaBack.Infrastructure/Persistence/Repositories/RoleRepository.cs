using PruebatecnicaBack.Application.Common.Persistence;
using PruebatecnicaBack.Contracts.Common;
using PruebatecnicaBack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace PruebatecnicaBack.Infrastructure.Persistence.Repositories
{
    internal class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
        {
            _context=context;
        }

        public async Task Add(Role role)
        {
            try
            {
                _context.Add(role);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task Delete(Role role)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRoleByIdAsync(int roleId)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteRolesByIdAsync(List<UserRole> roles)
        {
            try
            {

                foreach (UserRole role in roles)
                {
                    _context.UserRoles.Remove(role);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> CheckIfUserHasRoleAsync(int roleId, int userId)
        {
            try
            {
                var hasRole = await _context.UserRoles.Where(x => x.RoleId == roleId && x.UserId == userId).FirstOrDefaultAsync();

                if(hasRole is null)
                { 
                    return false;
                }

                return true;

        }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PagedList<Role>> GetAllAsync(
            string? searchTerm,
            string? sortColumn,
            string? sortOrder,
            int page,
            int pageSize)
        {
            IQueryable<Role> rolesQuery = _context.Roles;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                rolesQuery = rolesQuery.Where(p =>
                p.Name.Contains(searchTerm) ||
                p.Description.Contains(searchTerm));
            }

            if (!string.IsNullOrWhiteSpace(sortColumn))
            {
                Expression<Func<Role, object>> keySelector = sortColumn.ToLower() switch
                {
                    "name" => role => role.Name,
                    "description" => role => role.Description,
                    "updatedDate" => role => role.UpdatedDate,
                    "creationDate" => role => role.CreationDate,
                    _ => role => role.RoleId
                };

                if (sortOrder.ToLower() == "desc")
                {
                    rolesQuery = rolesQuery.OrderByDescending(keySelector);
                }
                else
                {
                    rolesQuery = rolesQuery.OrderBy(keySelector);
                }
            }

            var rolequery = rolesQuery.AsNoTracking();

            var roles = await PagedList<Role>.CreateAsync(rolequery, page, pageSize);

            return roles;
        }

        public async Task<List<Role>> GetAllUserRolesAsync(int userId)
        {
            return await _context.UserRoles.Where(x => x.UserId == userId).Select(x => new Role()
            {
                Name = x.Role.Name,
                CreationDate = x.Role.CreationDate,
                RoleId = x.Role.RoleId,
                Description = x.Role.Description,
                UpdatedDate = x.Role.UpdatedDate
            }).ToListAsync();
        }

        public async Task<Role> GetByIdAsync(int roleId)
        {
            return await _context.Roles.Where(x => x.RoleId == roleId).FirstOrDefaultAsync();
        }

        public async Task<Role> GetByNameAsync(string Name)
        {
            return await _context.Roles.Where(x => x.Name == Name).FirstOrDefaultAsync();
        }

        public async Task Update(Role role)
        {
            try
            {
                _context.Update(role);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
