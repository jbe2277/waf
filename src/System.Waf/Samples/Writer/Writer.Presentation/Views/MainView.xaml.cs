using System.ComponentModel.Composition;
using System.Windows.Controls;
using Waf.Writer.Applications.Views;
using System.Windows;

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
