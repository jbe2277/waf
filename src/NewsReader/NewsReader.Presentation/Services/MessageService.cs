using Jbe.NewsReader.Applications.Services;
using System;
using System.Composition;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;

namespace Jbe.NewsReader.Presentation.Services
{
    [Export(typeof(IMessageService)), Export, Shared]
    internal class MessageService : IMessageService
    {
        public async Task<bool> ShowYesNoQuestionAsync(string message)
        {
            var messageDialog = new MessageDialog(message);
            var yesCommand = new UICommand(ResourceLoader.GetForViewIndependentUse().GetString("Yes"));
            var noCommand = new UICommand(ResourceLoader.GetForViewIndependentUse().GetString("No"));
            messageDialog.Commands.Add(yesCommand);
            messageDialog.Commands.Add(noCommand);
            messageDialog.DefaultCommandIndex = 0;
            messageDialog.CancelCommandIndex = 1;
            var result = await messageDialog.ShowAsync();
            return result == yesCommand;
        }    
    }
}
