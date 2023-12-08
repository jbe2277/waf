using Waf.NewsReader.Applications.Services;

namespace Test.NewsReader.Applications.Services;

public class MockMessageService : IMessageService
{
    public Task ShowMessage(string message) => throw new NotImplementedException();

    public Task<bool> ShowYesNoQuestion(string message) => throw new NotImplementedException();
}
