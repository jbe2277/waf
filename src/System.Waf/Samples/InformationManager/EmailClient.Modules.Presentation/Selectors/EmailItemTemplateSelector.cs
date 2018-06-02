using System.Windows;
using System.Windows.Controls;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;

namespace Waf.InformationManager.EmailClient.Modules.Presentation.Selectors
{
    public class EmailItemTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var element = (FrameworkElement)container;
            var email = (Email)item;
            if (email.EmailType == EmailType.Received)
            {
                return (DataTemplate)element.FindResource("ReceivedEmailItemTemplate");
            }
            else
            {
                return (DataTemplate)element.FindResource("SentEmailItemTemplate");
            }
        }
    }
}
