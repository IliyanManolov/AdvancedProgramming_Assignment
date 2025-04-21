using IMDB.Application.Abstractions.Services;
using IMDB.Application.DTOs.User;
using IMDB.Domain.Enums;
using IMDB.Domain.Models;
using IMDB.Module.Tests.Fixture;
using Microsoft.Extensions.DependencyInjection;

namespace IMDB.Module.Tests.Services;

[Collection("SharedCollection")]
public class UserServiceTests
{
    private readonly ModuleTestsInMemoryFixture _fixture;
    private readonly IUserService _service;
    private readonly IWatchListService _watchlistService;

    public UserServiceTests(ModuleTestsInMemoryFixture fixture)
    {
        _fixture = fixture;
        _service = _fixture.ServiceProvider.GetRequiredService<IUserService>();
        _watchlistService = _fixture.ServiceProvider.GetRequiredService<IWatchListService>();
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

    [Theory]
    [InlineData("Invalid password", "TestUser", "test@imdb.com", "", "")] // Empty pass
    [InlineData("Invalid password", "TestUser", "test@imdb.com", null, null)] // Null pass
    [InlineData("Invalid password", "TestUser", "test@imdb.com", "password", "")] // Empty confirm-pass
    [InlineData("Invalid password", "TestUser", "test@imdb.com", "", "password")] // Empty pass
    [InlineData("Passwords mismatch", "TestUser", "test@imdb.com", "PASSWORD", "password")] // Mismatch pass and confirm-pass
    [InlineData("Username already exists", "JohnDoeAccount", "test@imdb.com", "password", "password")] // Existing username
    [InlineData("Email already used", "TestUser", "johndoe@imdb.com", "password", "password")] // Existing email
    public async Task ShouldFailToRegister(string expectedError, string username, string email, string? password, string? confirmPassword)
    {
        var model = new CreateUserDto()
        {
            Username = username,
            Password = password,
            ConfirmPassword = confirmPassword,
            Email = email
        };

        var (result, message) = await _service.CreateUserAsync(model, Role.User);

        Assert.Null(result);
        Assert.NotNull(message);
        Assert.Equal(expectedError, message);
    }

    [Fact]
    public async Task ShouldRegister()
    {
        string username = "RegisterTest";
        string email = "registertest@imdb.com";
        string firstName = "Test first name";
        string lastName = "Test last name";

        var model = new CreateUserDto()
        {
            Username = username,
            Password = "pass",
            ConfirmPassword = "pass",
            FirstName = firstName,
            LastName = lastName,
            Email = email
        };

        var (result, message) = await _service.CreateUserAsync(model, Role.User);
        Assert.NotNull(result);
        Assert.Null(message);

        var (getResult, getMessage) = await _service.GetUserDetailsAsync(result);

        Assert.NotNull(getResult);
        Assert.Null(getMessage);

        Assert.Equal(username, getResult.Username);
        Assert.Equal(email, getResult.Email);
        Assert.Equal(firstName, getResult.FirstName);
        Assert.Equal(lastName, getResult.LastName);
        Assert.Equal(result, getResult.Id);
        Assert.Equal(Role.User, getResult.Role);

        // Business logic is that every user has a watchlist associated with it. We need to veryify that one is created when a user registers
        var (watchlist, watchlistMessage) = await _watchlistService.GetListDetailsAsync(result);

        Assert.Null(watchlistMessage);
        Assert.NotNull(watchlist);
    }
}