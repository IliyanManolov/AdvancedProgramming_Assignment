using IMDB.Application.Abstractions.Services;
using IMDB.Application.DTOs.Media;
using IMDB.Application.DTOs.Review;
using IMDB.Domain.AbstractModels;
using IMDB.Domain.Models;
using IMDB.Module.Tests.Fixture;
using Microsoft.Extensions.DependencyInjection;

namespace IMDB.Module.Tests.Services;

[Collection("SharedCollection")]
public class MediaTransformerServiceTests
{
    private readonly ModuleTestsInMemoryFixture _fixture;
    private readonly IMediaTransformer _service;

    public MediaTransformerServiceTests(ModuleTestsInMemoryFixture fixture)
    {
        _fixture = fixture;
        _service = _fixture.ServiceProvider.GetRequiredService<IMediaTransformer>();
    }

    public static List<object[]> ShouldTransformToShort_Data => new()
    {
        new object[]
        {
            new MediaShortDto()
            {
                Length = 3600,
                Genres = new HashSet<string>(){ { "Thriller" }, { "Horror" } },
                Description = "Random movie",
                Rating = 2,
                Reviews = 2,
                Title = "Random title",
                Type = MediaType.Movie,
                Director = "John Doe"
            },
            new List<Media>()
            {
                new Movie()
                {
                    Rating = 2,
                    Reviews = new HashSet<Review>(){ { new Review() }, { new Review() } },
                    Title = "Random title",
                    Director = new Director() {FirstName = "John", LastName = "Doe" },
                    Description = "Random movie",
                    Length = 3600,
                    Genres = new HashSet<Genre>()
                    {
                        { new Genre(){ Name = "Horror" } },
                        { new Genre(){ Name = "Thriller" } }
                    }
                }
            }
        },
        new object[]
        {
            new MediaShortDto()
            {
                Length = 2200,
                Genres = new HashSet<string>(){ { "Thriller" }, { "Horror" } },
                Description = "Random show",
                Rating = 2,
                Reviews = 2,
                Title = "Random title",
                Type = MediaType.TvShow,
                Director = "John Doe",
                ReleaseDate = DateTime.Parse("2000-01-01"),
                ShowEpisodesCount = 2,
                ShowSeasonsCount = 1
            },
            new List<Media>()
            {
                new TvShow()
                {
                    Rating = 2,
                    Reviews = new HashSet<Review>(){ { new Review() }, { new Review() } },
                    Title = "Random title",
                    Director = new Director() {FirstName = "John", LastName = "Doe" },
                    Description = "Random show",
                    ReleaseDate = DateTime.Parse("2000-01-01"),
                    Episodes = new HashSet<ShowEpisode>()
                    {
                        { new ShowEpisode(){ Length = 1000 } },
                        { new ShowEpisode(){ Length = 1200 } }
                    },
                    Genres = new HashSet<Genre>()
                    {
                        { new Genre(){ Name = "Horror" } },
                        { new Genre(){ Name = "Thriller" } }
                    },
                    Seasons = 1
                }
            }
        }
    };

    [Theory]
    [MemberData(nameof(ShouldTransformToShort_Data))]
    public void ShouldTransformToShort(MediaShortDto expected, IEnumerable<Media> request)
    {
        var actual = _service.ToShortDto(request);

        Assert.Single(actual);

        Assert.Equivalent(expected, actual.First());
    }

    public static List<object[]> ShouldTransformReviewToDetails_Data => new()
    {
        new object[]
        {
            new ReviewDetailsDto()
            {
                CreatedDate = DateTime.Parse("2000-01-01"),
                LastEditDate = null,
                Rating = 7.5,
                ReviewText = "Random review",
                UserName = "Review Creator"
            },
            new List<Review>()
            {
                new Review()
                {
                    Rating = 7.5,
                    CreateTimeStamp = DateTime.Parse("2000-01-01"),
                    ReviewText = "Random review",
                    User = new User()
                    {
                        Username = "Review Creator"
                    }
                }
            }
        }
    };

    [Theory]
    [MemberData(nameof(ShouldTransformReviewToDetails_Data))]
    public void ShouldTransformReviewToDetails(ReviewDetailsDto expected, IEnumerable<Review> request)
    {
        var actual = _service.ToDetails(request);

        Assert.Single(actual);

        Assert.Equivalent(expected, actual.First());
    }
}