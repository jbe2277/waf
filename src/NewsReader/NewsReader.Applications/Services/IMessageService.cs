using System.Threading.Tasks;

namespace Jbe.NewsReader.Applications.Services
{
    public interface IMessageService
    {
        void ShowMessage(string message);

        Task ShowMessageDialogAsync(string message);

        Task<bool> ShowYesNoQuestionDialogAsync(string message);
    }
}
