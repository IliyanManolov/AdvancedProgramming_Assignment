using IMDB.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMDB.Infrastructure.Mappings;

internal class WatchListMapping : IEntityTypeConfiguration<WatchList>
{
    public void Configure(EntityTypeBuilder<WatchList> builder)
    {
        builder.ToTable("watch_lists", "imdb");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired()
            .ValueGeneratedOnAdd();


        builder.HasMany(e => e.Shows)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "WatchlistTvShow",
                j => j
                    .HasOne<TvShow>()
                    .WithMany()
                    .HasForeignKey("TvShowId")
                    .OnDelete(DeleteBehavior.Restrict),
                j => j
                    .HasOne<WatchList>()
                    .WithMany()
                    .HasForeignKey("WatchlistId")
                    .OnDelete(DeleteBehavior.Restrict),
                j =>
                {
                    j.HasKey("WatchlistId", "TvShowId");
                });

        builder.HasMany(e => e.Movies)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "WatchlistMovie",
                j => j
                    .HasOne<Movie>()
                    .WithMany()
                    .HasForeignKey("MovieId")
                    .OnDelete(DeleteBehavior.Restrict),
                j => j
                    .HasOne<WatchList>()
                    .WithMany()
                    .HasForeignKey("WatchlistId")
                    .OnDelete(DeleteBehavior.Restrict),
                j =>
                {
                    j.HasKey("WatchlistId", "MovieId");
                });

        builder.HasOne(e => e.User)
            .WithOne(u => u.WatchList)
            .HasForeignKey<User>(e => e.WatchListId);

        builder.AddBaseEntityTemporalMappings();
    }
}