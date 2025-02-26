using PruebatecnicaBack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PruebatecnicaBack.Infrastructure.Persistence.Configurations;

internal class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.UserId);

        builder.Property(u => u.UserId)
            .ValueGeneratedOnAdd();

        builder.HasMany(u => u.UserRoles)
            .WithOne(ur => ur.User)
            .HasForeignKey(ur => ur.UserId);

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(u => u.Password)
            .IsRequired();

        builder.Property(u => u.FirstSurname)
            .HasMaxLength(100);

        builder.Property(u => u.SecondSurname)
            .HasMaxLength(100);

        builder.Property(u => u.Birthday);

        builder.Property(u => u.IsActive)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(u => u.PhoneNumber)
            .HasMaxLength(15);

        builder.Property(u => u.Address)
            .HasMaxLength(250);
        builder.Property(u => u.LastLogin)
            .IsRequired(false);
    }
}
