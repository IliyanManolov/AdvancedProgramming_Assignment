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

        builder.HasMany(e => e.Genres)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "MovieGenre",
                j => j
                    .HasOne<Genre>()
                    .WithMany()
                    .HasForeignKey("GenreId")
                    .OnDelete(DeleteBehavior.Restrict),
                j => j
                    .HasOne<Movie>()
                    .WithMany()
                    .HasForeignKey("MovieId")
                    .OnDelete(DeleteBehavior.Restrict),
                j =>
                {
                    j.HasKey("GenreId", "MovieId");
                });

        builder.HasOne(e => e.CreatedByUser)
            .WithMany(u => u.CreatedMovies)
            .HasForeignKey(e => e.CreatedByUserId);

        builder.HasOne(e => e.Director)
            .WithMany(e => e.DirectedMovies)
            .HasForeignKey(e => e.DirectorId);
        

        builder.AddMediaSharedMappings();
        builder.AddBaseEntityTemporalMappings();
    }
}
