using IMDB.Application.Abstractions.Services;
using IMDB.Application.DTOs.Media;
using IMDB.Application.DTOs.Media.Movie;
using IMDB.Application.DTOs.Media.TvShow;
using IMDB.Application.DTOs.Review;
using IMDB.Domain.AbstractModels;
using IMDB.Domain.Models;
using IMDB.Module.Tests.Fixture;
using Microsoft.Extensions.DependencyInjection;

namespace IMDB.Module.Tests.Services;

[Collection("SharedCollection")]
public class MediaServiceTests
{
    private readonly ModuleTestsInMemoryFixture _fixture;
    private readonly IMediaService _service;

    public MediaServiceTests(ModuleTestsInMemoryFixture fixture)
    {
        _fixture = fixture;
        _service = _fixture.ServiceProvider.GetRequiredService<IMediaService>();
    }


    #region Movie tests

    public static List<object[]> ShouldFailToCreateMovie_Data => new()
    {
        new object[]
        {
            new CreateMovieDto()
            {
                Length = 3600,
                Description = "Random movie",
                Title = "Basic Movie Title",
                CreatedByUserId = 2
            },
            "already exists"
        },
        new object[]
        {
            new CreateMovieDto()
            {
                Length = 3600,
                Description = "Random movie",
                Title = "Test Movie",
                CreatedByUserId = 1
            },
            "UNAUTHORIZED"
        },
        new object[]
        {
            new CreateMovieDto()
            {
                Length = 3600,
                Description = "Random movie",
                Title = "Test Movie",
                CreatedByUserId = 3
            },
            "UNAUTHORIZED"
        },
        new object[]
        {
            new CreateMovieDto()
            {
                Length = 3600,
                Description = "Random movie",
                Title = "Test Movie",
                CreatedByUserId = 2,
                DirectorId = 100000
            },
            "Directors not found"
        },
        new object[]
        {
            new CreateMovieDto()
            {
                Length = 3600,
                Description = "Random movie",
                Title = "Test Movie",
                CreatedByUserId = 2,
                DirectorId = 1,
                GenreIds = new HashSet<long>(){ { 123456 } }
            },
            "Genres not found"
        },
        new object[]
        {
            new CreateMovieDto()
            {
                Length = 3600,
                Description = "Random movie",
                Title = "Test Movie",
                CreatedByUserId = 2,
                DirectorId = 1,
                GenreIds = new HashSet<long>(){ { 1 } },
                ActorIds = new HashSet<long>(){ { 123456 } }
            },
            "Actors not found"
        }
    };

    [Theory]
    [MemberData(nameof(ShouldFailToCreateMovie_Data))]
    public async Task ShouldFailToCreateMovie(CreateMovieDto model, string errorContains)
    {
        var (result, message) = await _service.CreateMovieAsync(model);

        Assert.Null(result);
        Assert.NotNull(message);

        Assert.Contains(errorContains, message, StringComparison.InvariantCultureIgnoreCase);
    }

    #endregion

    #region Show tests

    public static List<object[]> ShouldFailToCreateShow_Data => new()
    {
        new object[]
        {
            new CreateTvShowDto()
            {
                Description = "Random show",
                Title = "Basic Show Title",
                CreatedByUserId = 2
            },
            "already exists"
        },
        new object[]
        {
            new CreateTvShowDto()
            {
                Description = "Random show",
                Title = "Test Show",
                CreatedByUserId = 1
            },
            "UNAUTHORIZED"
        },
        new object[]
        {
            new CreateTvShowDto()
            {
                Description = "Random show",
                Title = "Test Show",
                CreatedByUserId = 3
            },
            "UNAUTHORIZED"
        },
        new object[]
        {
            new CreateTvShowDto()
            {
                Description = "Random show",
                Title = "Test Show",
                CreatedByUserId = 2,
                DirectorId = 100000
            },
            "Directors not found"
        },
        new object[]
        {
            new CreateTvShowDto()
            {
                Description = "Random show",
                Title = "Test Show",
                CreatedByUserId = 2,
                DirectorId = 1,
                GenreIds = new HashSet<long>(){ { 123456 } }
            },
            "Genres not found"
        },
        new object[]
        {
            new CreateTvShowDto()
            {
                Description = "Random show",
                Title = "Test Show",
                CreatedByUserId = 2,
                DirectorId = 1,
                GenreIds = new HashSet<long>(){ { 1 } },
                ActorIds = new HashSet<long>(){ { 123456 } }
            },
            "Actors not found"
        }
    };

