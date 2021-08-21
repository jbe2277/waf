using Waf.InformationManager.AddressBook.Modules.Applications.Views;
using System.ComponentModel.Composition;

namespace Waf.InformationManager.AddressBook.Modules.Presentation.Views
{
    [Export(typeof(IContactLayoutView))]
    public partial class ContactLayoutView : IContactLayoutView
    {
        public ContactLayoutView()
        {
            InitializeComponent();
        }
    }
}
