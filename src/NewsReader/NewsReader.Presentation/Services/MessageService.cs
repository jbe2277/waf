using System.Threading.Tasks;
using Waf.NewsReader.Applications.Services;
using Xamarin.Forms;

namespace Waf.NewsReader.Presentation.Services
{
    public class MessageService : IMessageService
    {
        public Task ShowMessage(string message)
        {
            return Application.Current.MainPage.DisplayAlert("Info", message, "OK");
        }

        public Task<bool> ShowYesNoQuestion(string message)
        {
            return Application.Current.MainPage.DisplayAlert("Question", message, "Yes", "No");
        }
    }
}
