using Waf.NewsReader.Applications.Services;
using Waf.NewsReader.Presentation.Properties;

namespace Waf.NewsReader.Presentation.Services;

public class MessageService : IMessageService
{
    public Task ShowMessage(string message) => Application.Current!.Windows[0].Page!.DisplayAlert(Resources.Info, message, Resources.Ok);

    public Task<bool> ShowYesNoQuestion(string message) => Application.Current!.Windows[0].Page!.DisplayAlert(Resources.Question, message, Resources.Yes, Resources.No);
}
