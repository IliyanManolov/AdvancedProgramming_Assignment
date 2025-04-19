using IMDB.Application.Abstractions.Repositories;
using IMDB.Application.Abstractions.Services;
using IMDB.Application.DTOs.Actor;
using IMDB.Module.Tests.Fixture;
using Microsoft.Extensions.DependencyInjection;

namespace IMDB.Module.Tests.Services;

[Collection("SharedCollection")]
public class ActorServiceTests
{
    private readonly ModuleTestsInMemoryFixture _fixture;
    private readonly IActorService _service;
    private readonly IActorRepository _repository;

    public ActorServiceTests(ModuleTestsInMemoryFixture fixture)
    {
        _fixture = fixture;
        _service = _fixture.ServiceProvider.GetRequiredService<IActorService>();
        _repository = _fixture.ServiceProvider.GetRequiredService<IActorRepository>();
    }

    [Theory]
    [InlineData(1, "TestFirstName", "TestLastName", "200-01-01", "UNAUTHORIZED")]
    [InlineData(2, "John", "Doe", "2000-01-01", "Actor John Doe already exists")]
    [InlineData(2, "TestFirstName", "TestLastName", "2050-01-01", "Invalid birth date")]
    [InlineData(2, "TestFirstName", "TestLastName", null, "Invalid birth date")]
    public async Task ShouldFailToCreate(long userId, string firstName, string lastName, string? dateTimeString, string errorContains)
    {
        var createDto = new CreateActorDto()
        {
            CreatedByUserId = userId,
            FirstName = firstName,
            LastName = lastName,
            BirthDate = dateTimeString is null ? null : DateTime.Parse(dateTimeString),
            Nationality = "Bulgarian"
        };

        var (result, message) = await _service.CreateAsync(createDto);

        Assert.Null(result);
        Assert.NotNull(message);

        Assert.Contains(errorContains, message, StringComparison.InvariantCultureIgnoreCase);
    }

    [Fact]
    public async Task ShouldCreateNewActor()
    {
        string firstName = "SuccessActorFirstName";
        string lastName = "SuccessActorLastName";
        string biography = "A random biography";
        var createDto = new CreateActorDto()
        {
            CreatedByUserId = 2,
            FirstName = firstName,
            LastName = lastName,
            BirthDate = DateTime.Parse("2000-01-01"),
            Nationality = "Bulgarian",
            Biography = biography
        };

        var (result, message) = await _service.CreateAsync(createDto);

        Assert.NotNull(result);
        Assert.Null(message);

        // Get the created genre and validate its name to make sure it was actually created
        var getResult = await _repository.GetByIdAsync(result);

        Assert.NotNull(getResult);
        
        Assert.Equal(firstName, getResult.FirstName);
        Assert.Equal(lastName, getResult.LastName);
        Assert.Equal(biography, getResult.Biography);
    }

    [Fact]
    public async Task ShouldGetAll()
    {
        var result = await _service.GetAllAsync();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.True(result.Count() > 2);

        foreach (var item in result)
        {
            Assert.False(string.IsNullOrEmpty(item.FirstName), "Empty/null first name found");
            Assert.False(string.IsNullOrEmpty(item.LastName), "Empty/null last name found");
            Assert.Equal($"{item.FirstName} {item.LastName}", item.FullName);
            Assert.NotNull(item.BirthDate);
            Assert.NotNull(item.Id);
        }
    }
}