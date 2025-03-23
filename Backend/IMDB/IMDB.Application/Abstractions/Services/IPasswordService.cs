namespace IMDB.Application.Abstractions.Services;

public interface IPasswordService
{
    public Task<string> GetHash(string text);
}
