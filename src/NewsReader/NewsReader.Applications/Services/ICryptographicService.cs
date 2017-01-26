using System.IO;
using System.Threading.Tasks;

namespace Jbe.NewsReader.Applications.Services
{
    public interface ICryptographicService
    {
        Task<string> HashAsync(Stream stream);

        Task<Stream> EncryptAsync(Stream stream, string key, string salt, uint iterationCount);

        Task<Stream> DecryptAsync(Stream stream, string key, string salt, uint iterationCount);
    }
}
