﻿using IMDB.Domain.AbstractModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMDB.Infrastructure.Mappings.Extensions;

public static class MappingBuilderConfigurationExtensions
{
    public static void AddBaseEntityTemporalMappings<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : DomainEntity
    {
        builder.Property(e => e.CreateTimeStamp)
            .HasColumnName("create_date")
            .HasConversion(
                v => v.Value.ToUniversalTime(),
                v => new DateTime(v.Ticks, DateTimeKind.Utc));

        builder.Property(e => e.UpdateTimeStamp)
            .HasColumnName("update_date")
            .HasConversion(
                v => v.Value.ToUniversalTime(),
                v => new DateTime(v.Ticks, DateTimeKind.Utc));
    }

    public static void AddPersonEntitySharedMappings<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : Person
    {

        builder.Property(e => e.FirstName)
            .HasColumnName("first_name")
            .IsRequired();

        builder.Property(e => e.LastName)
            .HasColumnName("last_name")
            .IsRequired();


        builder.Property(e => e.Biography)
            .HasColumnName("biography");

        builder.Property(e => e.Nationality)
            .HasColumnName("nationality");

        builder.Property(e => e.ProfileImage)
            .HasColumnName("profile_image")
            .HasColumnType("VARBINARY(MAX)");

        builder.Property(e => e.BirthDate)
            .HasColumnName("date_birth")
            .HasConversion(
                v => v.Value.ToUniversalTime(),
                v => new DateTime(v.Ticks, DateTimeKind.Utc))
            .IsRequired();

        builder.Property(e => e.DateOfDeath)
            .HasColumnName("date_death")
            .HasConversion(
                v => v.Value.ToUniversalTime(),
                v => new DateTime(v.Ticks, DateTimeKind.Utc));
    }

    public static void AddMediaSharedMappings<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : Media
    {
        builder.Property(e => e.ReleaseDate)
            .HasColumnName("release_date")
            .HasConversion(
                v => v.ToUniversalTime(),
                v => new DateTime(v.Ticks, DateTimeKind.Utc));

        builder.Property(e => e.Title)
            .HasColumnName("title")
            .IsRequired();

        builder.Property(e => e.Rating)
            .HasColumnName("rating");

        builder.Property(e => e.Description)
            .HasColumnName("description");

        builder.Property(e => e.PosterImage)
            .HasColumnName("poster_image")
            .HasColumnType("VARBINARY(MAX)");

    }
}