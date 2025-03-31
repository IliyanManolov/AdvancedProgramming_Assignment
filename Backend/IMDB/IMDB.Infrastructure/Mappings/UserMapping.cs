using IMDB.Domain.Enums;
using IMDB.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMDB.Infrastructure.Mappings;

internal class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users", "imdb");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired()
            .ValueGeneratedOnAdd();


        builder.Property(e => e.Username)
            .HasColumnName("username")
            .IsRequired();

        builder.Property(e => e.Email)
            .HasColumnName("email")
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(e => e.Password)
            .HasColumnName("password")
            .IsRequired();

        builder.Property(e => e.FirstName)
            .HasColumnName("first_name");

        builder.Property(e => e.LastName)
            .HasColumnName("last_name");

        builder.Property(e => e.IsDeleted)
            .HasColumnName("is_deleted")
            .HasConversion<bool>();

        builder.Property(e => e.Role)
            .HasColumnName("role")
            .HasConversion<string>()
            .HasDefaultValue(Role.User);

        builder.HasOne(e => e.WatchList)
            .WithOne(w => w.User)
            .HasForeignKey<WatchList>(e => e.UserId);

        builder.HasMany(e => e.CreatedShows)
            .WithOne(c => c.CreatedByUser)
            .HasForeignKey(c => c.CreatedByUserId);

        builder.HasMany(e => e.CreatedMovies)
            .WithOne(c => c.CreatedByUser)
            .HasForeignKey(c => c.CreatedByUserId);

        builder.HasMany(e => e.CreatedActors)
            .WithOne(c => c.CreatedByUser)
            .HasForeignKey(c => c.CreatedByUserId);

        builder.HasMany(e => e.CreatedDirectors)
            .WithOne(c => c.CreatedByUser)
            .HasForeignKey(c => c.CreatedByUserId);

        builder.HasMany(e => e.CreatedEpisodes)
            .WithOne(c => c.CreatedByUser)
            .HasForeignKey(c => c.CreatedByUserId);

        builder.HasMany(e => e.CreatedGenres)
            .WithOne(c => c.CreatedByUser)
            .HasForeignKey(c => c.CreatedByUserId);

        builder.AddBaseEntityTemporalMappings();
    }
}