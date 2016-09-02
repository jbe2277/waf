using System;
using System.Threading.Tasks;

namespace Jbe.NewsReader.Applications.Services
{
    public interface ILauncherService
    {
        Task<bool> LaunchUriAsync(Uri uri);
    }
}
