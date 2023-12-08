using Waf.NewsReader.Applications.Services;

namespace Test.NewsReader.Applications.Services;

public class MockWebStorageService : Model, IWebStorageService
{
    private UserAccount? currentAccount;

    public UserAccount? CurrentAccount { get => currentAccount; set => SetProperty(ref currentAccount, value); }

    public Task<(Stream? stream, string? cTag)> DownloadFile(string? cTag) => throw new NotImplementedException();

    public Task SignIn() => throw new NotImplementedException();

    public Task SignOut() => throw new NotImplementedException();

    public Task<bool> TrySilentSignIn() => Task.FromResult(false);

    public Task<string?> UploadFile(Stream source) => throw new NotImplementedException();
}
