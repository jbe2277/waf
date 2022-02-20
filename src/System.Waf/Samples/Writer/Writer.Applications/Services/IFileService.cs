using Waf.Writer.Applications.Documents;
using System.Windows.Input;
using System.Waf.Applications;

namespace Waf.Writer.Applications.Services
{
    public interface IFileService : INotifyPropertyChanged
    {
        ReadOnlyObservableCollection<IDocument> Documents { get; }

        IDocument? ActiveDocument { get; set; }

        RecentFileList RecentFileList { get; }

        ICommand NewCommand { get; }

        ICommand OpenCommand { get; }

        ICommand CloseCommand { get; }

        ICommand SaveCommand { get; }

        ICommand SaveAsCommand { get; }
    }
}
