using IMDB.Application.Abstractions.Services;
using IMDB.Application.DTOs.Media;
using IMDB.Application.DTOs.Media.Movie;
using IMDB.Application.DTOs.Media.TvShow;
using IMDB.Application.DTOs.Review;
using IMDB.Domain.AbstractModels;
using IMDB.Domain.Models;
using IMDB.Module.Tests.Fixture;
using Microsoft.Extensions.DependencyInjection;
using OpenSearch.Client;

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

    [Theory]
    [InlineData(1)]
    public async Task ShouldGetMovieById(long id)
    {
        var (result, message) = await _service.GetMovieByIdAsync(id);

        Assert.NotNull(result);
        Assert.Null(message);
    }

    [Theory]
    [InlineData(123456)]
    public async Task ShouldFailToGetMovieById(long id)
    {
        string errorContains = "does not exist";
        var (result, message) = await _service.GetMovieByIdAsync(id);

        Assert.Null(result);
        Assert.NotNull(message);

        Assert.Contains(errorContains, message, StringComparison.InvariantCultureIgnoreCase);
    }

    [Fact]
    public async Task ShouldCreateMovie()
    {
        var model = new CreateMovieDto()
        {
            Length = 3600,
            Description = "Random movie",
            Title = "Test Create Movie",
            CreatedByUserId = 2,
            DirectorId = 1,
            GenreIds = new HashSet<long>() { { 1 } },
            ActorIds = new HashSet<long>() { { 1 } }
        };

        var (result, message) = await _service.CreateMovieAsync(model);

        Assert.NotNull(result);
        Assert.Null(message);

        var (final, finalError) = await _service.GetMovieByIdAsync(result);

        Assert.NotNull(final);
        Assert.Null(finalError);

        Assert.Equal(model.Length, final.Length);
        Assert.Equal(model.Title, final.Title);
        Assert.Equal(model.Description, final.Description);
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

    [Theory]
    [InlineData(1)]
    public async Task ShouldGetShowById(long id)
    {
        var (result, message) = await _service.GetShowByIdAsync(id);

        Assert.NotNull(result);
        Assert.Null(message);
    }

    [Theory]
    [InlineData(123456)]
    public async Task ShouldFailToGetShowById(long id)
    {
        string errorContains = "does not exist";
        var (result, message) = await _service.GetShowByIdAsync(id);

        Assert.Null(result);
        Assert.NotNull(message);

        Assert.Contains(errorContains, message, StringComparison.InvariantCultureIgnoreCase);
    }

    [Fact]
    public async Task ShouldCreateShow()
    {
        var model = new CreateTvShowDto()
        {
            Description = "Random show",
            Title = "Test Create Show",
            CreatedByUserId = 2,
            DirectorId = 1,
            GenreIds = new HashSet<long>() { { 1 } },
            ActorIds = new HashSet<long>() { { 1 } },
            SeasonsCount = 1
        };

        var (result, message) = await _service.CreateTvShowAsync(model);

        Assert.Null(message);
        Assert.NotNull(result);

        var (final, finalError) = await _service.GetShowByIdAsync(result);

        Assert.NotNull(final);
        Assert.Null(finalError);

        // Show without any episodes defaults to 0 length
        Assert.Equal(0, final.Length);
        Assert.Equal(0, final.ShowEpisodesCount);
        Assert.Equal(model.SeasonsCount, final.ShowSeasonsCount);
        Assert.Equal(model.Title, final.Title);
        Assert.Equal(model.Description, final.Description);
    }

    #endregion

    #region Episodes tests
    [Theory]
    [InlineData(1)]
    public async Task ShouldGetEpisodesByShowId(long id)
    {
        var (result, message) = await _service.GetShowEpisodes(id);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Null(message);
    }

    [Theory]
    [InlineData(123456)]
    public async Task ShouldFailToGetEpisodesByShowId(long id)
    {
        string errorContains = "does not exist";
        var (result, message) = await _service.GetShowEpisodes(id);

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

    [Theory]
    [InlineData(MediaType.Movie, 1)]
    [InlineData(MediaType.TvShow, 1)]
    [InlineData(MediaType.Episode, 1)]
    public async Task ShouldGetReviewsForMediaById(MediaType type, long id)
    {
        var model = new ReviewsRequestDto()
        {
            MediaId = id,
            MediaType = type,
        };

        var (result, message) = await _service.GetReviewsForMediaAsync(model);

        Assert.Null(message);
        Assert.NotEmpty(result);
    }

    [Theory]
    [InlineData(MediaType.Movie, 123456)]
    [InlineData(MediaType.TvShow, 123456)]
    [InlineData(MediaType.Episode, 123456)]
    public async Task ShouldFailToGetReviewsForMediaById(MediaType type, long id)
    {
        var model = new ReviewsRequestDto()
        {
            MediaId = id,
            MediaType = type,
        };

        var (result, message) = await _service.GetReviewsForMediaAsync(model);

        Assert.Null(message);
        Assert.Empty(result);
    }

    #endregion


    #region Miscellaneous tests 
    [Fact]
    public async Task ShouldGetTopThen()
    {
        var (result, message) = await _service.GetTopTenAsync();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Null(message);
    }

    [Fact]
    public async Task ShouldGetAllDiscovery()
    {
        var (result, message) = await _service.GetAllDiscoveryAsync();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Null(message);
    }

    [Fact]
    public async Task ShouldGetAllMovies()
    {
        var (result, message) = await _service.GetAllMoviesAsync();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Null(message);
    }

    [Fact]
    public async Task ShouldGetAllShows()
    {
        var (result, message) = await _service.GetAllShowsAsync();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Null(message);
    }


    [Theory]
    [InlineData(2, "Horror")]
    [InlineData(0, "Romantic")]
    public async Task ShouldGetAllWithGenre(int expectedCount, params string[] genres)
    {
        var (result, message) = await _service.GetAllWithGenres(genres);

        Assert.NotNull(result);
        Assert.Equal(expectedCount, result.Count());
        Assert.Null(message);
    }

    [Theory]
    [InlineData(1, "Horror", "InvalidGenre")]
    [InlineData(1, "InvalidGenre", "Horror")]
    [InlineData(1, "Horror", "InvalidGenre", "Romantic")]
    [InlineData(2, "InvalidGenre", "Another Invalid Genre", "Romantic")]
    public async Task ShouldFailToGetAllWithGenre(int expectedCount, params string[] genres)
    {
        var (result, message) = await _service.GetAllWithGenres(genres);

        Assert.Null(result);
        Assert.NotNull(message);
        Assert.Contains("Genre not found", message, StringComparison.InvariantCultureIgnoreCase);
        Assert.Equal(expectedCount, message.Split(';').Length);
    }

    #endregion
}