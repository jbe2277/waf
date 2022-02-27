using System.Windows;

namespace LocalizationSample.Presentation;

public partial class ShellWindow
{
    public ShellWindow()
    {
        InitializeComponent();
    }

    private void CloseClick(object sender, RoutedEventArgs e) => Close();
}
