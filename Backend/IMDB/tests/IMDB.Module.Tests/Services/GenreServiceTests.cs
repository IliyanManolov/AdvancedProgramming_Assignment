using IMDB.Application.Abstractions.Services;
using IMDB.Application.DTOs.Genres;
using IMDB.Application.DTOs.ShowEpisode;
using IMDB.Module.Tests.Fixture;
using Microsoft.Extensions.DependencyInjection;

namespace IMDB.Module.Tests.Services;

[Collection("SharedCollection")]
public class GenreServiceTests
{
    private readonly ModuleTestsInMemoryFixture _fixture;
    private readonly IGenreService _service;

    public GenreServiceTests(ModuleTestsInMemoryFixture fixture)
    {
        _fixture = fixture;
        _service = _fixture.ServiceProvider.GetRequiredService<IGenreService>();
    }


    #region Success cases

    [Theory]
    [InlineData("Horror")]
    [InlineData("Thriller")]
    public async Task ShouldGetByName(string? name)
    {
        var (result, message) = await _service.GetByNameAsync(name);

        Assert.NotNull(result);
        Assert.Null(message);

        Assert.Equal(name, result.Name);
        Assert.NotNull(result.Id);
    }

    [Theory]
    [InlineData((long)1)]
    public async Task ShouldGetById(long? id)
    {
        var (result, message) = await _service.GetByIdAsync(id);

        Assert.NotNull(result);
        Assert.Null(message);

        Assert.Equal("Thriller", result.Name);
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task ShouldGetAll()
    {
        var (result, message) = await _service.GetAllAsync();

        Assert.NotNull(result);
        Assert.Null(message);

        Assert.NotEmpty(result);
        Assert.True(result.Count() > 2);

        foreach (var item in result)
        {
            Assert.False(string.IsNullOrEmpty(item.Name), "Empty/null name found");
            Assert.NotNull(item.Id);
        }
    }

    [Theory]
    [InlineData(2, "TestCreateGenre")]
    public async Task ShouldCreateNewGenre(long userId, string genreName)
    {
        var newGenre = new CreateGenreDto()
        {
            CreatedByUserId = userId,
            Name = genreName,
        };

        var (result, message) = await _service.CreateAsync(newGenre);

        Assert.NotNull(result);
        Assert.Null(message);

        // Get the created genre and validate its name to make sure it was actually created
        var (getResult, getMessage) = await _service.GetByIdAsync(result);

        Assert.NotNull(getResult);
        Assert.Null(getMessage);

        Assert.Equal(genreName, getResult.Name);
    }

    #endregion

    #region Failure cases

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("Non Existant")]
    [InlineData("HORROR")] // Invalid case
    public async Task ShouldNotGetByName(string? name)
    {
        var (result, message) = await _service.GetByNameAsync(name);

        Assert.Null(result);
        Assert.NotNull(message);
    }

    [Theory]
    [InlineData((long)200)]
    [InlineData(null)]
    public async Task ShouldNotGetById(long? id)
    {
        var (result, message) = await _service.GetByIdAsync(id);

        Assert.Null(result);
        Assert.NotNull(message);
    }

    [Theory]
    [InlineData(2, "Thriller", "Thriller already exists")]
    [InlineData(1, "NewGenre", "UNAUTHORIZED")]
    public async Task ShouldFailToCreate(long userId, string genreName, string errorContains)
    {
        var newGenre = new CreateGenreDto()
        {
            CreatedByUserId = userId,
            Name = genreName,
        };

        var (result, message) = await _service.CreateAsync(newGenre);

        Assert.Null(result);
        Assert.NotNull(message);

        Assert.Contains(errorContains, message, StringComparison.InvariantCultureIgnoreCase);
    }

    #endregion
}