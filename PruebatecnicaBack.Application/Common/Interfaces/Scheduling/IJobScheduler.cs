namespace PruebatecnicaBack.Application.Common.Interfaces.Scheduling;

public interface IJobScheduler
{
    Task ScheduleScraperJob(int year, bool update, int userId);
}
