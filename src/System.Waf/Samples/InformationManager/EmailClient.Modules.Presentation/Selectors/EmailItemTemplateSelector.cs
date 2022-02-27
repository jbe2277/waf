using System.Windows;
using System.Windows.Controls;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;

namespace Waf.InformationManager.EmailClient.Modules.Presentation.Selectors;

public class EmailItemTemplateSelector : DataTemplateSelector
{
    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        var element = (FrameworkElement)container;
        var email = (Email)item;
        return (DataTemplate)(email.EmailType == EmailType.Received ? element.FindResource("ReceivedEmailItemTemplate") : element.FindResource("SentEmailItemTemplate"));
    }
}
