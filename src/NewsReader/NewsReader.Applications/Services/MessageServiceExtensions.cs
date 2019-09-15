using System.Globalization;
using System.Threading.Tasks;

namespace Waf.NewsReader.Applications.Services
{
    public static class MessageServiceExtensions
    {
        public static Task ShowMessage(this IMessageService service, string format, params object[] args)
        {
            return service.ShowMessage(string.Format(CultureInfo.CurrentCulture, format, args));
        }

        public static Task<bool> ShowYesNoQuestion(this IMessageService service, string format, params object[] args)
        {
            return service.ShowYesNoQuestion(string.Format(CultureInfo.CurrentCulture, format, args));
        }
    }
}
