using IMDB.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMDB.Infrastructure.Mappings;

internal class GenreMapping : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.ToTable("genres", "imdb");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Name)
            .HasColumnName("genre_name");

        builder.HasOne(e => e.CreatedByUser)
            .WithMany(s => s.CreatedGenres)
            .HasForeignKey(e => e.CreatedByUserId);

        builder.AddBaseEntityTemporalMappings();
    }
}