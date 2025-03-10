using PruebatecnicaBack.Contracts.Common;
using PruebatecnicaBack.Domain.Entities;

namespace PruebatecnicaBack.Application.Common.Persistence;

public interface IScraperQueueRepository
{
    Task<bool> IsScrapingInProgress(int year);
    Task AddScraperJobAsync(ScraperQueue job);
    Task MarkJobAsCompleted(int year);
    Task<PagedList<ScraperQueue>> GetPendingJobs(
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        int page,
        int pageSize);
}
