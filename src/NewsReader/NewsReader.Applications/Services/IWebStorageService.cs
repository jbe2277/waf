using System.IO;
using System.Threading.Tasks;

namespace Jbe.NewsReader.Applications.Services
{
    public interface IWebStorageService
    {
        Task<bool> DownloadFileAsync(string fileName, Stream destination, string token);

        Task UploadFileAsync(Stream source, string fileName, string token);
    }
}
