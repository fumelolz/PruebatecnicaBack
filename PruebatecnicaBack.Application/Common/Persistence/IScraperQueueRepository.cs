using PruebatecnicaBack.Domain.Entities;

namespace PruebatecnicaBack.Application.Common.Persistence;

public interface IScraperQueueRepository
{
    Task<bool> IsScrapingInProgress(int year);
    Task AddScraperJobAsync(ScraperQueue job);
    Task MarkJobAsCompleted(int year);
    Task<List<ScraperQueue>> GetPendingJobs();
}
