using System.Globalization;
using System.Threading.Tasks;

namespace Jbe.NewsReader.Applications.Services
{
    public static class MessageServiceExtensions
    {
        public static void ShowMessage(this IMessageService service, string format, params object[] args)
        {
            service.ShowMessage(string.Format(CultureInfo.CurrentCulture, format, args));
        }

        public static Task ShowMessageDialogAsync(this IMessageService service, string format, params object[] args)
        {
            return service.ShowMessageDialogAsync(string.Format(CultureInfo.CurrentCulture, format, args));
        }

        public static Task<bool> ShowYesNoQuestionDialogAsync(this IMessageService service, string format, params object[] args)
        {
            return service.ShowYesNoQuestionDialogAsync(string.Format(CultureInfo.CurrentCulture, format, args));
        }
    }
}