    [Theory]
    [MemberData(nameof(ShouldFailToCreateShow_Data))]
    public async Task ShouldFailToCreateShow(CreateTvShowDto model, string errorContains)
    {
        var (result, message) = await _service.CreateTvShowAsync(model);

        Assert.Null(result);
        Assert.NotNull(message);

        Assert.Contains(errorContains, message, StringComparison.InvariantCultureIgnoreCase);
    }

    #endregion


    #region Reviews tests

    public static List<object[]> ShouldFailToCreateMovieReview_Data => new()
    {
        new object[]
        {
            new CreateReviewDto()
            {
                MediaType = MediaType.Movie,
                MediaId = 1,
                Rating = 5,
                Review = "Random review",
                UserId = 3
            },
            "unauthorized"
        },
        new object[]
        {
            new CreateReviewDto()
            {
                MediaType = MediaType.Movie,
                MediaId = 1,
                Rating = -5,
                Review = "Random review",
                UserId = 1
            },
            "Invalid value for rating"
        },
        new object[]
        {
            new CreateReviewDto()
            {
                MediaType = MediaType.Movie,
                MediaId = 1,
                Rating = 25,
                Review = "Random review",
                UserId = 1
            },
            "Invalid value for rating"
        },
        new object[]
        {
            new CreateReviewDto()
            {
                MediaType = MediaType.Movie,
                MediaId = 1234567,
                Rating = 10,
                Review = "Random review",
                UserId = 1
            },
            "Media not found"
        },
    };

    [Theory]
    [MemberData(nameof(ShouldFailToCreateMovieReview_Data))]
    public async Task ShouldFailToCreateMovieReview(CreateReviewDto model, string errorContains)
    {
        var (result, message) = await _service.CreateReview(model);

        Assert.Null(result);
        Assert.NotNull(message);

        Assert.Contains(errorContains, message, StringComparison.InvariantCultureIgnoreCase);
    }


    public static List<object[]> ShouldFailToCreateShowReview_Data => new()
    {
        new object[]
        {
            new CreateReviewDto()
            {
                MediaType = MediaType.TvShow,
                MediaId = 1,
                Rating = 5,
                Review = "Random review",
                UserId = 3
            },
            "unauthorized"
        },
        new object[]
        {
            new CreateReviewDto()
            {
                MediaType = MediaType.TvShow,
                MediaId = 1,
                Rating = -5,
                Review = "Random review",
                UserId = 1
            },
            "Invalid value for rating"
        },
        new object[]
        {
            new CreateReviewDto()
            {
                MediaType = MediaType.TvShow,
                MediaId = 1,
                Rating = 25,
                Review = "Random review",
                UserId = 1
            },
            "Invalid value for rating"
        },
        new object[]
        {
            new CreateReviewDto()
            {
                MediaType = MediaType.TvShow,
                MediaId = 1234567,
                Rating = 10,
                Review = "Random review",
                UserId = 1
            },
            "Media not found"
        },
    };

    [Theory]
    [MemberData(nameof(ShouldFailToCreateShowReview_Data))]
    public async Task ShouldFailToCreateShowReview(CreateReviewDto model, string errorContains)
    {
        var (result, message) = await _service.CreateReview(model);

        Assert.Null(result);
        Assert.NotNull(message);

        Assert.Contains(errorContains, message, StringComparison.InvariantCultureIgnoreCase);
    }

    public static List<object[]> ShouldFailToCreateEpisodeReview_Data => new()
    {
        new object[]
        {
            new CreateReviewDto()
            {
                MediaType = MediaType.Episode,
                MediaId = 1,
                Rating = 5,
                Review = "Random review",
                UserId = 3
            },
            "unauthorized"
        },
        new object[]
        {
            new CreateReviewDto()
            {
                MediaType = MediaType.Episode,
                MediaId = 1,
                Rating = -5,
                Review = "Random review",
                UserId = 1
            },
            "Invalid value for rating"
        },
        new object[]
        {
            new CreateReviewDto()
            {
                MediaType = MediaType.Episode,
                MediaId = 1,
                Rating = 25,
                Review = "Random review",
                UserId = 1
            },
            "Invalid value for rating"
        },
        new object[]
        {
            new CreateReviewDto()
            {
                MediaType = MediaType.Episode,
                MediaId = 1234567,
                Rating = 10,
                Review = "Random review",
                UserId = 1
            },
            "Media not found"
        },
    };

    [Theory]
    [MemberData(nameof(ShouldFailToCreateEpisodeReview_Data))]
    public async Task ShouldFailToCreateEpisodeReview(CreateReviewDto model, string errorContains)
    {
        var (result, message) = await _service.CreateReview(model);

        Assert.Null(result);
        Assert.NotNull(message);

        Assert.Contains(errorContains, message, StringComparison.InvariantCultureIgnoreCase);
    }

    #endregion
}