using System.Windows.Controls;
using Waf.InformationManager.AddressBook.Modules.Applications.Views;
using System.ComponentModel.Composition;

namespace Waf.InformationManager.AddressBook.Modules.Presentation.Views
{
    [Export(typeof(IContactView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ContactView : UserControl, IContactView
    {
        public ContactView()
        {
            InitializeComponent();
        }
    }
}
