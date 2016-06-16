using Jbe.NewsReader.Applications.ViewModels;
using Jbe.NewsReader.Applications.Views;
using System;
using System.Composition;
using Windows.UI.Xaml.Controls;

namespace Jbe.NewsReader.Presentation.Views
{
    [Export(typeof(ISettingsLayoutView)), Shared]
    public sealed partial class SettingsLayoutView : UserControl, ISettingsLayoutView
    {
        private readonly Lazy<SettingsLayoutViewModel> viewModel;


        public SettingsLayoutView()
        {
            InitializeComponent();
            viewModel = new Lazy<SettingsLayoutViewModel>(() => (SettingsLayoutViewModel)DataContext);
        }


        public SettingsLayoutViewModel ViewModel => viewModel.Value;
    }
}
