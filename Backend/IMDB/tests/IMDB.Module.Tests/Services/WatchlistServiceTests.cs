using IMDB.Application.Abstractions.Services;
using IMDB.Module.Tests.Fixture;
using Microsoft.Extensions.DependencyInjection;

namespace IMDB.Module.Tests.Services;

[Collection("SharedCollection")]
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
}