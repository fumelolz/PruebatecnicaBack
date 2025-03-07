using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using PruebatecnicaBack.Infrastructure.Messaging;
using PruebatecnicaBack.Infrastructure.Persistence.Quartz.Jobs;
using Quartz;

namespace PruebatecnicaBack.Infrastructure.Persistence.Quartz
{
    public static class QuartzConfiguration
    {
        public static IServiceCollection AddQuartzConfiguration(this IServiceCollection services)
        {
            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();

                q.AddJob<ScraperJob>(opts => opts.WithIdentity("ScraperJob"));

                q.AddTrigger(opts => opts
                    .ForJob("ScraperJob")
                    .WithIdentity("ScraperTrigger")
                    .StartNow()
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(30).RepeatForever())); // Se ejecuta cada 30 segundos
            });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            return services;
        }
    }
}
