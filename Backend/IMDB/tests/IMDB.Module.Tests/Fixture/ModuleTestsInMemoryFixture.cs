using IMDB.Application.Abstractions.Services;
using IMDB.Infrastructure.Database;
using IMDB.Module.Tests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDB.Module.Tests.Fixture;

public class ModuleTestsInMemoryFixture
{
    public IServiceProvider ServiceProvider { get; private set; }

    public ModuleTestsInMemoryFixture()
    {
        var services = DiHelper.GetServices();

        ServiceProvider = services.BuildServiceProvider();

        var dbContext = ServiceProvider.GetRequiredService<DatabaseContext>();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        dbContext.SeedData(ServiceProvider.GetRequiredService<IPasswordService>());
    }
}


[CollectionDefinition("SharedCollection")]
public class BasicCollection : ICollectionFixture<ModuleTestsInMemoryFixture>
{
    // Intentionally empty
    // Each different collection requires such a class
}