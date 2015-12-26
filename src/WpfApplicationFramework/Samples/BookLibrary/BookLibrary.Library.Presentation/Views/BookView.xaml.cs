using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Controls;
using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Applications.Views;
using System;

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
