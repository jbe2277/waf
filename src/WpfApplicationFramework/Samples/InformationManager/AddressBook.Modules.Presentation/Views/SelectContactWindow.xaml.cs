using System;
using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Input;
using Waf.InformationManager.AddressBook.Modules.Applications.ViewModels;
using Waf.InformationManager.AddressBook.Modules.Applications.Views;
using Waf.InformationManager.AddressBook.Modules.Domain;

namespace Waf.InformationManager.AddressBook.Modules.Presentation.Views
{
    [Export(typeof(ISelectContactView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class SelectContactWindow : Window, ISelectContactView
    {
        private readonly Lazy<SelectContactViewModel> viewModel;
        

        public SelectContactWindow()
        {
            InitializeComponent();
            viewModel = new Lazy<SelectContactViewModel>(() => ViewHelper.GetViewModel<SelectContactViewModel>(this));
        }


        public SelectContactViewModel ViewModel { get { return viewModel.Value; } }


        public void ShowDialog(object owner)
        {
            Owner = owner as Window;
            ShowDialog();
        }

        private void ContactListViewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var element = e.OriginalSource as FrameworkElement;
            if (element != null && element.DataContext is Contact)
            {
                if (ViewModel.OkCommand.CanExecute(null)) { ViewModel.OkCommand.Execute(null); }
            }
        }
    }
}
