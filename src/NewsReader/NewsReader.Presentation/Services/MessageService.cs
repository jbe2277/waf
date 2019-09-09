using System.Threading.Tasks;
using Waf.NewsReader.Applications.Services;
using Xamarin.Forms;

namespace Waf.NewsReader.Presentation.Services
{
    public class MessageService : IMessageService
    {
        public Task ShowMessageAsync(string message)
        {
            return Application.Current.MainPage.DisplayAlert("Info", message, "OK");
        }

        public Task<bool> ShowYesNoQuestionAsync(string message)
        {
            return Application.Current.MainPage.DisplayAlert("Question", message, "No", "Yes");
        }
    }
}
