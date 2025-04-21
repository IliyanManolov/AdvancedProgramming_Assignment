using IMDB.Application.Abstractions.Repositories;
using IMDB.Application.Abstractions.Services;
using IMDB.Application.DTOs.ShowEpisode;
using IMDB.Module.Tests.Fixture;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;

namespace IMDB.Module.Tests.Services;

[Collection("SharedCollection")]
public class EpisodeServiceTests
{
    private readonly ModuleTestsInMemoryFixture _fixture;
    private readonly IShowEpisodeService _service;
    private readonly IEpisodeRepository _repository;

    public EpisodeServiceTests(ModuleTestsInMemoryFixture fixture)
    {
        _fixture = fixture;
        _service = _fixture.ServiceProvider.GetRequiredService<IShowEpisodeService>();
        _repository = _fixture.ServiceProvider.GetRequiredService<IEpisodeRepository>();
    }

    [Theory]
    [InlineData(1, "TestEpisode", (long)1, "2020-01-01", 1, "User does not exist")] // No access
    [InlineData(3, "TestEpisode", (long)1, "2000-01-01", 1, "User does not exist")] // Deleted User
    [InlineData(2, "TestEpisode", (long)1, "2020-01-01", 10000, "Show does not exist")] // Invalid show
    [InlineData(2, "TestEpisode", (long)-1, "2020-01-01", 1, "Invalid episode length")] // negative
    [InlineData(2, "", (long)20, "2020-01-01", 1, "Invalid episode title")] // Empty title
    public async Task ShouldFailToCreate(long userId, string title, long length, string dateTimeString, int showId, string errorContains)
    {
        var createDto = new CreateEpisodeDto()
        {
            CreatedByUserId = userId,
            Title = title,
            DateAired = DateTime.Parse(dateTimeString),
            Length = length,
            ShowId = showId,
            SeasonNumber = 1
        };

        var (result, message) = await _service.CreateAsync(createDto);

        Assert.Null(result);
        Assert.NotNull(message);

        Assert.Contains(errorContains, message, StringComparison.InvariantCultureIgnoreCase);
    }
}