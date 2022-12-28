namespace Waf.NewsReader.Applications.Services;

public interface IMessageService
{
    Task ShowMessage(string message);

    Task<bool> ShowYesNoQuestion(string message);
}
