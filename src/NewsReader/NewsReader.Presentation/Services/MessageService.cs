using Jbe.NewsReader.Applications.Services;
using Jbe.NewsReader.Applications.ViewModels;
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
        private readonly Lazy<ShellViewModel> shellViewModel;


        [ImportingConstructor]
        public MessageService(Lazy<ShellViewModel> shellViewModel)
        {
            this.shellViewModel = shellViewModel;
        }


        public void ShowMessage(string message)
        {
            shellViewModel.Value.ShowMessage(message, null);
        }

        public async Task ShowMessageDialogAsync(string message)
        {
            var messageDialog = new MessageDialog(message);
            var closeCommand = new UICommand(ResourceLoader.GetForViewIndependentUse().GetString("Close"));
            messageDialog.Commands.Add(closeCommand);
            messageDialog.DefaultCommandIndex = 0;
            messageDialog.CancelCommandIndex = 0;
            await messageDialog.ShowAsync();
        }

        public async Task<bool> ShowYesNoQuestionDialogAsync(string message)
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
