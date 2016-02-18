using System.Collections.ObjectModel;
using System.Waf.Applications;
using System.Waf.Foundation;
using System.Windows.Input;
using Waf.Writer.Applications.Documents;
using Waf.Writer.Applications.Services;

namespace Waf.Writer.Presentation.DesignData
{
    public class MockFileService : Model, IFileService
    {
        public ReadOnlyObservableCollection<IDocument> Documents { get; set; }
        
        public IDocument ActiveDocument { get; set; }
        
        public RecentFileList RecentFileList { get; set; }
        
        public ICommand NewCommand { get; set; }
        
        public ICommand OpenCommand { get; set; }
        
        public ICommand CloseCommand { get; set; }
        
        public ICommand SaveCommand { get; set; }
        
        public ICommand SaveAsCommand { get; set; }
    }
}
