using System.ComponentModel.Composition;
using Waf.Writer.Applications.Views;
using System.Windows;

namespace Waf.Writer.Presentation.Views;

[Export(typeof(IMainView))]
public partial class MainView : IMainView
{
    private ContentViewState contentViewState;

    public MainView()
    {
        InitializeComponent();
        VisualStateManager.GoToElementState(rootContainer, ContentViewState.ToString(), false);
    }

    public ContentViewState ContentViewState
    {
        get => contentViewState;
        set
        {
            if (contentViewState == value) return;
            contentViewState = value;
            VisualStateManager.GoToElementState(rootContainer, value.ToString(), true);
        }
    }
}
