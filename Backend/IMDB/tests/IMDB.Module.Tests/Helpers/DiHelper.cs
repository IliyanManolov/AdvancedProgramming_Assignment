using IMDB.Application;
using IMDB.Application.Abstractions.Services;
using IMDB.Application.Services;
using IMDB.Domain.Enums;
using IMDB.Domain.Models;
using IMDB.Infrastructure;
using IMDB.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDB.Module.Tests.Helpers;

public static class DiHelper
{
    public static IServiceCollection GetServices()
    {
        var services = new ServiceCollection();

        services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseInMemoryDatabase("ModuleTestsDatabase");
        });

        services.AddLogging();
        services.AddApplicationLayer();
        services.AddRepositories();

        return services;
    }

    public static void SeedData(this DatabaseContext context, IPasswordService passwordService)
    {
        var password = new PasswordService();

        context.Users.Add(new User() { Id = 1, CreateTimeStamp = DateTime.UtcNow, FirstName = "John", LastName = "Doe", Username = "JohnDoeAccount", Role = Role.User, Email = "johndoe@imdb.com", Password = password.GetHash("password") });
        context.Users.Add(new User() { Id = 2, CreateTimeStamp = DateTime.UtcNow, FirstName = "Admin", LastName = "Account", Username = "LocalAdmin", Role = Role.Administrator, Email = "localadmin@imdb.com", Password = password.GetHash("admin") });
        context.Users.Add(new User() { Id = 3, CreateTimeStamp = DateTime.UtcNow, FirstName = "Deleted", LastName = "User", Username = "DeletedUser", Role = Role.User, Email = "deleted@imdb.com", Password = password.GetHash("deletedpassword"), IsDeleted = true });
        context.Users.Add(new User() { Id = 4, CreateTimeStamp = DateTime.UtcNow, FirstName = "Watch", LastName = "List", Username = "WatchlistUser", Role = Role.User, Email = "watchlist@imdb.com", Password = password.GetHash("password") });

        var genresDict = new Dictionary<string, Genre>
        {
            { "Thriller", new Genre() { Id = 1, Name = "Thriller", CreateTimeStamp = DateTime.UtcNow, CreatedByUserId = 2 } },
            { "Horror", new Genre() { Id = 2, Name = "Horror", CreateTimeStamp = DateTime.UtcNow, CreatedByUserId = 2 } },
            { "Comedy", new Genre() { Id = 3, Name = "Comedy", CreateTimeStamp = DateTime.UtcNow, CreatedByUserId = 2 } },
            { "Action", new Genre() { Id = 4, Name = "Action", CreateTimeStamp = DateTime.UtcNow, CreatedByUserId = 2 } },
            { "Science Fiction", new Genre() { Id = 5, Name = "Science Fiction", CreateTimeStamp = DateTime.UtcNow, CreatedByUserId = 2 } },
            { "Romantic", new Genre() { Id = 6, Name = "Romantic", CreateTimeStamp = DateTime.UtcNow, CreatedByUserId = 2 } }
        };

        context.Genres.AddRange(genresDict.Values);

        var moviesDict = new Dictionary<string, Movie>
        {
            { "Basic",
                new Movie()
                {
                    CreatedByUserId = 2,
                    Description = "Basic Movie Description",
                    DirectorId = 1,
                    Genres = new HashSet<Genre>(){ { genresDict["Horror"] } },
                    Rating = 10,
                    Reviews = new HashSet<Review>(),
                    Title = "Basic Movie Title",
                    Id = 1
                }
            }
        };

        var showsDict = new Dictionary<string, TvShow>
        {
            { "Basic",
                new TvShow()
                {
                    CreatedByUserId = 2,
                    Description = "Basic Movie Description",
                    DirectorId = 1,
                    Genres = new HashSet<Genre>(){ { genresDict["Horror"] } },
                    Rating = 10,
                    Reviews = new HashSet<Review>(),
                    Title = "Basic Show Title",
                    Id = 1,
                    Seasons = 1
                }
            }
        };

        context.Movies.AddRange(moviesDict.Values);
        context.TvShows.AddRange(showsDict.Values);

        context.WatchLists.Add(new WatchList() { UserId = 1, Id = 1 });
        context.WatchLists.Add(new WatchList() { UserId = 2, Id = 2, Movies = new HashSet<Movie>() { { moviesDict["Basic"] } } });
        context.WatchLists.Add(new WatchList() { UserId = 3, Id = 3 });
        context.WatchLists.Add(new WatchList() { UserId = 4, Id = 4 });

        context.ShowEpisodes.Add(new ShowEpisode()
        {
            CreatedByUserId = 2,
            Description = "Random episode description",
            ShowId = 1,
            SeasonNumber = 1,
            Title = "Randomg episode"
        });

        context.Actors.Add(new Actor() { BirthDate = DateTime.ParseExact("2001-02-21", "yyyy-MM-dd", CultureInfo.InvariantCulture), Id = 1, Biography = "Actor #1", CreatedByUserId = 2, FirstName = "John", LastName = "Doe", Nationality = "Bulgarian" });
        context.Actors.Add(new Actor() { BirthDate = DateTime.ParseExact("1998-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture), Id = 2, Biography = "Actor #2", CreatedByUserId = 2, FirstName = "Jane", LastName = "Doe", Nationality = "Bulgarian" });
        context.Actors.Add(new Actor() { BirthDate = DateTime.ParseExact("2002-12-12", "yyyy-MM-dd", CultureInfo.InvariantCulture), Id = 3, Biography = "Actor #3", CreatedByUserId = 2, FirstName = "Sam", LastName = "Smith", Nationality = "Bulgarian" });

        context.Directors.Add(new Director() { Id = 1, FirstName = "Director First Name", LastName = "Director Last Name", CreatedByUserId = 2, BirthDate = DateTime.ParseExact("2002-12-12", "yyyy-MM-dd", CultureInfo.InvariantCulture), Nationality = "Bulgarian" });

        context.SaveChanges();
    }
}
