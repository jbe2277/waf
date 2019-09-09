using System.Threading.Tasks;

namespace Waf.NewsReader.Applications.Services
{
    public interface IMessageService
    {
        Task ShowMessageAsync(string message);

        Task<bool> ShowYesNoQuestionAsync(string message);
    }
}
