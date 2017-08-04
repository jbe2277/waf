using Jbe.NewsReader.Applications.ViewModels;
using Jbe.NewsReader.Applications.Views;
using System;
using System.Composition;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml.Input;

namespace Jbe.NewsReader.Presentation.Views
{
    [Export(typeof(IInfoSettingsView)), Shared]
    public sealed partial class InfoSettingsView : IInfoSettingsView
    {
        private readonly Lazy<InfoSettingsViewModel> viewModel;


        public InfoSettingsView()
        {
            InitializeComponent();
            viewModel = new Lazy<InfoSettingsViewModel>(() => (InfoSettingsViewModel)DataContext);
        }


        public InfoSettingsViewModel ViewModel => viewModel.Value;


        private void KeyDownHandler(object sender, KeyRoutedEventArgs e)
        {
            var coreWindow = CoreWindow.GetForCurrentThread();
            if (e.Key == VirtualKey.D && coreWindow.GetKeyState(VirtualKey.Control).HasFlag(CoreVirtualKeyStates.Down)
                && coreWindow.GetKeyState(VirtualKey.Shift).HasFlag(CoreVirtualKeyStates.Down))
            {
                ViewModel.EnableDeveloperSettingsCommand.Execute(null);
            }
        }

        private void HiddenRectangleDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            ViewModel.EnableDeveloperSettingsCommand.Execute(null);
        }
    }
}
