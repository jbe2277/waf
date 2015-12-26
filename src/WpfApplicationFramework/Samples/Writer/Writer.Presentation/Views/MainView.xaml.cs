using System.Linq;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using Waf.Writer.Applications.Views;
using System;
using Waf.Writer.Applications.ViewModels;
using System.Collections.Generic;
using System.Waf.Applications;
using System.Windows;
using Waf.Writer.Presentation.Converters;
using System.Globalization;

namespace Waf.Writer.Presentation.Views
{
    [Export(typeof(IMainView))]
    public partial class MainView : UserControl, IMainView
    {
        private ContentViewState contentViewState;
        

        public MainView()
        {
            InitializeComponent();
            VisualStateManager.GoToElementState(rootContainer, ContentViewState.ToString(), false);
        }


        public ContentViewState ContentViewState
        {
            get { return contentViewState; }
            set
            {
                if (contentViewState != value)
                {
                    contentViewState = value;
                    VisualStateManager.GoToElementState(rootContainer, value.ToString(), true);
                }
            }
        }
    }
}
