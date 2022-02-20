using System.ComponentModel.Composition;
using System.Windows;
using Waf.Writer.Applications.Views;
using Waf.Writer.Applications.ViewModels;
using System.Waf.Applications;

namespace Waf.Writer.Presentation.Views;

[Export(typeof(IStartView))]
public partial class StartView : IStartView
{
    private readonly Lazy<StartViewModel> viewModel;

    public StartView()
    {
        InitializeComponent();
        viewModel = new Lazy<StartViewModel>(() => ViewHelper.GetViewModel<StartViewModel>(this)!);
        newButton.IsVisibleChanged += NewButtonIsVisibleChanged;
    }

    private StartViewModel ViewModel => viewModel.Value;

    private void NewButtonIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (newButton.IsVisible) newButton.Focus();
    }

    private void OpenContextMenuHandler(object sender, RoutedEventArgs e)
    {
        var recentFile = (RecentFile)((FrameworkElement)sender).DataContext;
        ViewModel.FileService.OpenCommand.Execute(recentFile.Path);
    }

    private void PinContextMenuHandler(object sender, RoutedEventArgs e)
    {
        var recentFile = (RecentFile)((FrameworkElement)sender).DataContext;
        recentFile.IsPinned = true;
    }

    private void UnpinContextMenuHandler(object sender, RoutedEventArgs e)
    {
        var recentFile = (RecentFile)((FrameworkElement)sender).DataContext;
        recentFile.IsPinned = false;
    }

    private void RemoveContextMenuHandler(object sender, RoutedEventArgs e)
    {
        var recentFile = (RecentFile)((FrameworkElement)sender).DataContext;
        ViewModel.FileService.RecentFileList.Remove(recentFile);
    }
}
