using IMDB.Domain.AbstractModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMDB.Infrastructure.Mappings;

public static class MappingBuilderConfigurationExtensions
{
    public static void AddBaseEntityTemporalMappings<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : DomainEntity
    {
        builder.Property(e => e.CreateTimeStamp)
            .HasColumnName("create_date")
            .HasConversion(
                v => v.Value.ToUniversalTime(),
                v => new DateTime(v.Ticks, DateTimeKind.Utc));

        builder.Property(e => e.UpdateTimeStamp)
            .HasColumnName("update_date")
            .HasConversion(
                v => v.Value.ToUniversalTime(),
                v => new DateTime(v.Ticks, DateTimeKind.Utc));
    }
}