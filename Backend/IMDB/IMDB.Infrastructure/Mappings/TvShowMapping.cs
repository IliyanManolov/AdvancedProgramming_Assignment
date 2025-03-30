using IMDB.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMDB.Infrastructure.Mappings;

internal class TvShowMapping : IEntityTypeConfiguration<TvShow>
{
    public void Configure(EntityTypeBuilder<TvShow> builder)
    {
        builder.ToTable("tv_shows", "imdb");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Seasons)
            .HasColumnName("seasons");

        builder.HasMany(e => e.Episodes)
            .WithOne(ep => ep.Show)
            .HasForeignKey(ep => ep.ShowId);

        builder.Property(e => e.EndDate)
            .HasColumnName("end_date")
            .HasConversion(
                v => v.Value.ToUniversalTime(),
                v => new DateTime(v.Ticks, DateTimeKind.Utc));

        builder.Property(e => e.Genres)
            .HasColumnName("genres");


        builder.HasOne(e => e.Director)
            .WithMany(e => e.DirectedShows)
            .HasForeignKey(e => e.DirectorId);

        builder.HasOne(e => e.CreatedByUser)
            .WithMany(u => u.CreatedShows)
            .HasForeignKey(e => e.CreatedByUserId);

        builder.AddMediaSharedMappings();
        builder.AddBaseEntityTemporalMappings();
    }
}