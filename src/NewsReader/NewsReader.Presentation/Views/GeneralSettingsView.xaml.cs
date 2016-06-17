using Jbe.NewsReader.Applications.ViewModels;
using Jbe.NewsReader.Applications.Views;
using System;
using System.Composition;
using Windows.UI.Xaml.Controls;

namespace Jbe.NewsReader.Presentation.Views
{
    [Export(typeof(IGeneralSettingsView)), Shared]
    public sealed partial class GeneralSettingsView : UserControl, IGeneralSettingsView
    {
        private readonly Lazy<GeneralSettingsViewModel> viewModel;


        public GeneralSettingsView()
        {
            InitializeComponent();
            viewModel = new Lazy<GeneralSettingsViewModel>(() => (GeneralSettingsViewModel)DataContext);
        }


        public GeneralSettingsViewModel ViewModel => viewModel.Value;
    }
}
