using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PruebatecnicaBack.Domain.Entities;

namespace PruebatecnicaBack.Infrastructure.Persistence.Configurations;

internal class ScraperQueueConfigurations : IEntityTypeConfiguration<ScraperQueue>
{
    public void Configure(EntityTypeBuilder<ScraperQueue> builder)
    {
        builder.HasKey(u => u.ScraperQueueId);

        builder.Property(u => u.ScraperQueueId)
            .ValueGeneratedOnAdd();
    }
}
