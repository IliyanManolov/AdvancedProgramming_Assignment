using IMDB.Domain.Models;
using IMDB.Infrastructure.Mappings.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMDB.Infrastructure.Mappings;

internal class ReviewMapping : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("media_reviews", "imdb");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(e => e.IsDeleted)
            .HasColumnName("is_deleted")
            .HasConversion<bool>();

        builder.Property(e => e.ReviewText)
            .HasColumnName("review_text")
            .IsRequired();

        builder.Property(e => e.Rating)
            .HasColumnName("rating");

        builder.HasOne(e => e.User)
            .WithMany(e => e.Reviews)
            .HasForeignKey(e => e.UserId);

        builder.HasOne(e => e.Movie)
            .WithMany(m => m.Reviews)
            .HasForeignKey(e => e.MovieId);

        builder.HasOne(e => e.Show)
            .WithMany(m => m.Reviews)
            .HasForeignKey(e => e.ShowId);

        builder.HasOne(e => e.Episode)
            .WithMany(e => e.Reviews)
            .HasForeignKey(e => e.EpisodeId);

        builder.AddBaseEntityTemporalMappings();
    }
}