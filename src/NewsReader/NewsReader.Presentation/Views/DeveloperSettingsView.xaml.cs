using Jbe.NewsReader.Applications.ViewModels;
using Jbe.NewsReader.Applications.Views;
using System;
using System.Composition;
using Windows.UI.Xaml.Controls;

namespace Jbe.NewsReader.Presentation.Views
{
    [Export(typeof(IDeveloperSettingsView)), Shared]
    public sealed partial class DeveloperSettingsView : UserControl, IDeveloperSettingsView
    {
        private readonly Lazy<DeveloperSettingsViewModel> viewModel;


        public DeveloperSettingsView()
        {
            InitializeComponent();
            viewModel = new Lazy<DeveloperSettingsViewModel>(() => (DeveloperSettingsViewModel)DataContext);
        }


        public DeveloperSettingsViewModel ViewModel => viewModel.Value;
    }
}
