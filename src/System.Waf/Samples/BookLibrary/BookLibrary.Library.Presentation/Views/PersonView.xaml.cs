using System.ComponentModel.Composition;
using Waf.BookLibrary.Library.Applications.Views;

namespace Waf.BookLibrary.Library.Presentation.Views;

[Export(typeof(IPersonView))]
public partial class PersonView : IPersonView
{
    public PersonView()
    {
        InitializeComponent();
    }
}
