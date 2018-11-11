using System;
using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Windows;
using Waf.Writer.Applications.ViewModels;
using Waf.Writer.Applications.Views;

namespace Waf.Writer.Presentation.Views
{
    [Export(typeof(IPrintPreviewView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class PrintPreviewView : IPrintPreviewView
    {
        private readonly Lazy<PrintPreviewViewModel> viewModel;
        
        public PrintPreviewView()
        {
            InitializeComponent();

            viewModel = new Lazy<PrintPreviewViewModel>(() => ViewHelper.GetViewModel<PrintPreviewViewModel>(this));
            Loaded += FirstTimeLoadedHandler;
            IsVisibleChanged += IsVisibleChangedHandler;
        }

        private PrintPreviewViewModel ViewModel => viewModel.Value;


        public void FitToWidth()
        {
            documentViewer.FitToWidth();
        }

        private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        {
            // Ensure that this handler is called only once.
            Loaded -= FirstTimeLoadedHandler;
            documentViewer.Focus();
        }

        private void IsVisibleChangedHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            ViewModel.IsVisible = IsVisible;
        }
    }
}
