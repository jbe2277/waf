using System.IO;
using System.IO.Packaging;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;
using System.Windows.Xps.Serialization;
using Waf.Writer.Applications.ViewModels;
using Waf.Writer.Applications.Views;

namespace Waf.Writer.Presentation.Views;

public partial class PrintPreviewView : IPrintPreviewView
{
    private const string PackagePath = "pack://temp.xps";
    private readonly Lazy<PrintPreviewViewModel> viewModel;
    private Package? package;
    private XpsDocument? xpsDocument;

    public PrintPreviewView()
    {
        InitializeComponent();
        viewModel = new(() => ViewHelper.GetViewModel<PrintPreviewViewModel>(this)!);
        Loaded += LoadedHandler;
        Unloaded += UnloadedHandler;
        IsVisibleChanged += IsVisibleChangedHandler;
    }

    private PrintPreviewViewModel ViewModel => viewModel.Value;

    public void FitToWidth() => documentViewer.FitToWidth();

    private void LoadedHandler(object sender, RoutedEventArgs e)
    {
        // We have to clone the FlowDocument before we use different pagination settings for the export.        
        var clone = (FlowDocument)ViewModel.Document.CloneContent();
        clone.ColumnWidth = double.PositiveInfinity;

        var packageStream = new MemoryStream();
        package = Package.Open(packageStream, FileMode.Create, FileAccess.ReadWrite);
        PackageStore.AddPackage(new Uri(PackagePath), package);
        xpsDocument = new XpsDocument(package, CompressionOption.SuperFast, PackagePath);
        using (var policy = new XpsPackagingPolicy(xpsDocument))
        using (var serializer = new XpsSerializationManager(policy, false))
        {
            var paginator = ((IDocumentPaginatorSource)clone).DocumentPaginator;
            serializer.SaveAsXaml(paginator);
        }
        documentViewer.Document = xpsDocument.GetFixedDocumentSequence();
        documentViewer.Focus();
    }

    private void UnloadedHandler(object sender, RoutedEventArgs e)
    {
        xpsDocument?.Close();
        PackageStore.RemovePackage(new Uri(PackagePath));
        package?.Close();
    }

    private void IsVisibleChangedHandler(object sender, DependencyPropertyChangedEventArgs e) => ViewModel.IsVisible = IsVisible;
}
