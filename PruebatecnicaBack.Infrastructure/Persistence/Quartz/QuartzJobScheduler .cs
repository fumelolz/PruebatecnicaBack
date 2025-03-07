using PruebatecnicaBack.Application.Common.Interfaces.Scheduling;
using PruebatecnicaBack.Infrastructure.Persistence.Quartz.Jobs;
using Quartz;

namespace PruebatecnicaBack.Infrastructure.Persistence.Quartz;

public class QuartzJobScheduler : IJobScheduler
{
    private readonly ISchedulerFactory _schedulerFactory;
    public QuartzJobScheduler(ISchedulerFactory schedulerFactory)
    {
        _schedulerFactory = schedulerFactory;
    }

    public async Task ScheduleScraperJob(int year, bool update, int userId)
    {
        var scheduler = await _schedulerFactory.GetScheduler();

        var jobData = new JobDataMap
            {
                { "year", year },
                { "update", update },
                { "userId", userId }
            };

        var jobKey = new JobKey($"ScraperJob-{year}");
        var jobExists = await scheduler.CheckExists(jobKey);

        if(jobExists)
        {
            return;
        }

        var job = JobBuilder.Create<ScraperJob>()
            .WithIdentity(jobKey)
            .UsingJobData(jobData)
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity($"ScraperTrigger-{year}")
            .StartNow()
            .Build();

        await scheduler.ScheduleJob(job, trigger);
        scheduler.ListenerManager.AddJobListener(new ScraperJobListener());
    }
}
