using IMDB.Application.Abstractions.Services;
using System.Security.Cryptography;
using System.Text;

namespace IMDB.Application.Services;

class PasswordService : IPasswordService
{
    public string GetHash(string text)
    {
        var bytes = Encoding.UTF8.GetBytes(text);

        var hashBytes = SHA256.HashData(bytes);
        return BitConverter.ToString(hashBytes);
    }
}
