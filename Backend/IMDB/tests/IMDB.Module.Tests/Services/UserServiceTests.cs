using IMDB.Application.Abstractions.Services;
using IMDB.Domain.Enums;
using IMDB.Module.Tests.Fixture;
using Microsoft.Extensions.DependencyInjection;

namespace IMDB.Module.Tests.Services;

[Collection("SharedCollection")]
public class UserServiceTests
{
    private readonly ModuleTestsInMemoryFixture _fixture;
    private readonly IUserService _service;
    public UserServiceTests(ModuleTestsInMemoryFixture fixture)
    {
        _fixture = fixture;
        _service = _fixture.ServiceProvider.GetRequiredService<IUserService>();
    }

    [Theory]
    [InlineData(null)] // Null user ID
    [InlineData((long)123456)] // Invalid user ID
    [InlineData((long)3)] // Deleted user
    public async Task ShouldFailToGetBasic(long? userId)
    {
        var (result, message) = await _service.GetUserBasicsAsync(userId);

        Assert.Null(result);
        Assert.NotNull(message);
    }

    [Fact]
    public async Task ShouldGetBasic()
    {
        long userId = 2;
        var (result, message) = await _service.GetUserBasicsAsync(userId);

        Assert.NotNull(result);
        Assert.Null(message);

        Assert.Equal(userId, result.Id);
        Assert.Equal("LocalAdmin", result.UserName);
        Assert.Equal("Admin Account", result.FullName);
    }

    [Fact]
    public async Task ShouldGetDetails()
    {
        long userId = 2;
        var (result, message) = await _service.GetUserDetailsAsync(userId);

        Assert.NotNull(result);
        Assert.Null(message);

        Assert.Equal(userId, result.Id);
        Assert.Equal("LocalAdmin", result.Username);
        Assert.Equal("Admin", result.FirstName);
        Assert.Equal("Account", result.LastName);
        Assert.Equal(Role.Administrator, result.Role);
        Assert.Equal("localadmin@imdb.com", result.Email);
    }

    [Theory]
    [InlineData(null)] // Null user ID
    [InlineData((long)123456)] // Invalid user ID
    [InlineData((long)3)] // Deleted user
    public async Task ShouldFailToGetDetails(long? userId)
    {
        var (result, message) = await _service.GetUserDetailsAsync(userId);

        Assert.Null(result);
        Assert.NotNull(message);
    }
}