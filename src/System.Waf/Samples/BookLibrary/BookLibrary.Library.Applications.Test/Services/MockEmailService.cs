using Waf.BookLibrary.Library.Applications.Services;

namespace Test.BookLibrary.Library.Applications.Services;

internal class MockEmailService : IEmailService
{
    public string? ToEmailAddress { get; set; }

    public void CreateNewEmail(string toEmailAddress) => ToEmailAddress = toEmailAddress;
}
