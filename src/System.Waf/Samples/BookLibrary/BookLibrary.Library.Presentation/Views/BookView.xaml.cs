using System.ComponentModel.Composition;
using Waf.BookLibrary.Library.Applications.Views;

namespace Waf.BookLibrary.Library.Presentation.Views
{
    [Export(typeof(IBookView))]
    public partial class BookView : IBookView
    {
        public BookView()
        {
            InitializeComponent();
        }
    }
}
