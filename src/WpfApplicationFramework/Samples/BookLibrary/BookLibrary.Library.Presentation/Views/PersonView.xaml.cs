using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Waf.Applications;
using System.Windows.Controls;
using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Applications.Views;

namespace Waf.BookLibrary.Library.Presentation.Views
{
    [Export(typeof(IPersonView))]
    public partial class PersonView : UserControl, IPersonView
    {
        public PersonView()
        {
            InitializeComponent();
        }
    }
}
