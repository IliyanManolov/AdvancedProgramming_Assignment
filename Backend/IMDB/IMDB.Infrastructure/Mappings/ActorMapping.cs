using IMDB.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMDB.Infrastructure.Mappings;

internal class ActorMapping : IEntityTypeConfiguration<Actor>
{
    public void Configure(EntityTypeBuilder<Actor> builder)
    {
        builder.ToTable("actors", "imdb");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.HasOne(e => e.CreatedByUser)
            .WithMany(s => s.CreatedActors)
            .HasForeignKey(e => e.CreatedByUserId);

        builder.HasMany(a => a.ParticipatedShows)
        .WithMany(t => t.Actors)
        .UsingEntity(
            "ActorTvShow",
            r => r.HasOne(typeof(Actor)).WithMany().HasForeignKey("ActorId").OnDelete(DeleteBehavior.Restrict).HasPrincipalKey(nameof(Actor.Id)),
            l => l.HasOne(typeof(TvShow)).WithMany().HasForeignKey("TvShowId").OnDelete(DeleteBehavior.Restrict).HasPrincipalKey(nameof(TvShow.Id)),
            j => j.HasKey("ActorId", "TvShowId"));

        

        builder.HasMany(e => e.ParticipatedMovies)
            .WithMany(e => e.Actors)
            .UsingEntity(
            "ActorMovie",
            r => r.HasOne(typeof(Actor)).WithMany().HasForeignKey("ActorId").OnDelete(DeleteBehavior.Restrict).HasPrincipalKey(nameof(Actor.Id)),
            l => l.HasOne(typeof(Movie)).WithMany().HasForeignKey("MovieId").OnDelete(DeleteBehavior.Restrict).HasPrincipalKey(nameof(Movie.Id)),
            j => j.HasKey("ActorId", "MovieId"));

        builder.AddPersonEntitySharedMappings();
        builder.AddBaseEntityTemporalMappings();
    }
}