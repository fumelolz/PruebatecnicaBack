using PruebatecnicaBack.Contracts.Common;
using PruebatecnicaBack.Domain.Entities;

namespace PruebatecnicaBack.Application.Common.Persistence
{
    public interface IZoneRepository
    {
        Task<PagedList<Zone>> GetAllAsync(
            string? searchTerm,
            string? sortColumn,
            string? sortOrder,
            int? year,
            decimal? minCapacity,
            decimal? maxCapacity,
            int page,
            int pageSize);
        Task<Zone?> GetByIdAsync(int id);
        Task AddAsync(Zone zone);
        Task UpdateAsync(Zone zone);
        Task DeleteAsync(int id);
        Task DeleteByYearAsync(int year);
        Task<bool> ExistsByYearAsync(int year);
        Task BulkInsertAsync(List<Zone> zones);
    }
}
