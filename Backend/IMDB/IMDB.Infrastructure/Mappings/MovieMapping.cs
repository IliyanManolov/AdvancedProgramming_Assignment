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
                j => j.HasOne(typeof(Genre))
                    .WithMany()
                    .HasForeignKey("GenreId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasPrincipalKey(nameof(Genre.Id)),
                j => j.HasOne(typeof(Movie))
                    .WithMany()
                    .HasForeignKey("MovieId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasPrincipalKey(nameof(Movie.Id)),
                j => j.HasKey("MovieId", "GenreId")
            );

        builder.HasOne(e => e.CreatedByUser)
            .WithMany(u => u.CreatedMovies)
            .HasForeignKey(e => e.CreatedByUserId);
        

        builder.AddMediaSharedMappings();
        builder.AddBaseEntityTemporalMappings();
    }
}
