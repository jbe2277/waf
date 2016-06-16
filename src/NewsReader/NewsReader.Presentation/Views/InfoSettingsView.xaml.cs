using Jbe.NewsReader.Applications.ViewModels;
using Jbe.NewsReader.Applications.Views;
using System;
using System.Composition;
using Windows.UI.Xaml.Controls;

namespace Jbe.NewsReader.Presentation.Views
{
    [Export(typeof(IInfoSettingsView)), Shared]
    public sealed partial class InfoSettingsView : UserControl, IInfoSettingsView
    {
        private readonly Lazy<InfoSettingsViewModel> viewModel;


        public InfoSettingsView()
        {
            InitializeComponent();
            viewModel = new Lazy<InfoSettingsViewModel>(() => (InfoSettingsViewModel)DataContext);
        }


        public InfoSettingsViewModel ViewModel => viewModel.Value;
    }
}
