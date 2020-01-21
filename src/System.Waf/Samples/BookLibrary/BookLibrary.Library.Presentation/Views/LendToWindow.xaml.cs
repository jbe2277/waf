using System;
using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Input;
using Waf.BookLibrary.Library.Applications.ViewModels;
using Waf.BookLibrary.Library.Applications.Views;
using Waf.BookLibrary.Library.Domain;

namespace Waf.BookLibrary.Library.Presentation.Views
{
    [Export(typeof(ILendToView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class LendToWindow : ILendToView
    {
        private readonly Lazy<LendToViewModel> viewModel;
        
        public LendToWindow()
        {
            InitializeComponent();
            viewModel = new Lazy<LendToViewModel>(() => this.GetViewModel<LendToViewModel>()!);
        }

        private LendToViewModel ViewModel => viewModel.Value;

        public void ShowDialog(object owner)
        {
            Owner = owner as Window;
            ShowDialog();
        }

        private void PersonsListMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is FrameworkElement element && element?.DataContext is Person)
            {
                ViewModel.OkCommand.Execute(null);
            }
        }
    }
}
