using System.Runtime.CompilerServices;

namespace Waf.NewsReader.Presentation.Controls;

public class LayoutPage : ContentPage
{
    public static readonly BindableProperty HeaderProperty = BindableProperty.Create(nameof(Header), typeof(View), typeof(LayoutPage), null);

    public static readonly BindableProperty FooterProperty = BindableProperty.Create(nameof(Footer), typeof(View), typeof(LayoutPage), null);

    LayoutPageGrid? pageGrid;

    public LayoutPage()
    {
        NavigationPage.SetHasNavigationBar(this, false);
        ControlTemplate = new ControlTemplate(() => 
        {
            pageGrid = new LayoutPageGrid();
            return pageGrid;
        });
    }

    public View? Header
    {
        get => (View?)GetValue(HeaderProperty); set => SetValue(HeaderProperty, value);
    }

    public View? Footer
    {
        get => (View?)GetValue(FooterProperty); set => SetValue(FooterProperty, value);
    }

    protected private LayoutPageGrid PageGrid => pageGrid ?? throw new InvalidOperationException("ControlTemplate was not yet applied. PageGrid is null.");

    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName is nameof(Header)) PageGrid.Header.Content = Header;
        if (propertyName is nameof(Footer)) PageGrid.Footer.Content = Footer;
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        PageGrid.BindingContext = BindingContext;
    }
}

[EditorBrowsable(EditorBrowsableState.Never)]  // Only for internal use
public sealed class LayoutPageGrid : Grid
{
    public LayoutPageGrid()
    {
        RowDefinitions = [new(GridLength.Auto), new(GridLength.Star), new(GridLength.Auto)];

        Header = new ContentView();
        SetRow((IView)Header, 0);
        Add(Header);

        var contentPresenter = new ContentPresenter();
        SetRow((IView)contentPresenter, 1);
        Add(contentPresenter);

        Footer = new ContentView();
        SetRow((IView)Footer, 2);
        Add(Footer);
    }

    public ContentView Header { get; }

    public ContentView Footer { get; }
}