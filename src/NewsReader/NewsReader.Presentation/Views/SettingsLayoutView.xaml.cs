using Jbe.NewsReader.Applications.ViewModels;
using Jbe.NewsReader.Applications.Views;
using System;
using System.ComponentModel;
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
            viewModel = new Lazy<SettingsLayoutViewModel>(() => InitializeViewModel((SettingsLayoutViewModel)DataContext));
            pivot.Items.Remove(developerPivotItem);
        }


        public SettingsLayoutViewModel ViewModel => viewModel.Value;


        private SettingsLayoutViewModel InitializeViewModel(SettingsLayoutViewModel viewModelToInit)
        {
            viewModelToInit.PropertyChanged += ViewModelPropertyChanged;
            return viewModelToInit;
        }

        private void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SettingsLayoutViewModel.DeveloperSettingsEnabled))
            {
                if (ViewModel.DeveloperSettingsEnabled) { pivot.Items.Add(developerPivotItem); }
                else { pivot.Items.Remove(developerPivotItem); }
            }
        }
    }
}
