using System.ComponentModel.Composition;
using System.Windows.Documents;
using Waf.Writer.Applications.Services;
using Waf.Writer.Applications.Views;

namespace Waf.Writer.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class PrintPreviewViewModel : ZoomViewModel<IPrintPreviewView>
    {
        private IDocumentPaginatorSource document;
        
        
        [ImportingConstructor]
        public PrintPreviewViewModel(IPrintPreviewView view, IShellService shellService)
            : base(view, shellService)
        {
        }


        public IDocumentPaginatorSource Document
        {
            get { return document; }
            set { SetProperty(ref document, value); }
        }


        protected override void FitToWidthCore()
        {
            base.FitToWidthCore();
            ViewCore.FitToWidth();
        }
    }
}