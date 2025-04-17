using IMDB.Domain.Models;
using IMDB.Infrastructure.Mappings.Extensions;
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
            .WithOne(e => e.Director)
            .HasForeignKey(e => e.DirectorId);


        builder.HasMany(d => d.DirectedMovies)
            .WithOne(e => e.Director)
            .HasForeignKey(e => e.DirectorId);


        builder.AddPersonEntitySharedMappings();
        builder.AddBaseEntityTemporalMappings();
    }
}
