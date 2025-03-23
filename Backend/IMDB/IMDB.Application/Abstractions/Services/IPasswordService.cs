namespace IMDB.Application.Abstractions.Services;

public interface IPasswordService
{
    public string GetHash(string text);
}
