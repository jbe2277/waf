namespace Waf.NewsReader.Applications.Services;

public interface IWebStorageService : INotifyPropertyChanged
{
    UserAccount? CurrentAccount { get; }

    Task<bool> TrySilentSignIn();

    Task SignIn();

    Task SignOut();

    Task<(Stream? stream, string? cTag)> DownloadFile(string? cTag);

    Task<string?> UploadFile(Stream source);
}

public sealed record UserAccount(string UserName, string Email);
