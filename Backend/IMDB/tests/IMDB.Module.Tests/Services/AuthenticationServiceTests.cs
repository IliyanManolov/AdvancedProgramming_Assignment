using IMDB.Application.Abstractions.Services;
using IMDB.Domain.Enums;
using IMDB.Module.Tests.Fixture;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDB.Module.Tests.Services;

[Collection("SharedCollection")]
public class AuthenticationServiceTests
{
    private readonly ModuleTestsInMemoryFixture _fixture;
    private readonly IAuthenticationService _service;
    private const string _errorMessage = "Invalid username or password";

    public AuthenticationServiceTests(ModuleTestsInMemoryFixture fixture)
    {
        _fixture = fixture;
        _service = _fixture.ServiceProvider.GetRequiredService<IAuthenticationService>();
    }

    [Theory]
    [InlineData("", "")] // Both empty
    [InlineData("LocalAdmin", "")] // Empty pass
    [InlineData("", "password")] // Empty username
    [InlineData("NonExistingUser", "password")] // Nonexisting user
    [InlineData("DeletedUser", "deletedpassword")] // Deleted user
    [InlineData("LocalAdmin", "invalidpassword")] // Invalid password
    [InlineData("LocalAdmin", "ADMIN")] // Invalid password casing
    public async Task ShouldFailAuthentication(string username, string password)
    {
        var (result, message) = await _service.AuthenticateAsync(username, password);

        Assert.Null(result);
        Assert.Equal(_errorMessage, message);
    }

    [Fact]
    public async Task ShouldAuthenticateSuccessfully()
    {
        string userName = "LocalAdmin";
        string password = "admin";
        var (result, message) = await _service.AuthenticateAsync(userName, password);

        Assert.NotNull(result);
        Assert.Null(message);

        Assert.Equal(userName, result.Username);
        Assert.Equal(Role.Administrator, result.Role);
        Assert.False(string.IsNullOrEmpty(result.FirstName), "First name returned as null/empty");
        Assert.False(string.IsNullOrEmpty(result.LastName), "Last name returned as null/empty");
        Assert.False(string.IsNullOrEmpty(result.Email), "Email returned as null/empty");
        Assert.NotNull(result.Id);

    }
}
