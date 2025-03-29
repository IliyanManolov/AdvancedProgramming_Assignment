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

        // TODO: Test if this will work correctly
        builder.HasMany(e => e.Genres)
            .WithMany()
            .UsingEntity(
                "TvShowGenre",
                l => l.HasOne(typeof(TvShow)).WithMany().HasForeignKey("TvShowId").HasPrincipalKey(nameof(TvShow.Id)),
                r => r.HasOne(typeof(Genre)).WithMany().HasForeignKey("GenreId").HasPrincipalKey(nameof(Genre.Id)),
                j => j.HasKey("TvShowId", "GenreId"));

        builder.HasMany(e => e.Actors)
            .WithMany()
            .UsingEntity(
                "ActorTvShow",
                l => l.HasOne(typeof(TvShow)).WithMany().HasForeignKey("TvShowId").HasPrincipalKey(nameof(TvShow.Id)),
                r => r.HasOne(typeof(Actor)).WithMany().HasForeignKey("ActorId").HasPrincipalKey(nameof(Actor.Id)),
                j => j.HasKey("ActorId", "TvShowId"));


        builder.HasMany(e => e.Directors)
            .WithMany()
            .UsingEntity("DirectorTvShow",
                l => l.HasOne(typeof(TvShow)).WithMany().HasForeignKey("TvShowId").HasPrincipalKey(nameof(TvShow.Id)),
                r => r.HasOne(typeof(Director)).WithMany().HasForeignKey("DirectorId").HasPrincipalKey(nameof(Director.Id)),
                j => j.HasKey("DirectorId", "TvShowId"));

        builder.HasOne(e => e.CreatedByUser)
            .WithMany(u => u.CreatedShows)
            .HasForeignKey(e => e.CreatedByUserId);

        builder.AddMediaSharedMappings();
        builder.AddBaseEntityTemporalMappings();
    }
}