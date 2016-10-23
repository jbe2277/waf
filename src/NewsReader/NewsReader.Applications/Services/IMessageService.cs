using System.Threading.Tasks;

namespace Jbe.NewsReader.Applications.Services
{
    public interface IMessageService
    {
        Task ShowMessageAsync(string message);

        Task<bool> ShowYesNoQuestionAsync(string message);
    }
}
