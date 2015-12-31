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
    public partial class LendToWindow : Window, ILendToView
    {
        private readonly Lazy<LendToViewModel> viewModel;
        
        
        public LendToWindow()
        {
            InitializeComponent();

            viewModel = new Lazy<LendToViewModel>(() => ViewHelper.GetViewModel<LendToViewModel>(this));
        }


        private LendToViewModel ViewModel => viewModel.Value;


        public void ShowDialog(object owner)
        {
            Owner = owner as Window;
            ShowDialog();
        }

        private void PersonsListMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var element = e.OriginalSource as FrameworkElement;
            if (element?.DataContext is Person)
            {
                ViewModel.OkCommand.Execute(null);
            }
        }
    }
}
