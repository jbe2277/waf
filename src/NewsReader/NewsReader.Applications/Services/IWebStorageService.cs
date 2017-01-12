using System.IO;
using System.Threading.Tasks;

namespace Jbe.NewsReader.Applications.Services
{
    public interface IWebStorageService
    {
        Task<string> DownloadFileAsync(string fileName, Stream destination, string token, string eTag);

        Task<string> UploadFileAsync(Stream source, string fileName, string token);
    }
}
