using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;

namespace Waf.NewsReader.Applications.Services
{
    public interface IWebStorageService : INotifyPropertyChanged
    {
        UserAccount CurrentAccount { get; }

        Task<bool> TrySilentSignIn();

        Task SignIn();

        Task SignOut();

        Task<(Stream stream, string cTag)> DownloadFileAsync(string fileName, string cTag);

        Task<string> UploadFileAsync(Stream source, string fileName);
    }

    public class UserAccount
    {
        public UserAccount(string userName, string email)
        {
            UserName = userName;
            Email = email;
        }

        public string UserName { get; }

        public string Email { get; }
    }
}
