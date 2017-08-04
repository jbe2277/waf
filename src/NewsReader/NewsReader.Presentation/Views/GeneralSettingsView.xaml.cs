using Jbe.NewsReader.Applications.ViewModels;
using Jbe.NewsReader.Applications.Views;
using System;
using System.Composition;

namespace Jbe.NewsReader.Presentation.Views
{
    [Export(typeof(IGeneralSettingsView)), Shared]
    public sealed partial class GeneralSettingsView : IGeneralSettingsView
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
