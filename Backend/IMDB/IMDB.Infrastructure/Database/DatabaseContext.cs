using IMDB.Domain.Models;
using IMDB.Infrastructure.Mappings;
using Microsoft.EntityFrameworkCore;

namespace IMDB.Infrastructure.Database;

public class DatabaseContext : DbContext
{
    public DbSet<Actor> Actors { get; set; }
    public DbSet<ShowEpisode> ShowEpisodes { get; set; }
    public DbSet<Director> Directors { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<TvShow> TvShows { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<WatchList> WatchLists { get; set; }


    public DatabaseContext(DbContextOptions dbContextOptions)
    : base(dbContextOptions)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.ApplyConfiguration(new ShowEpisodeMapping());
        modelBuilder.ApplyConfiguration(new ActorMapping());
        modelBuilder.ApplyConfiguration(new DirectorMapping());
        modelBuilder.ApplyConfiguration(new MovieMapping());
        modelBuilder.ApplyConfiguration(new TvShowMapping());
        modelBuilder.ApplyConfiguration(new UserMapping());
        modelBuilder.ApplyConfiguration(new WatchListMapping());

        base.OnModelCreating(modelBuilder);
    }
}
