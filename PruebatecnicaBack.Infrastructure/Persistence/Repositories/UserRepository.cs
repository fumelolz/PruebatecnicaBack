using PruebatecnicaBack.Application.Common.Persistence;
using PruebatecnicaBack.Contracts.Common;
using PruebatecnicaBack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace PruebatecnicaBack.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(User user)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
        }

        public Task<bool> CheckIfEmailExist(string username)
        {
            throw new NotImplementedException();
        }

        public Task Delete(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedList<User>> GetAllAsync(
            string? roleName,
            string? searchTerm,
            string? sortColumn,
            string? sortOrder,
            int page,
            int pageSize)
        {
            //return await _context.Users.Include(x => x.UserRoles).ThenInclude(x => x.Role).ToListAsync();
            IQueryable<User> usersQuery = _context.Users;

            // Filtrar por role si se proporciona un nombre de role
            if (!string.IsNullOrWhiteSpace(roleName))
            {
                usersQuery = usersQuery.Where(p =>
                    p.UserRoles.Any(cp => cp.Role.Name.Contains(roleName)));
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                usersQuery = usersQuery.Where(p => p.Name.Contains(searchTerm) || p.Email.Contains(searchTerm));
            }

            if (!string.IsNullOrWhiteSpace(sortColumn))
            {
                Expression<Func<User, object>> keySelector = sortColumn.ToLower() switch
                {
                    "name" => user => user.Name,
                    "updatedDate" => user => user.UpdatedDate,
                    "creationDate" => user => user.CreationDate,
                    _ => user => user.UserId
                };

                if (sortOrder.ToLower() == "desc")
                {
                    usersQuery = usersQuery.OrderByDescending(keySelector);
                }
                else
                {
                    usersQuery = usersQuery.OrderBy(keySelector);
                }
            }

            var userQuery = usersQuery
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role);

            var users = await PagedList<User>.CreateAsync(userQuery, page, pageSize);

            return users;
        }

        public Task<User> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.Include(x => x.UserRoles).ThenInclude(x => x.Role).FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User> GetUserByIdAsync(int? userId)
        {
            return await _context.Users.Include(x => x.UserRoles).ThenInclude(x => x.Role).Include(x => x.UserRoles).FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<User> GetUserByTokenAsync(string token)
        {
            return await _context.Users.Include(x => x.UserRoles).ThenInclude(x => x.Role).FirstOrDefaultAsync(x => x.RefreshToken == token);
        }

        public async Task<List<Role>> GetUserRoles(int id)
        {
            return await _context.UserRoles.Where(ur => ur.UserId == id).Select(ur => new Role() { Name = ur.Role.Name, RoleId = ur.Role.RoleId}).ToListAsync();
        }

        public async Task Update(User user)
        {
            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
