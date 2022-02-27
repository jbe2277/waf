using Waf.InformationManager.EmailClient.Modules.Applications.Views;
using System.ComponentModel.Composition;

namespace Waf.InformationManager.EmailClient.Modules.Presentation.Views;

[Export(typeof(IEmailView)), PartCreationPolicy(CreationPolicy.NonShared)]
public partial class EmailView : IEmailView
{
    public EmailView()
    {
        InitializeComponent();
    }
}
