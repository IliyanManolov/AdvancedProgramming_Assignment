using IMDB.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMDB.Infrastructure.Mappings;

internal class DirectorMapping : IEntityTypeConfiguration<Director>
{
    public void Configure(EntityTypeBuilder<Director> builder)
    {
        builder.ToTable("directors", "imdb");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.HasOne(e => e.CreatedByUser)
            .WithMany(s => s.CreatedDirectors)
            .HasForeignKey(e => e.CreatedByUserId);

        builder.HasMany(e => e.DirectedShows)
            .WithMany(e => e.Directors)
            .UsingEntity<Dictionary<string, object>>(
                "DirectorTvShow",
                j => j
                    .HasOne<TvShow>()
                    .WithMany()
                    .HasForeignKey("TvShowId")
                    .OnDelete(DeleteBehavior.Restrict),
                j => j
                    .HasOne<Director>()
                    .WithMany()
                    .HasForeignKey("DirectorId")
                    .OnDelete(DeleteBehavior.Restrict),
                j =>
                {
                    j.HasKey("DirectorId", "TvShowId");
                });


        builder.HasMany(d => d.DirectedMovies)
            .WithMany(m => m.Directors)
            .UsingEntity<Dictionary<string, object>>(
                "DirectorMovie",
                j => j
                    .HasOne<Movie>()
                    .WithMany()
                    .HasForeignKey("MovieId")
                    .OnDelete(DeleteBehavior.Restrict),
                j => j
                    .HasOne<Director>()
                    .WithMany()
                    .HasForeignKey("DirectorId")
                    .OnDelete(DeleteBehavior.Restrict),
                j =>
                {
                    j.HasKey("DirectorId", "MovieId");
                });



        builder.AddPersonEntitySharedMappings();
        builder.AddBaseEntityTemporalMappings();
    }
}
