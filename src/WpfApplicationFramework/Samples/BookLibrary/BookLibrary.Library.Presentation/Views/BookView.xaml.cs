using System.ComponentModel.Composition;
using System.Windows.Controls;
using Waf.BookLibrary.Library.Applications.Views;

namespace Waf.BookLibrary.Library.Presentation.Views
{
    [Export(typeof(IBookView))]
    public partial class BookView : UserControl, IBookView
    {
        public BookView()
        {
            InitializeComponent();
        }
    }
}
