using IMDB.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMDB.Infrastructure.Mappings;

internal class WatchListMapping : IEntityTypeConfiguration<WatchList>
{
    public void Configure(EntityTypeBuilder<WatchList> builder)
    {
        builder.ToTable("watch_lists", "imdb");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired()
            .ValueGeneratedOnAdd();


        builder.HasMany(e => e.Shows)
            .WithMany();

        builder.HasMany(e => e.Movies)
            .WithMany();

        builder.HasOne(e => e.User)
            .WithOne(u => u.WatchList)
            .HasForeignKey<User>(e => e.WatchListId);

        builder.AddBaseEntityTemporalMappings();
    }
}