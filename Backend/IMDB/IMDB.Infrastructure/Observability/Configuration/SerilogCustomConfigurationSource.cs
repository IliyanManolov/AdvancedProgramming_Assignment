using Microsoft.Extensions.Configuration;

namespace IMDB.Infrastructure.Observability.Configuration;

public class SerilogCustomConfigurationSource : IConfigurationSource
{
    private readonly IConfigurationRoot _configuration;

    public SerilogCustomConfigurationSource(IConfigurationRoot configuration)
    {
        _configuration = configuration;
    }
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new SerilogCustomConfigurationProvider(_configuration);
    }
}
