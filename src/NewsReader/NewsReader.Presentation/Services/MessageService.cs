using Waf.NewsReader.Applications.Services;
using Waf.NewsReader.Presentation.Properties;

namespace Waf.NewsReader.Presentation.Services;

public class MessageService : IMessageService
{
    public Task ShowMessage(string message) => Application.Current!.MainPage!.DisplayAlert(Resources.Info, message, Resources.Ok);

    public Task<bool> ShowYesNoQuestion(string message) => Application.Current!.MainPage!.DisplayAlert(Resources.Question, message, Resources.Yes, Resources.No);
}
