using IMDB.Application.Abstractions.Services;
using IMDB.Application.DTOs.Media;
using IMDB.Application.DTOs.Watchlist;
using IMDB.Module.Tests.Fixture;
using Microsoft.Extensions.DependencyInjection;

namespace IMDB.Module.Tests.Services;

[Collection("NonParallelCollection")]
public class WatchlistServiceTests
{
    private readonly ModuleTestsInMemoryFixture _fixture;
    private readonly IWatchListService _service;
    public WatchlistServiceTests(ModuleTestsInMemoryFixture fixture)
    {
        _fixture = fixture;
        _service = _fixture.ServiceProvider.GetRequiredService<IWatchListService>();
    }

    [Theory]
    [InlineData(null)] // Null user ID
    [InlineData((long)123456)] // Invalid user ID
    [InlineData((long)3)] // Deleted user
    public async Task ShouldFailToGetBasic(long? userId)
    {
        var (result, message) = await _service.GetListBasicAsync(userId);

        Assert.Null(result);
        Assert.NotNull(message);
    }

    [Theory]
    [InlineData(2, 1)]
    public async Task ShouldToGetBasic(long userId, long expected)
    {
        var (result, message) = await _service.GetListBasicAsync(userId);

        Assert.NotNull(result);
        Assert.Null(message);
        Assert.Equal(expected, result.MediaCount);
    }


    [Theory]
    [InlineData(null)] // Null user ID
    [InlineData((long)123456)] // Invalid user ID
    [InlineData((long)3)] // Deleted user
    public async Task ShouldFailToGetDetails(long? userId)
    {
        var (result, message) = await _service.GetListDetailsAsync(userId);

        Assert.Null(result);
        Assert.NotNull(message);
    }

    // Only assert that a list is returned. The transformer has its own tests
    [Theory]
    [InlineData(2, 1)]
    public async Task ShouldToGetDetails(long userId, long expected)
    {
        var (result, message) = await _service.GetListDetailsAsync(userId);

        Assert.NotNull(result);
        Assert.Null(message);
        Assert.Equal(expected, result.Media.Count);
    }


    // Execution flow:
    // Validate empty watchlist
    // Add
    // Validate 1 element with correct ID & Type
    // Delete
    // Validate empty watchlist
    [Theory]
    [InlineData(MediaType.Movie)]
    [InlineData(MediaType.TvShow)]
    public async Task ShouldHandleChanges(MediaType type)
    {
        long userId = 1;
        var model = new WatchlistModificationDto()
        {
            Id = 1,
            Type = type
        };

        // Validate initial watchlist is empty
        var (initial, _) = await _service.GetListBasicAsync(userId);
        Assert.Equal(0, initial!.MediaCount);

        var (addStatus, addError) = await _service.AddElement(model, userId);

        Assert.True(addStatus, "Adding element failed");
        Assert.Null(addError);

        // Validate watchlist element is added
        var (added, _) = await _service.GetListDetailsAsync(userId);

        Assert.NotNull(added);
        Assert.Single(added.Media);
        Assert.Equal(1, added.Media.First().Id);
        Assert.Equal(type, added.Media.First().Type);

        // Remove
        var (removeStatus, removeError) = await _service.DeleteElement(model, userId);

        Assert.True(removeStatus, "Removing element failed");
        Assert.Null(removeError);

        // Validate end watchlist is empty
        var (remove, _) = await _service.GetListDetailsAsync(userId);
        Assert.NotNull(remove);
        Assert.Empty(remove.Media);
    }

    [Theory]
    [InlineData(MediaType.Movie)]
    [InlineData(MediaType.TvShow)]
    public async Task ShouldFailToAddInvalidId(MediaType type)
    {
        long userId = 4;
        var model = new WatchlistModificationDto()
        {
            Id = 12345,
            Type = type
        };

        // Validate initial watchlist is empty
        var (initial, _) = await _service.GetListBasicAsync(userId);
        Assert.Equal(0, initial!.MediaCount);

        var (addStatus, addError) = await _service.AddElement(model, userId);

        Assert.False(addStatus, "Adding element did not fail");
        Assert.Contains("not found", addError);
    }

    [Theory]
    [InlineData(MediaType.Movie)]
    [InlineData(MediaType.TvShow)]
    public async Task ShouldFailToAddExisting(MediaType type)
    {
        long userId = 4;
        var model = new WatchlistModificationDto()
        {
            Id = 1,
            Type = type
        };

        // Validate initial watchlist is empty
        var (initial, _) = await _service.GetListBasicAsync(userId);
        Assert.Equal(0, initial!.MediaCount);

        var (addStatus, addError) = await _service.AddElement(model, userId);

        Assert.True(addStatus, "Initial Adding element failed");
        Assert.Null(addError);

        var (finalStatus, finalError) = await _service.AddElement(model, userId);

        Assert.False(finalStatus, "Final Adding element did not fail");
        Assert.Contains("already exists", finalError);

        await Cleanup(userId, model);
    }

    [Theory]
    [InlineData(MediaType.Movie)]
    [InlineData(MediaType.TvShow)]
    public async Task ShouldFailToDelete(MediaType type)
    {
        long userId = 4;
        var model = new WatchlistModificationDto()
        {
            Id = 12345,
            Type = type
        };

        // Validate initial watchlist is empty
        var (initial, _) = await _service.GetListBasicAsync(userId);
        Assert.Equal(0, initial!.MediaCount);

        var (addStatus, addError) = await _service.DeleteElement(model, userId);

        Assert.False(addStatus, "Deleting element did not fail");
        Assert.Contains("not found", addError);
    }

    private async Task Cleanup(long userId, WatchlistModificationDto dto)
        => await _service.DeleteElement(dto, userId);
}