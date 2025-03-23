using IMDB.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMDB.Infrastructure.Mappings;

internal class ShowEpisodeMapping : IEntityTypeConfiguration<ShowEpisode>
{
    public void Configure(EntityTypeBuilder<ShowEpisode> builder)
    {

        builder.ToTable("showEpisodes", "imdb");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Length)
            .HasColumnName("length_seconds");

        builder.Property(e => e.Title)
            .HasColumnName("title");

        builder.Property(e => e.Description)
            .HasColumnName("description");

        builder.Property(e => e.SeasonNumber)
            .HasColumnName("season_number")
            .IsRequired();

        builder.Property(e => e.Rating)
            .HasColumnName("rating");

        builder.Property(e => e.Reviews)
            .HasColumnName("reviews_count");

        builder.Property(e => e.DateAired)
            .HasColumnName("aired_date")
            .IsRequired();

        builder.HasOne(e => e.Show)
            .WithMany(s => s.Episodes)
            .HasForeignKey(e => e.ShowId);

        builder.HasOne(e => e.CreatedByUser)
            .WithMany(s => s.CreatedEpisodes)
            .HasForeignKey(e => e.CreatedByUserId);

        builder.AddBaseEntityTemporalMappings();
    }
}
