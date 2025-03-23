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
            .WithMany(e => e.Directors);


        builder.HasMany(e => e.DirectedMovies)
            .WithMany(e => e.Directors);


        builder.AddPersonEntitySharedMappings();
        builder.AddBaseEntityTemporalMappings();
    }
}
