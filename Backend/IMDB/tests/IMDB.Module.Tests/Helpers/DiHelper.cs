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

        context.Genres.Add(new Genre() { Id = 1, Name = "Thriller", CreateTimeStamp = DateTime.UtcNow, CreatedByUserId = 2 });
        context.Genres.Add(new Genre() { Id = 2, Name = "Horror", CreateTimeStamp = DateTime.UtcNow, CreatedByUserId = 2 });
        context.Genres.Add(new Genre() { Id = 3, Name = "Comedy", CreateTimeStamp = DateTime.UtcNow, CreatedByUserId = 2 });
        context.Genres.Add(new Genre() { Id = 4, Name = "Action", CreateTimeStamp = DateTime.UtcNow, CreatedByUserId = 2 });
        context.Genres.Add(new Genre() { Id = 5, Name = "Science Fiction", CreateTimeStamp = DateTime.UtcNow, CreatedByUserId = 2 });
        context.Genres.Add(new Genre() { Id = 6, Name = "Romantic", CreateTimeStamp = DateTime.UtcNow, CreatedByUserId = 2 });

        context.SaveChanges();
    }
}
