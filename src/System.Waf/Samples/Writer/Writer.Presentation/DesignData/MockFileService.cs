using System.Waf.Applications;
using System.Windows.Input;
using Waf.Writer.Applications.Documents;
using Waf.Writer.Applications.Services;

namespace Waf.Writer.Presentation.DesignData
{
    public class MockFileService : Model, IFileService
    {
        public ReadOnlyObservableCollection<IDocument> Documents { get; set; } = null!;
        
        public IDocument? ActiveDocument { get; set; }

        public RecentFileList RecentFileList { get; set; } = null!;

        public ICommand NewCommand { get; set; } = DelegateCommand.DisabledCommand;
        
        public ICommand OpenCommand { get; set; } = DelegateCommand.DisabledCommand;

        public ICommand CloseCommand { get; set; } = DelegateCommand.DisabledCommand;

        public ICommand SaveCommand { get; set; } = DelegateCommand.DisabledCommand;

        public ICommand SaveAsCommand { get; set; } = DelegateCommand.DisabledCommand;
    }
}
