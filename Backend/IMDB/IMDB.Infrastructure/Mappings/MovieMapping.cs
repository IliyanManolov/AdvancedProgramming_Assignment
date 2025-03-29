using IMDB.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMDB.Infrastructure.Mappings;

internal class MovieMapping : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.ToTable("movies", "imdb");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(e => e.TrailerUrl)
            .HasColumnName("trailer_url");

        builder.Property(e => e.Length)
            .HasColumnName("length_seconds");

        // TODO: Test if this will work correctly
        builder.HasMany(e => e.Genres)
            .WithMany()
            .UsingEntity(
            "MovieGenre",
            l => l.HasOne(typeof(Movie)).WithMany().HasForeignKey("MovieId").HasPrincipalKey(nameof(Movie.Id)),
            r => r.HasOne(typeof(Genre)).WithMany().HasForeignKey("GenreId").HasPrincipalKey(nameof(Genre.Id)),
            j => j.HasKey("MovieId", "GenreId"));

        builder.HasMany(e => e.Actors)
            .WithMany()
            .UsingEntity(
            "ActorMovie",
            l => l.HasOne(typeof(Movie)).WithMany().HasForeignKey("MovieId").HasPrincipalKey(nameof(Movie.Id)),
            r => r.HasOne(typeof(Actor)).WithMany().HasForeignKey("ActorId").HasPrincipalKey(nameof(Actor.Id)),
            j => j.HasKey("ActorId", "MovieId"));

        builder.HasMany(e => e.Directors)
            .WithMany()
            .UsingEntity(
            "DirectorMovie",
            l => l.HasOne(typeof(Movie)).WithMany().HasForeignKey("MovieId").HasPrincipalKey(nameof(Movie.Id)),
            r => r.HasOne(typeof(Director)).WithMany().HasForeignKey("DirectorId").HasPrincipalKey(nameof(Director.Id)),
            j => j.HasKey("DirectorId", "MovieId")); ;

        builder.HasOne(e => e.CreatedByUser)
            .WithMany(u => u.CreatedMovies)
            .HasForeignKey(e => e.CreatedByUserId);
        

        builder.AddMediaSharedMappings();
        builder.AddBaseEntityTemporalMappings();
    }
}
