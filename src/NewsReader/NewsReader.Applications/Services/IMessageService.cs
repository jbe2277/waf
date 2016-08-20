using System.Threading.Tasks;

namespace Jbe.NewsReader.Applications.Services
{
    public interface IMessageService
    {
        Task<bool> ShowYesNoQuestionAsync(string message);
    }
}
