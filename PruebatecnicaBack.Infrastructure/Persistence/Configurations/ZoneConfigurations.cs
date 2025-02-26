using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PruebatecnicaBack.Domain.Entities;

namespace PruebatecnicaBack.Infrastructure.Persistence.Configurations;

public class ZoneConfigurations : IEntityTypeConfiguration<Zone>
{
    public void Configure(EntityTypeBuilder<Zone> builder)
    {
        builder.HasKey(x => x.ZoneId);

        builder.Property(x => x.ZoneId)
            .ValueGeneratedOnAdd();
    }
}
