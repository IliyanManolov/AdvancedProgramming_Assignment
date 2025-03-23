using IMDB.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMDB.Infrastructure.Mappings;

internal class ActorMapping : IEntityTypeConfiguration<Actor>
{
    public void Configure(EntityTypeBuilder<Actor> builder)
    {
        builder.ToTable("actors", "imdb");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.HasOne(e => e.CreatedByUser)
            .WithMany(s => s.CreatedActors)
            .HasForeignKey(e => e.CreatedByUserId);

        builder.HasMany(e => e.ParticipatedShows)
            .WithMany(e => e.Actors);


        builder.HasMany(e => e.ParticipatedMovies)
            .WithMany(e => e.Actors);


        builder.AddPersonEntitySharedMappings();
        builder.AddBaseEntityTemporalMappings();
    }
}